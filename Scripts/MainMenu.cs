using Godot;
using System;
using System.IO;

public partial class MainMenu : Node
{
    [Export]
    Button LoadFileButton, LoadDirectoryButton, CreateQuestionButton;
    [Export]
    FileDialog FileDialog;
    [Export]
    AcceptDialog AcceptDialog;

    [Export]
    PackedScene QuizManager;
    [Export]
    PackedScene QuestionCreator;


    public override void _Ready()
    {
        LoadFileButton.Pressed += LoadFile;
        LoadDirectoryButton.Pressed += LoadDirectory;
        CreateQuestionButton.Pressed += LoadQuestionCreator;
        FileDialog.FileSelected += FileSelected;
        FileDialog.DirSelected += DirectorySelected;
    }


    private void LoadFile()
    {
        FileDialog.FileMode = FileDialog.FileModeEnum.OpenFile;
        FileDialog.PopupCentered();
    }

    private void LoadDirectory()
    {
        FileDialog.FileMode = FileDialog.FileModeEnum.OpenDir;
        FileDialog.PopupCentered();
    }

    private void LoadQuestionCreator()
    {
        CreateQuestionCreator();
        QueueFree();
    }


    private void LoadQuestion(Question question)
    {
        CreateQuizManager().LoadSingleQuestion(question);

        QueueFree();
    }

    private void LoadQuiz(Quiz quiz)
    {
        CreateQuizManager().LoadQuiz(quiz);

        QueueFree();
    }


    private QuizManager CreateQuizManager()
    {
        QuizManager quizManager = QuizManager.Instantiate<QuizManager>();
        GetTree().Root.AddChild(quizManager);

        return quizManager;
    }

    private QuestionCreator CreateQuestionCreator()
    {
        QuestionCreator questionCreator = QuestionCreator.Instantiate<QuestionCreator>();
        GetTree().Root.AddChild(questionCreator);

        return questionCreator;
    }


    private void FileSelected(string path)
    {
        Question question;
        try
        {
            question = Question.GetFromFile(path);
        }
        catch (Exception ex)
        {
            PopupMessage("Error while loading question!\n" + ex.Message, "Something went wrong!");
            GD.PrintErr(ex);
            return;
        }

        LoadQuestion(question);
    }

    private void DirectorySelected(string path)
    {
        if (!Directory.Exists(path))
        {
            PopupMessage("Directory not found!", "Something went wrong!");
            return;
        }

        Quiz quiz = Quiz.GetFromDirectory(path);
        LoadQuiz(quiz);
    }



    void PopupMessage(string message, string title)
    {
        AcceptDialog.DialogText = message;
        AcceptDialog.Title = title;
        AcceptDialog.PopupCentered();
    }
}
