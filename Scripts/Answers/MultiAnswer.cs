using Godot;
using System.Collections.Generic;

public partial class MultiAnswer : AnswerNode
{
    [Export]
    Node ChoiceParent;
    [Export]
    PackedScene ChoicePrefab;

    [Export]
    bool SingleChoice;


    List<(Choice, bool)> choices = new();



    public override void SetAnswers(AnswerOption[] answers)
    {
        ClearChoices();

        foreach (AnswerOption answer in answers)
        {
            Choice choice = ChoicePrefab.Instantiate<Choice>();
            ChoiceParent.AddChild(choice);

            choice.SetText(answer.Text);
            choice.SetRound(SingleChoice);
            choice.Toggled += OnButtonToggled;

            choices.Add((choice, answer.Correct));
        }
    }

    public override void ShowAnswers()
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
        if (!SingleChoice || !pressed) return;

        foreach ((Choice, bool) choiceData in choices)
        {
            Choice choice = choiceData.Item1;
            if (choice != toggledChoice) choice.SetPressed(false);
        }
    }


    public override bool HasAnswered()
    {
        foreach ((Choice choice, bool _) in choices)
        {
            if (choice.IsPressed()) return true;
        }
        return false;
    }

    public override bool IsIncomplete()
    {
        return !HasAnswered();
    }

    public override bool HasCorrectlyAnswered()
    {
        int correctAnswers = 0;
        int selectedCorrectAnswers = 0;

        foreach ((Choice, bool) choiceData in choices)
        {
            Choice choice = choiceData.Item1;
            bool correct = choiceData.Item2;
            if (choice.IsPressed() && !correct) return false;

            if (correct) correctAnswers++;
            if (choice.IsPressed()) selectedCorrectAnswers++;
        }

        if (selectedCorrectAnswers == correctAnswers) return true;
        else return selectedCorrectAnswers > 0 && SingleChoice;
    }

    public override object GetUserAnswers()
    {
        bool[] answers = new bool[choices.Count];

        for (int i = 0; i < choices.Count; i++)
        {
            answers[i] = choices[i].Item1.IsPressed();
        }

        return answers;
    }

    public override void SetUserAnswers(object data)
    {
        if (data is not bool[] answers) return;
        if (answers.Length != choices.Count) return;

        for (int i = 0; i < choices.Count; i++)
        {
            Choice choice = choices[i].Item1;
            choice.SetPressed(answers[i]);
        }
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
