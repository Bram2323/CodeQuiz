using Godot.Collections;
using System;
using System.Collections.Generic;

public record Answer(AnswerType Type, AnswerOption[] Answers, bool CaseSensitive)
{
    public static Answer GetFromDictionary(Dictionary data)
    {
        if (!data.ContainsKey("type")) throw new ArgumentException("Answer doesnt have a type!");
        if (!Enum.TryParse(data["type"].ToString(), true, out AnswerType type)) throw new ArgumentException("Answer has invalid type!");


        List<AnswerOption> answerOptions = new();
        if (data.ContainsKey("answers"))
        {
            Array<Dictionary> answerOptionsData = data["answers"].AsGodotArray<Dictionary>();
            foreach (Dictionary answerOptionData in answerOptionsData)
            {
                answerOptions.Add(AnswerOption.GetFromDictionary(answerOptionData));
            }
        }


        bool caseSensitive = false;
        if (data.ContainsKey("caseSensitive")) caseSensitive = data["caseSensitive"].AsBool();


        return new(type, answerOptions.ToArray(), caseSensitive);
    }
}