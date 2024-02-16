using Godot;
using System.Collections.Generic;

public partial class ListAnswer : AnswerNode
{
    [Export]
    LineEdit AnswerField;
    [Export]
    GridContainer ButtonParent;
    [Export]
    PackedScene ButtonPrefab;

    List<(Button, AnswerOption)> buttons = new();

    bool answered = false;


    public override void _Ready()
    {
        AnswerField.TextSubmitted += OnUserInput;
    }


    public override void SetAnswers(AnswerOption[] answers)
    {
        ClearButtons();
        answered = false;

        foreach (AnswerOption answer in answers)
        {
            Button button = ButtonPrefab.Instantiate<Button>();
            ButtonParent.AddChild(button);

            button.MouseFilter = Control.MouseFilterEnum.Ignore;
            button.FocusMode = Control.FocusModeEnum.None;
            button.ToggleMode = true;
            button.Text = "?";
            if (!answer.Correct)
            {
                button.Text = "!";
                button.ThemeTypeVariation = "Incorrect";
            }

            buttons.Add((button, answer));
        }

        if (answers.Length < 2) ButtonParent.Columns = 1;
        else
        {
            for (int i = 5; i >= 2; i--)
            {
                if (answers.Length % i == 0)
                {
                    ButtonParent.Columns = i;
                    break;
                }
            }
        }
    }

    public override void ShowAnswers()
    {
        AnswerField.Editable = false;

        foreach ((Button, AnswerOption) buttonData in buttons)
        {
            Button button = buttonData.Item1;
            AnswerOption answer = buttonData.Item2;

            if (!button.Disabled)
            {
                button.ButtonPressed = true;
                button.Text = answer.Text.Trim();
            }
        }
    }


    void OnUserInput(string text)
    {
        text = text.Trim();

        answered = true;

        foreach ((Button, AnswerOption) buttonData in buttons)
        {
            Button button = buttonData.Item1;
            AnswerOption answer = buttonData.Item2;
            string answerText = answer.Text.Trim();

            if (text == answerText || (!answer.CaseSensitive && text.ToLower() == answerText.ToLower()))
            {
                button.Disabled = true;
                button.Text = answerText;
            }
        }

        AnswerField.Clear();
    }


    public override bool HasAnswered()
    {
        return answered;
    }

    public override bool HasCorrectlyAnswered()
    {
        foreach ((Button, AnswerOption) buttonData in buttons)
        {
            Button button = buttonData.Item1;
            AnswerOption answer = buttonData.Item2;

            if (button.Disabled ^ answer.Correct) return false;
        }

        return true;
    }

    public override object GetUserAnswers()
    {
        bool[] answers = new bool[buttons.Count + 1];

        answers[^1] = answered;

        for (int i = 0; i < buttons.Count; i++)
        {
            answers[i] = buttons[i].Item1.Disabled;
        }

        return answers;
    }

    public override void SetUserAnswers(object data)
    {
        if (data is not bool[] answers) return;
        if (answers.Length != buttons.Count + 1) return;

        int guessedAnswers = 0;
        answered = answers[^1];

        for (int i = 0; i < buttons.Count; i++)
        {
            (Button, AnswerOption) buttonData = buttons[i];
            Button button = buttonData.Item1;
            AnswerOption answer = buttonData.Item2;

            if (answers[i])
            {
                button.Disabled = true;
                button.Text = answer.Text.Trim();
                guessedAnswers++;
            }
        }
    }


    void ClearButtons()
    {
        foreach ((Button, AnswerOption) buttonData in buttons) buttonData.Item1.QueueFree();
        buttons.Clear();
    }
}
