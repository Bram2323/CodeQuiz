using Godot;
using System;
using System.Collections.Generic;

public partial class ListAnswer : Node, IAnswerNode
{
	[Export]
	LineEdit AnswerField;
	[Export]
	GridContainer ButtonParent;
	[Export]
	PackedScene ButtonPrefab;

	List<(Button, string)> buttons = new();
	bool caseSensitive = false;
	bool answeredAll = false;

	bool answered = false;


    public override void _Ready()
    {
		AnswerField.TextSubmitted += OnUserInput;
    }


    public void SetAnswers(AnswerOption[] answers, bool caseSensitive)
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

            buttons.Add((button, answer.Text));
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

	public void ShowAnswers()
	{
		AnswerField.Editable = false;

		foreach ((Button, string) buttonData in buttons)
		{
			Button button = buttonData.Item1;
			string answer = buttonData.Item2;

			if (!button.Disabled)
            {
                button.ButtonPressed = true;
				button.Text = answer;
            }
		}
	}


	void OnUserInput(string text)
	{
		text = text.Trim();
		if (!caseSensitive) text = text.ToLower();

		answered = true;

		int guessedAnswers = 0;

		foreach ((Button, string) buttonData in buttons)
		{
			Button button = buttonData.Item1;
			string buttonText = buttonData.Item2;
			string answer = buttonText;
			if (!caseSensitive) answer = answer.ToLower();

			if (answer == text)
			{
				button.Disabled = true;
				button.Text = buttonText;
			}

			if (button.Disabled) guessedAnswers++;
		}

		if (guessedAnswers == buttons.Count) answeredAll = true;

		AnswerField.Clear();
	}


	public bool HasAnswered()
	{
		return answered;
	}

	public bool HasCorrectlyAnswered()
	{
		return answeredAll;
	}

	public object GetUserAnswers()
	{
        bool[] answers = new bool[buttons.Count + 1];

		answers[^1] = answered;

        for (int i = 0; i < buttons.Count; i++)
        {
            answers[i] = buttons[i].Item1.Disabled;
        }

        return answers;
    }

    public void SetUserAnswers(object data)
    {
        if (data is not bool[] answers) return;
        if (answers.Length != buttons.Count + 1) return;

        int guessedAnswers = 0;
		answered = answers[^1];

        for (int i = 0; i < buttons.Count; i++)
		{
			(Button, string) buttonData = buttons[i];
			Button button = buttonData.Item1;
			string buttonText = buttonData.Item2;

			if (answers[i])
			{
				button.Disabled = true;
				button.Text = buttonText;
				guessedAnswers++;
			}
		}

        if (guessedAnswers == buttons.Count) answeredAll = true;
    }


	void ClearButtons()
	{
		foreach ((Button, string) buttonData in buttons) buttonData.Item1.QueueFree();
		buttons.Clear();
		answeredAll = false;
	}
}
