using Godot.Collections;
using System;
using System.Collections.Generic;

public record Answer(AnswerType Type, AnswerOption[] Answers, bool CaseSensitive)
{
    public static Answer Deserialize(Dictionary data)
    {
        string typeStr = "invalid";
        if (data.ContainsKey("type")) typeStr = data["type"].ToString();
        if (!Enum.TryParse(typeStr, true, out AnswerType type)) type = AnswerType.Invalid;

        bool caseSensitive = false;
        if (data.ContainsKey("caseSensitive")) caseSensitive = data["caseSensitive"].AsBool();

        List<AnswerOption> answerOptions = new();
        if (data.ContainsKey("answers"))
        {
            Array<Dictionary> answerOptionsData = data["answers"].AsGodotArray<Dictionary>();
            foreach (Dictionary answerOptionData in answerOptionsData)
            {
                answerOptions.Add(AnswerOption.Deserialize(answerOptionData));
            }
        }

        return new(type, answerOptions.ToArray(), caseSensitive);
    }
}