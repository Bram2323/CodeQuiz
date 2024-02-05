using Godot.Collections;
using System.Collections.Generic;

public record Question(string Title, Segment[] Segments, Answer Answer)
{
    public static Question Deserialize(Dictionary data)
    {
        string title = "";
        if (data.ContainsKey("title")) title = data["title"].ToString();

        List<Segment> segments = new();
        if (data.ContainsKey("segments"))
        {
            Array<Dictionary> segmentsData = data["segments"].AsGodotArray<Dictionary>();
            foreach (Dictionary segmentData in segmentsData)
            {
                segments.Add(Segment.Deserialize(segmentData));
            }
        }

        Answer answer = new(AnswerType.Invalid, System.Array.Empty<AnswerOption>(), false);
        if (data.ContainsKey("answer"))
        {
            Dictionary answerData = data["answer"].AsGodotDictionary();
            answer = Answer.Deserialize(answerData);
        }

        return new Question(title, segments.ToArray(), answer);
    }
}
