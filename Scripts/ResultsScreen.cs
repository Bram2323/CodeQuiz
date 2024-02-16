using Godot;
using System;
using System.Collections.Generic;

public partial class ResultsScreen : Control
{
    [Export]
    PackedScene EntryPrefab;

    [Export]
    Label Amount, Total, NumberText;
    [Export]
    Button ShowAnswersButton;

    List<ResultEntry> resultsEntries = new();
    Action ShowAnswersCallback;


    public override void _Ready()
    {
        ShowAnswersButton.Pressed += ShowAnswers;
    }

    public void LoadQuestions(QuestionStatus[] questionsStatus, bool showAnswers, QuizManager questionManager)
    {
        ClearEntries();

        int correct = 0;
        int answered = 0;

        ShowAnswersCallback = questionManager.AskToShowResults;

        for (int i = 0; i < questionsStatus.Length; i++)
        {
            QuestionStatus status = questionsStatus[i];

            if ((status & QuestionStatus.Correct) == QuestionStatus.Correct) correct++;
            if ((status & QuestionStatus.Answered) != QuestionStatus.Answered) answered++;

            ResultEntry entry = EntryPrefab.Instantiate<ResultEntry>();

            AddChild(entry);

            entry.SetNumber(i + 1);
            entry.SetStatus(status, showAnswers);
            int question = i;
            entry.SetButtonCallback(delegate { questionManager.LoadQuestion(question); });

            resultsEntries.Add(entry);
        }


        MoveChild(ShowAnswersButton, -1);

        if (showAnswers)
        {
            NumberText.Text = "Correct";
            Amount.Text = correct.ToString();
        }
        else
        {
            NumberText.Text = "Answered";
            Amount.Text = answered.ToString();
        }
        Total.Text = questionsStatus.Length.ToString();
    }

    void ShowAnswers()
    {
        ShowAnswersCallback?.Invoke();
    }

    void ClearEntries()
    {
        foreach (ResultEntry entry in resultsEntries)
        {
            entry.QueueFree();
        }

        resultsEntries.Clear();
    }
}
