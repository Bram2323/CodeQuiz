using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.IO;

public partial class MainMenu : Node
{
	[Export]
	Button LoadFileButton, LoadDirectoryButton;
    [Export]
    FileDialog FileDialog;
    [Export]
    AcceptDialog AcceptDialog;

    [Export]
    PackedScene QuestionManager;


    public override void _Ready()
    {
        LoadFileButton.Pressed += LoadFile;
        LoadDirectoryButton.Pressed += LoadDirectory;
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


    private void LoadQuestion(Question question)
    {
        QuestionManager questionManager = QuestionManager.Instantiate<QuestionManager>();
        GetTree().Root.AddChild(questionManager);
        questionManager.LoadSingleQuestion(question);

        QueueFree();
    }

    private void LoadQuiz(Quiz quiz)
    {
        QuestionManager questionManager = QuestionManager.Instantiate<QuestionManager>();
        GetTree().Root.AddChild(questionManager);
        questionManager.LoadQuiz(quiz);

        QueueFree();
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
