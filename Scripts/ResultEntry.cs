using Godot;
using System;

public partial class ResultEntry : Button
{
    [Export]
    string PrefixNumber, PostfixStatus;

    [Export]
    Label Status;

    public void SetNumber(int num)
    {
        Text = PrefixNumber + num;
    }

    public void SetStatus(QuestionStatus status, bool showAnswers)
    {
        string theme = GetColorTheme(status, showAnswers);

        ThemeTypeVariation = theme;
        Status.Text = theme + PostfixStatus;
    }

    public void SetButtonCallback(Action callback)
    {
        Pressed += callback;
    }


    string GetColorTheme(QuestionStatus status, bool showAnswers)
    {
        if (showAnswers)
        {
            if ((status & QuestionStatus.Correct) != 0) return "Correct";
            else return "Incorrect";
        }
        else
        {
            if ((status & QuestionStatus.Answered) != 0)
            {
                if ((status & QuestionStatus.Incomplete) != 0) return "Incomplete";
                else return "Answered";
            }
            else return "Unanswered";
        }
    }
}
