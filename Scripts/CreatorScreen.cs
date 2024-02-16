using Godot;
using System;

public partial class CreatorScreen : Control
{
    [Export]
    TextEdit SegmentsEdit;

    string lastSave = "";


    public string Text
    {
        get
        {
            return SegmentsEdit.Text;
        }
        set
        {
            SegmentsEdit.Text = value;
        }
    }



    public void Saved()
    {
        lastSave = SegmentsEdit.Text;
    }

    public bool HasUnsavedChanges()
    {
        return lastSave != SegmentsEdit.Text;
    }

    public void AddOnTextChangeCallback(Action callback)
    {
        SegmentsEdit.TextChanged += callback;
    }


    public Question CreateQuestion()
    {
        string text = SegmentsEdit.Text;
        string[] lines = text.Split('\n');

        QuestionBuilder builder = new QuestionBuilder();
        builder.AddLines(lines);

        return builder.ToQuestion();
    }
}
