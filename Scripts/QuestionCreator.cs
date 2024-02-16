using Godot;
using System;
using System.IO;

public partial class QuestionCreator : Node
{
    [Export]
    Label FileLabel, UnsavedLabel;

    [Export]
    Button MainMenuButton;
    [Export]
    Button PreviewButton, ShowAnswerButton;
    [Export]
    Button SaveButton, SaveAsButton, LoadButton, NewButton;

    [Export]
    ConfirmationDialog UnsavedConfirmation;

    [Export]
    FileDialog SaveDialog, LoadDialog;

    [Export]
    CreatorScreen CreatorScreen;
    [Export]
    QuestionScreen QuestionScreen;


    Action unsavedAction;

    string currentFilePath = null;
    bool inPreview = false;


    public override void _Ready()
    {
        GetTree().AutoAcceptQuit = false;

        MainMenuButton.Pressed += AskLoadMainMenu;
        UnsavedConfirmation.Confirmed += UnsavedConfirmed;
        PreviewButton.Pressed += TogglePreview;
        ShowAnswerButton.Pressed += ShowAnswer;
        SaveButton.Pressed += Save;
        SaveAsButton.Pressed += SaveAs;
        LoadButton.Pressed += Load;
        NewButton.Pressed += AskNew;

        SaveDialog.FileSelected += SaveFile;
        LoadDialog.FileSelected += LoadFile;

        CreatorScreen.AddOnTextChangeCallback(OnTextChange);

        SetPreview(false);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            if (keyEvent.CtrlPressed && keyEvent.Keycode == Key.S) Save();
        }
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            AskUserIfNotSaved(delegate { GetTree().Quit(); }, "Quit without saving?");
        }
    }

    void OnTextChange()
    {
        UnsavedLabel.Visible = CreatorScreen.HasUnsavedChanges();
    }


    void Save()
    {
        if (string.IsNullOrEmpty(currentFilePath)) SaveAs();
        else SaveFile(currentFilePath);
    }

    void SaveAs()
    {
        SaveDialog.PopupCentered();
    }

    void Load()
    {
        AskUserIfNotSaved(delegate { LoadDialog.PopupCentered(); }, "Load file?");
    }

    void AskNew()
    {
        AskUserIfNotSaved(New, "Create new question?");
    }

    void New()
    {
        CreatorScreen.Text = "";
        ResetFilePath();
    }


    void SaveFile(string path)
    {
        File.WriteAllText(path, CreatorScreen.Text);

        UpdateFilePath(path);
    }

    void LoadFile(string path)
    {
        if (!File.Exists(path)) return;

        CreatorScreen.Text = File.ReadAllText(path);

        UpdateFilePath(path);
    }

    void UpdateFilePath(string path)
    {
        CreatorScreen.Saved();
        OnTextChange();
        FileLabel.Text = Path.GetFileNameWithoutExtension(path);
        currentFilePath = path;
    }

    void ResetFilePath()
    {
        CreatorScreen.Saved();
        OnTextChange();
        FileLabel.Text = "Untitled";
        currentFilePath = null;
    }



    void TogglePreview()
    {
        SetPreview(!inPreview);
    }

    void SetPreview(bool enabled)
    {
        inPreview = enabled;

        CreatorScreen.Visible = !enabled;
        QuestionScreen.Visible = enabled;

        PreviewButton.Text = enabled ? "Editor" : "Preview";
        ShowAnswerButton.Visible = enabled;

        if (enabled)
        {
            Question question = CreatorScreen.CreateQuestion();
            QuestionScreen.LoadQuestion(question);
        }
    }

    void ShowAnswer()
    {
        QuestionScreen.ShowAnswers();
    }



    void AskUserIfNotSaved(Action action, string question)
    {
        unsavedAction = action;

        UnsavedConfirmation.DialogText = "You have unsaved changes!\n" + question;
        if (CreatorScreen.HasUnsavedChanges()) UnsavedConfirmation.PopupCentered();
        else UnsavedConfirmed();
    }

    void UnsavedConfirmed()
    {
        unsavedAction?.Invoke();
        unsavedAction = null;
    }



    void AskLoadMainMenu()
    {
        AskUserIfNotSaved(LoadMainMenu, "Quit to main menu?");
    }

    void LoadMainMenu()
    {
        Node mainScene = GD.Load<PackedScene>("res://Recources/Scenes/MainMenu.tscn").Instantiate();
        GetTree().Root.AddChild(mainScene);

        GetTree().AutoAcceptQuit = true;

        QueueFree();
    }
}
