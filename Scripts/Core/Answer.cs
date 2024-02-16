using System;

public record Answer(AnswerType Type, AnswerOption[] Answers) : IBlock
{
    public static Answer Empty => new(AnswerType.Empty, Array.Empty<AnswerOption>());
}