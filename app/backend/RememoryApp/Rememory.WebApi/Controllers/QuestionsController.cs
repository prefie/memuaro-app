using HtmlRendererCore.PdfSharp;
using Rememory.Auth;
using Rememory.Persistance.Entities;
using Rememory.Persistance.Models;
using Rememory.Persistance.Repositories.GlobalQuestionRepository;
using Rememory.Persistance.Repositories.QuestionRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore;
using Rememory.Email;
using Rememory.Persistance.Repositories.UserRepository;
using Rememory.Persistance.Repositories.UserSettingsRepository;
using Rememory.WebApi.Dtos.Question;
using Rememory.WebApi.Exceptions;

namespace Rememory.WebApi.Controllers;

[Authorize]
public class QuestionsController : BaseController
{
    private readonly IGlobalQuestionRepository _globalQuestionRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IUserSettingsRepository _userSettingsRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEmailClient _emailClient;

    public QuestionsController(
        IGlobalQuestionRepository globalQuestionRepository,
        IQuestionRepository questionRepository,
        AuthProvider authProvider,
        IUserRepository userRepository,
        IUserSettingsRepository userSettingsRepository,
        IEmailClient emailClient) : base(authProvider)
    {
        _globalQuestionRepository = globalQuestionRepository;
        _questionRepository = questionRepository;
        _emailClient = emailClient;
        _userRepository = userRepository;
        _userSettingsRepository = userSettingsRepository;
    }

    [HttpGet]
    [Route("global")]
    public async Task<ActionResult<GlobalQuestionsDto>> GetGlobalQuestions(
        [FromQuery] GetGlobalQuestionsRequestDto request)
    {
        var globalQuestionIds = new HashSet<Guid>();
        if (request.UserId.HasValue)
        {
            CheckAccessForUser(request.UserId.Value);

            var questions = await _questionRepository.GetForUserAsync(request.UserId.Value);
            foreach (var question in questions)
                globalQuestionIds.Add(question.GlobalQuestionId);
        }

        var globalQuestions = await _globalQuestionRepository.GetGlobalQuestionWithExcept(globalQuestionIds,
            request.CategoryIds?.ToHashSet());

        var globalQuestionsDto = globalQuestions
            .Select(gq => new GlobalQuestionDto(gq))
            .ToList();

        return Ok(new GlobalQuestionsDto {GlobalQuestions = globalQuestionsDto});
    }

    [HttpGet]
    [Route("new")]
    public async Task<ActionResult<QuestionsDto>> GetNewQuestions([FromQuery] Guid userId,
        [FromQuery] Guid[] globalQuestionIds)
    {
        CheckAccessForUser(userId);

        var questions = new List<QuestionDto>();
        foreach (var globalQuestionId in globalQuestionIds)
        {
            var globalQuestion = await _globalQuestionRepository.GetAsync(globalQuestionId);
            if (globalQuestion == null)
                continue;

            var userQuestion = new Question
            {
                Id = Guid.NewGuid(), GlobalQuestionId = globalQuestion.Id, Title = globalQuestion.Title,
                CategoryId = globalQuestion.CategoryId, UserId = userId, Status = Status.Unanswered
            };

            await _questionRepository.CreateAsync(userQuestion);
            questions.Add(new QuestionDto(userQuestion));
        }

        return Ok(new QuestionsDto {Questions = questions});
    }

    [HttpGet]
    [Route("")]
    public async Task<ActionResult<QuestionsDto>> GetAllQuestionsForUser([FromQuery] Guid userId)
    {
        CheckAccessForUser(userId);

        var allQuestions = await _questionRepository.GetForUserAsync(userId);
        var questions = allQuestions
            .Select(question => new QuestionDto(question))
            .ToList();

        return Ok(new QuestionsDto {Questions = questions});
    }

    [HttpPatch]
    [Route("{id:guid}")]
    public async Task<ActionResult<QuestionDto>> PatchAnswer(Guid id, [FromBody] AnswerRequestDto request)
    {
        var question = await _questionRepository.GetAsync(id);
        if (question == null)
            throw new NotFoundException(nameof(Question), id);

        CheckAccessForQuestion(question);

        question.Answer = request.Answer;
        if (request.NewStatus != null) question.Status = request.NewStatus.Value;

        await _questionRepository.UpdateAsync(question.Id, question);

        return Ok(new QuestionDto(question));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [Route("newGlobalQuestion")]
    public async Task<ActionResult<GlobalQuestionDto>> CreateNewGlobalQuestion(
        [FromBody] CreateGlobalQuestionRequestDto request)
    {
        var globalQuestion = new GlobalQuestion
        {
            Id = Guid.NewGuid(), Title = request.Title,
            CategoryId = request.CategoryId ?? Guid.Parse("ea815826-0c02-e446-a984-00f62a687381")
        };
        await _globalQuestionRepository.CreateAsync(globalQuestion);

        return Ok(new GlobalQuestionDto(globalQuestion));
    }

    [HttpPost]
    [Authorize]
    [Route("new")]
    public async Task<ActionResult<QuestionDto>> CreateNewQuestion([FromBody] AddQuestionRequestDto request)
    {
        CheckAccessForUser(request.UserId);

        var question = new Question
        {
            Id = Guid.NewGuid(), Title = request.Title, CategoryId = Guid.Parse("ea815826-0c02-e446-a984-00f62a687381"),
            GlobalQuestionId = Guid.Empty,
            Status = Status.Unanswered, UserId = request.UserId
        };

        await _questionRepository.CreateAsync(question);

        return Ok(new QuestionDto(question));
    }

    [HttpPost]
    [Authorize]
    [Route("book")]
    public async Task<IActionResult> SendRequestToCreateBook([FromQuery] Guid userId)
    {
        CheckAccessForUser(userId);

        var addressSettings = await _userSettingsRepository.GetAddressSettings(userId);
        if (addressSettings == null)
            throw new NotFoundException(nameof(AddressSettings), userId);

        var user = await _userRepository.GetAsync(userId);

        var questions = (await _questionRepository.GetForUserAsync(userId))
            .Where(q => q.Status == Status.Answered)
            .OrderBy(q => q.CategoryId)
            .Select(q => $"<p><h1>{q.Title}</h1>{q.Answer}</p>")
            .ToList();
        if (questions.Count < 1)
            return BadRequest("answers < 1");

        var resultString = string.Join(' ', questions);
        var pdf = PdfGenerator.GeneratePdf(resultString, PageSize.A4);
        using var stream = new MemoryStream();
        pdf.Save(stream);
        var message = $"<p>Адрес пользователя {user.Email}:</p><p>{addressSettings.ToHtml()}</p>";
        await _emailClient.SendMessageWithAttachments(
            _emailClient.EmailSettings.Email ?? "rememory.notifications@yandex.ru", message, stream, "answers.pdf");
        return Ok();
    }

    private void CheckAccessForUser(Guid userId)
    {
        var accessToken = _authProvider.ParseAuthHeader(HttpContext.Request.Headers.Authorization);
        var userCred = _authProvider.GetCurrentUserCredential(accessToken);
        if (userCred.Id != userId) throw new ForbiddenException();
    }

    private void CheckAccessForQuestion(Question question)
    {
        var accessToken = _authProvider.ParseAuthHeader(HttpContext.Request.Headers.Authorization);
        var userCred = _authProvider.GetCurrentUserCredential(accessToken);
        if (question.UserId != userCred.Id) throw new ForbiddenException();
    }
}