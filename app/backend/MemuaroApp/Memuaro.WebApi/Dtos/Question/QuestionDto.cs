﻿namespace Memuaro.WebApi.Dtos.Question;

public class QuestionDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public Guid UserId { get; set; }
    public string? Answer { get; set; }
    public string? Status { get; set; }

    public QuestionDto(Persistance.Entities.Question question)
    {
        Id = question.Id;
        Title = question.Title;
        UserId = question.UserId;
        Answer = question.Answer;
        Status = question.Status.ToString();
    }

    public QuestionDto()
    {
    }
}