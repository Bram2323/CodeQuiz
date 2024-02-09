using Godot.Collections;
using System;

public record AnswerOption(string Text, bool Correct)
{
    public static AnswerOption GetFromDictionary(Dictionary data)
    {
        if (!data.ContainsKey("text")) throw new ArgumentException("Answer option didnt have any text!");
        string text = data["text"].AsString();

        bool correct = false;
        if (data.ContainsKey("correct")) correct = data["correct"].AsBool();

        return new(text, correct);
    }
}