using Godot;
using Godot.Collections;
using System;
using System.IO;

public partial class MainMenu : Node
{
	[Export]
	Button LoadButton;
    [Export]
    FileDialog FileDialog;
    [Export]
    AcceptDialog AcceptDialog;

    [Export]
    PackedScene QuestionManager;


    public override void _Ready()
    {
        LoadButton.Pressed += Load;
        FileDialog.FileSelected += FileSelected;
    }


    void Load()
    {
        FileDialog.PopupCentered();
    }

    void LoadQuestion(Question question)
    {
        QuestionManager questionManager = QuestionManager.Instantiate<QuestionManager>();
        GetTree().Root.AddChild(questionManager);
        questionManager.LoadQuestion(question);

        QueueFree();
    }

    void FileSelected(string path)
    {
        if (!File.Exists(path))
        {
            PopupMessage("File not found!", "Something went wrong!");
            return;
        }
        else if (!path.EndsWith(".json"))
        {
            PopupMessage("Wrong file format!", "Something went wrong!");
            return;
        }

        try
        {
            string jsonStr = File.ReadAllText(path);
            Dictionary jsonData = Json.ParseString(jsonStr).AsGodotDictionary();

            Question question = Question.Deserialize(jsonData);

            LoadQuestion(question);
        }
        catch
        {
            PopupMessage("Invalid question json!", "Something went wrong!");
        }
    }

    void PopupMessage(string message, string title)
    {
        AcceptDialog.DialogText = message;
        AcceptDialog.Title = title;
        AcceptDialog.PopupCentered();
    }
}