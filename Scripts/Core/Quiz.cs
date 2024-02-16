using System.Collections.Generic;
using System.IO;

public record Quiz(string Title, Question[] Questions, Quiz[] Quizzes)
{
    public static Quiz GetFromDirectory(string path)
    {
        if (!Directory.Exists(path)) return null;
        string title = Path.GetDirectoryName(path);

        Question[] questions = Question.GetAllInDirectory(path);

        List<Quiz> quizzes = new();
        foreach (string directoryPath in Directory.GetDirectories(path))
        {
            Quiz quiz = GetFromDirectory(directoryPath);
            quizzes.Add(quiz);
        }

        return new(title, questions, quizzes.ToArray());
    }

    public Question[] GetAllQuestions()
    {
        List<Question> questions = new();
        GetAllQuestions(questions);
        return questions.ToArray();
    }

    private void GetAllQuestions(List<Question> questions)
    {
        questions.AddRange(Questions);

        foreach (Quiz quiz in Quizzes)
        {
            quiz.GetAllQuestions(questions);
        }
    }
}