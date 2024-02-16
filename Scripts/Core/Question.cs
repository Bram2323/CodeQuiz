using System;
using System.Collections.Generic;
using System.IO;

public record Question(IBlock[] Blocks)
{
    public static Question Empty => new(Array.Empty<IBlock>());


    public static Question GetFromText(string text)
    {
        QuestionBuilder builder = new();
        builder.AddLines(text.Split('\n'));
        return builder.ToQuestion();
    }

    public static Question GetFromFile(string path)
    {
        if (!File.Exists(path)) throw new ArgumentException("File not found!");
        if (!path.EndsWith(".question")) throw new ArgumentException("Wrong file format!");

        string text = File.ReadAllText(path);
        return GetFromText(text);
    }

    public static Question[] GetAllInDirectory(string path)
    {
        if (!Directory.Exists(path)) return Array.Empty<Question>();

        List<Question> questions = new();
        foreach (string filePath in Directory.GetFiles(path))
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
