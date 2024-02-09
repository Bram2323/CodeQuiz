using Godot.Collections;
using System;

public record Segment(SegmentType Type, string Text)
{
    public static Segment GetFromDictionary(Dictionary data)
    {
        if (!data.ContainsKey("type")) throw new ArgumentException("Segement doesnt have a type!");
        if (!Enum.TryParse(data["type"].ToString(), true, out SegmentType type)) throw new ArgumentException("Segement has invalid type!");

        string text = "";
        if (data.ContainsKey("text")) text = data["text"].ToString();

        return new(type, text);
    }
}