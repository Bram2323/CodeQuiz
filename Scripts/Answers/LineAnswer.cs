 using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

public partial class LineAnswer : Node, IAnswerNode
{
	[Export]
	Node InputParent;
	[Export]
	PackedScene InputPrefab;


	List<InputField> inputs = new();



    public void SetAnswers(AnswerOption[] answers, bool caseSensitive)
	{
		ClearInputs();

        foreach (AnswerOption answer in answers)
        {
            InputField input = InputPrefab.Instantiate<InputField>();
            InputParent.AddChild(input);

            input.SetAnswer(answer.Text, caseSensitive);

			inputs.Add(input);
        }
    }

	public void ShowAnswers()
	{
        foreach (InputField input in inputs)
        {
            input.ShowAnswer();
        }
    }


    public bool HasAnswered()
    {
		foreach (InputField input in inputs)
		{
			if (!string.IsNullOrWhiteSpace(input.GetText().Trim())) return true;
		}
        return false;
    }

    public bool HasCorrectlyAnswered()
	{
		bool correct = true;

        foreach (InputField input in inputs)
        {
			if (!input.HasCorrectlyAnswered()) correct = false;
        }

        return correct;
	}


	public object GetUserAnswers()
	{
		string[] answers = new string[inputs.Count];

		for (int i = 0; i < inputs.Count; i++)
		{
			answers[i] = inputs[i].GetText();
		}

		return answers;
	}

	public void SetUserAnswers(object data)
	{
		if (data is not string[] answers) return;
		if (answers.Length != inputs.Count) return;

		for (int i = 0;i < inputs.Count;i++)
		{
			inputs[i].SetText(answers[i]);
		}
	}


	void ClearInputs()
	{
		foreach (InputField input in inputs)
		{
			input.QueueFree();
		}

		inputs.Clear();
	}
}
