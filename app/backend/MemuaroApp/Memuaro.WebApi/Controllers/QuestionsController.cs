using Memuaro.Persistance.Client;
using Memuaro.Persistance.Entities;
using Memuaro.Persistance.Repositories.GlobalQuestionRepository;
using Memuaro.Persistance.Repositories.QuestionRepository;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Memuaro.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IDatabaseClient _databaseClient;
    private readonly IGlobalQuestionRepository _globalQuestionRepository;
    private readonly IQuestionRepository _questionRepository;

    public QuestionsController(
        IDatabaseClient databaseClient,
        IGlobalQuestionRepository globalQuestionRepository,
        IQuestionRepository questionRepository)
    {
        _databaseClient = databaseClient;
        _globalQuestionRepository = globalQuestionRepository;
        _questionRepository = questionRepository;
    }

    [HttpGet]
    [Route("new")]
    public async Task<Question> GetNewQuestion([FromQuery] Guid userId)
    {
        var newGlobalQuestion = await _globalQuestionRepository.GetRandomGlobalQuestionForUser(userId);
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

        return result;
    }

    [HttpGet]
    [Route("")]
    public async Task<List<Question>> GetAllQuestionsForUser([FromQuery] Guid userId)
    {
        var allQuestions = await _questionRepository.GetForUserAsync(userId);
        return allQuestions;
    }

    [HttpPatch]
    [Route("{id}")]
    public async Task<Question> PatchAnswer(string id, [FromBody] string answer)
    {
        var question = await _questionRepository.GetAsync(Guid.Parse(id));
        question.Answer = answer;

        await _questionRepository.UpdateAsync(question.Id, question);

        return question;
    }
}