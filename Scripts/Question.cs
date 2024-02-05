
using Godot.Collections;
using System;
using System.Collections.Generic;

public record Question(string Title, Segment[] Segments, Answer Answer)
{
    public static Question Deserialize(Dictionary data)
    {
        string title = "";
        if (data.ContainsKey("title")) title = data["title"].ToString();

        Array<Dictionary> segmentsData = data["segments"].AsGodotArray<Dictionary>();
        List<Segment> segments = new();
        foreach (Dictionary segmentData in segmentsData)
        {
            segments.Add(Segment.Deserialize(segmentData));
        }

        Dictionary answerData = data["answer"].AsGodotDictionary();
        Answer answer = Answer.Deserialize(answerData);

        return new Question(title, segments.ToArray(), answer);
    }
}
