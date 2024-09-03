using Godot;
using System.Collections.Generic;

public partial class LineAnswer : AnswerNode
{
    [Export]
    Node InputParent;
    [Export]
    PackedScene InputPrefab;


    List<InputField> inputs = new();



    public override void SetAnswers(AnswerOption[] answers)
    {
        ClearInputs();

        foreach (AnswerOption answer in answers)
        {
            InputField input = InputPrefab.Instantiate<InputField>();
            InputParent.AddChild(input);

            input.SetAnswer(answer.Text.Trim(), answer.CaseSensitive);

            inputs.Add(input);
        }
    }

    public override void ShowAnswers()
    {
        foreach (InputField input in inputs)
        {
            input.ShowAnswer();
        }
    }


    public override bool HasAnswered()
    {
        foreach (InputField input in inputs)
        {
            if (!string.IsNullOrWhiteSpace(input.GetText().Trim())) return true;
        }
        return false;
    }

    public override bool IsIncomplete()
    {
        foreach (InputField input in inputs)
        {
            if (string.IsNullOrWhiteSpace(input.GetText().Trim())) return true;
        }
        return false;
    }

    public override bool HasCorrectlyAnswered()
    {
        bool correct = true;

        foreach (InputField input in inputs)
        {
            if (!input.HasCorrectlyAnswered()) correct = false;
        }

        return correct;
    }


    public override object GetUserAnswers()
    {
        string[] answers = new string[inputs.Count];

        for (int i = 0; i < inputs.Count; i++)
        {
            answers[i] = inputs[i].GetText();
        }

        return answers;
    }

    public override void SetUserAnswers(object data)
    {
        if (data is not string[] answers) return;
        if (answers.Length != inputs.Count) return;

        for (int i = 0; i < inputs.Count; i++)
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
