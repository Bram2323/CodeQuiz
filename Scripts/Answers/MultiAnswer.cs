using Godot;
using System;
using System.Collections.Generic;

public partial class MultiAnswer : Node, IAnswerNode
{
	[Export]
	Node ChoiceParent;
	[Export]
	PackedScene ChoicePrefab;

	[Export]
	bool SingleChoice;


	List<(Choice, bool)> choices = new();
	int correctChoices = 0;



    public void SetAnswers(AnswerOption[] answers, bool caseSensitive)
	{
		ClearChoices();

		correctChoices = 0;

        foreach (AnswerOption answer in answers)
        {
            Choice choice = ChoicePrefab.Instantiate<Choice>();
            ChoiceParent.AddChild(choice);

            choice.SetText(answer.Text);
			choice.Toggled += OnButtonToggled;

			choices.Add((choice, answer.Correct));
			if (answer.Correct) correctChoices++;
        }
    }

	public void ShowAnswers()
	{
		foreach ((Choice, bool) choiceData in choices)
		{
			Choice choice = choiceData.Item1;
			bool correct = choiceData.Item2;

			choice.SetDisabled(true);
			choice.SetTextActive(correct);
		}
	}

	public void OnButtonToggled(Choice toggledChoice, bool pressed)
	{
		if (!SingleChoice || !pressed || correctChoices != 1) return;

		foreach ((Choice, bool) choiceData in choices)
		{
			Choice choice = choiceData.Item1;
			if (choice != toggledChoice) choice.SetPressed(false);
		}
	}


	public bool HasCorrectlyAnswered()
	{
		foreach ((Choice, bool) choiceData in choices)
		{
			Choice choice = choiceData.Item1;
			bool correct = choiceData.Item2;
			if (choice.IsPressed() != correct) return false;
		}

		return true;
	}


	void ClearChoices()
	{
		foreach ((Choice, bool) choiceData in choices)
		{
			Choice choice = choiceData.Item1;
			choice.Toggled -= OnButtonToggled;
			choice.QueueFree();
		}

		choices.Clear();
	}
}
