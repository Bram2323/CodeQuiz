using Godot.Collections;
using System;

public record Segment(SegmentType Type, string Text)
{
    public static Segment Deserialize(Dictionary data)
    {
        string typeStr = "invalid";
        if (data.ContainsKey("type")) typeStr = data["type"].ToString();
        if (!Enum.TryParse(typeStr, true, out SegmentType type)) type = SegmentType.Invalid;

        string text = "";
        if (data.ContainsKey("text")) text = data["text"].ToString();

        return new(type, text);
    }
}