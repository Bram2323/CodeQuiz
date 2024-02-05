using Godot.Collections;
using System;
using System.Collections.Generic;

public record Answer(AnswerType Type, AnswerOption[] Answers, bool CaseSensitive)
{
    public static Answer Deserialize(Dictionary data)
    {
        string typeStr = data["type"].ToString();
        if (!Enum.TryParse(typeStr, true, out AnswerType type)) type = AnswerType.Invalid;

        bool caseSensitive = false;
        if (data.ContainsKey("caseSensitive")) caseSensitive = data["caseSensitive"].AsBool();

        Array<Dictionary> answerOptionsData = data["answers"].AsGodotArray<Dictionary>();
        List<AnswerOption> answerOptions = new();
        foreach (Dictionary answerOptionData in answerOptionsData)
        {
            answerOptions.Add(AnswerOption.Deserialize(answerOptionData));
        }

        return new(type, answerOptions.ToArray(), caseSensitive);
    }
}