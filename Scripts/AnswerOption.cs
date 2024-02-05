using Godot.Collections;

public record AnswerOption(string Text, bool Correct)
{
    public static AnswerOption Deserialize(Dictionary data)
    {
        string text = "";
        if (data.ContainsKey("text")) text = data["text"].AsString();

        bool correct = false;
        if (data.ContainsKey("correct")) correct = data["correct"].AsBool();

        return new(text, correct);
    }
}