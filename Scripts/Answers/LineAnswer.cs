using Godot;
using System.Collections.Generic;

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


	public bool HasCorrectlyAnswered()
	{
		bool correct = true;

        foreach (InputField input in inputs)
        {
			if (!input.HasCorrectlyAnswered()) correct = false;
        }

        return correct;
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
