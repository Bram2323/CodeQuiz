using Godot;
using System;
using System.Collections.Generic;

public partial class ListAnswer : Node, IAnswerNode
{
	[Export]
	LineEdit AnswerField;
	[Export]
	Node ButtonParent;
	[Export]
	PackedScene ButtonPrefab;

	List<(Button, string)> buttons = new();
	bool caseSensitive = false;
	bool answeredAll = false;


    public override void _Ready()
    {
		AnswerField.TextSubmitted += OnUserInput;
    }


    public void SetAnswers(string[] answers, bool caseSensitive)
	{
		ClearButtons();

		foreach (string answer in answers)
		{
			Button button = ButtonPrefab.Instantiate<Button>();
			ButtonParent.AddChild(button);

			button.MouseFilter = Control.MouseFilterEnum.Ignore;
			button.ToggleMode = true;
			button.Text = "?";

            buttons.Add((button, answer));
        }
	}


	void OnUserInput(string text)
	{
		if (!caseSensitive) text = text.ToLower();

		int guessedAnswers = 0;

		foreach ((Button, string) buttonData in buttons)
		{
			Button button = buttonData.Item1;
			string buttonAnswer = buttonData.Item2;
			if (!caseSensitive) buttonAnswer = buttonAnswer.ToLower();

			if (buttonAnswer == text)
			{
				button.ButtonPressed = true;
				button.Text = buttonData.Item2;
			}

			if (button.ButtonPressed) guessedAnswers++;
		}

		if (guessedAnswers == buttons.Count) answeredAll = true;

		AnswerField.Clear();
	}


	public bool HasCorrectlyAnswered()
	{
		return answeredAll;
	}


	void ClearButtons()
	{
		foreach ((Button, string) buttonData in buttons) buttonData.Item1.QueueFree();
		buttons.Clear();
		answeredAll = false;
	}
}
