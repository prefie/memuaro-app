using Memuaro.Auth;
using Memuaro.Persistance.Client;
using Memuaro.Persistance.Entities;
using Memuaro.Persistance.Repositories.GlobalQuestionRepository;
using Memuaro.Persistance.Repositories.QuestionRepository;
using Memuaro.WebApi.Dtos.Question;
using Memuaro.WebApi.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Memuaro.WebApi.Controllers;

[Authorize]
public class QuestionsController : BaseController
{
    private readonly IDatabaseClient _databaseClient;
    private readonly IGlobalQuestionRepository _globalQuestionRepository;
    private readonly IQuestionRepository _questionRepository;

    public QuestionsController(
        IDatabaseClient databaseClient,
        IGlobalQuestionRepository globalQuestionRepository,
        IQuestionRepository questionRepository,
        AuthProvider authProvider) : base(authProvider)
    {
        _databaseClient = databaseClient;
        _globalQuestionRepository = globalQuestionRepository;
        _questionRepository = questionRepository;
    }

    [HttpGet]
    [Route("new")]
    public async Task<ActionResult<Question>> GetNewQuestion([FromQuery] Guid userId)
    {
        CheckAccessForUser(userId);
        
        var newGlobalQuestion = await _globalQuestionRepository.GetRandomGlobalQuestionForUser(userId);
        newGlobalQuestion.UserIds ??= new List<Guid>();

        newGlobalQuestion.UserIds.Add(userId);
        var userQuestion = new Question {Id = Guid.NewGuid(), Title = newGlobalQuestion.Title, UserId = userId};

        using var session = _databaseClient.GetSession();
        var transactionOptions = new TransactionOptions(
            readPreference: ReadPreference.Primary,
            readConcern: ReadConcern.Local,
            writeConcern: WriteConcern.WMajority);

        var cancellationToken = CancellationToken.None;
        var result = session.WithTransaction(
            (s, ct) =>
            {
                _globalQuestionRepository.UpdateAsync(newGlobalQuestion.Id, newGlobalQuestion);
                _questionRepository.CreateAsync(userQuestion);
                return userQuestion;
            },
            transactionOptions,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("")]
    public async Task<ActionResult<QuestionsDto>> GetAllQuestionsForUser([FromQuery] Guid userId)
    {
        CheckAccessForUser(userId);
        
        var allQuestions = await _questionRepository.GetForUserAsync(userId);
        return Ok(new QuestionsDto{Questions = allQuestions});
    }

    [HttpPatch]
    [Route("{id}")]
    public async Task<ActionResult<Question>> PatchAnswer(string id, [FromBody] AnswerRequestDto request)
    {
        var question = await _questionRepository.GetAsync(Guid.Parse(id));
        
        CheckAccessForQuestion(question);
        
        question.Answer = request.Answer;

        await _questionRepository.UpdateAsync(question.Id, question);

        return Ok(question);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [Route("newGlobalQuestion")]
    public async Task<IActionResult> CreateNewGlobalQuestion([FromBody] GlobalQuestionRequestDto request)
    {
        var globalQuestion = new GlobalQuestion {Id = Guid.NewGuid(), Title = request.Title};
        await _globalQuestionRepository.CreateAsync(globalQuestion);

        return NoContent();
    }

    private void CheckAccessForUser(Guid userId)
    {
        var accessToken = HttpContext.Request.Headers.Authorization.ToArray()[0].Split()[1]; // Временно
        var userCred = _authProvider.GetCurrentUserCredential(accessToken);
        if (userCred.Id != userId) throw new ForbiddenException();
    }

    private void CheckAccessForQuestion(Question question)
    {
        var accessToken = HttpContext.Request.Headers.Authorization.ToArray()[0].Split()[1]; // Временно
        var userCred = _authProvider.GetCurrentUserCredential(accessToken);
        if (question.UserId != userCred.Id) throw new ForbiddenException();
    }
}