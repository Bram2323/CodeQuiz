using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.IO;

public record Question(string Title, Segment[] Segments, Answer Answer)
{
    public static Question GetFromDictionary(Dictionary data)
    {
        string title = "";
        if (data.ContainsKey("title")) title = data["title"].ToString();


        List<Segment> segments = new();
        if (data.ContainsKey("segments"))
        {
            Array<Dictionary> segmentsData = data["segments"].AsGodotArray<Dictionary>();
            foreach (Dictionary segmentData in segmentsData)
            {
                segments.Add(Segment.GetFromDictionary(segmentData));
            }
        }


        if (!data.ContainsKey("answer")) throw new ArgumentException("Answer not found!");

        Dictionary answerData = data["answer"].AsGodotDictionary();
        Answer answer = Answer.GetFromDictionary(answerData);


        return new Question(title, segments.ToArray(), answer);
    }


    public static Question GetFromJson(string json)
    {
        Variant? dataVariant = Json.ParseString(json);
        if (dataVariant is null) throw new ArgumentException("Failed to parse json!");
        Dictionary data = (Dictionary)dataVariant;

        return GetFromDictionary(data);
    }

    public static Question GetFromFile(string path)
    {
        if (!File.Exists(path)) throw new ArgumentException("File not found!");
        if (!path.EndsWith(".json")) throw new ArgumentException("Wrong file format!");

        string json = File.ReadAllText(path);
        return GetFromJson(json);
    }

    public static Question[] GetAllInDirectory(string path)
    {
        if (!Directory.Exists(path)) return System.Array.Empty<Question>();

        List<Question> questions = new();
        foreach (string filePath in Directory.GetFiles(path, "*.json"))
        {
            try
            {
                Question question = GetFromFile(filePath);
                questions.Add(question);
            }
            catch (ArgumentException) { }
        }

        return questions.ToArray();
    }
}
