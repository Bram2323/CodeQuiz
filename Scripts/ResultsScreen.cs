using Godot;
using System;
using System.Collections.Generic;

public partial class ResultsScreen : Control
{
    [Export]
    PackedScene EntryPrefab;

    [Export]
    AnsweredLabel[] answeredLabels;
    [Export]
    ResultsLabel[] resultsLabels;
    [Export]
    Button ShowAnswersButton;

    List<ResultEntry> resultsEntries = new();
    Action ShowAnswersCallback;


    public override void _Ready()
    {
        ShowAnswersButton.Pressed += ShowAnswers;

        foreach (AnsweredLabel label in answeredLabels)
        {
            label.Visible = false;
        }

        foreach (ResultsLabel label in resultsLabels)
        {
            label.Visible = false;
        }
    }

    public void LoadQuestions(QuestionStatus[] questionsStatus, bool showAnswers, QuizManager questionManager)
    {
        ClearEntries();

        int total = questionsStatus.Length;
        int correct = 0;
        int answered = 0;
        int incomplete = 0;

        ShowAnswersCallback = questionManager.AskToShowResults;

        for (int i = 0; i < questionsStatus.Length; i++)
        {
            QuestionStatus status = questionsStatus[i];

            if ((status & QuestionStatus.Correct) != 0) correct++;
            if ((status & QuestionStatus.Answered) != 0) answered++;
            if ((status & QuestionStatus.Incomplete) != 0) incomplete++;

            ResultEntry entry = EntryPrefab.Instantiate<ResultEntry>();

            AddChild(entry);

            entry.SetNumber(i + 1);
            entry.SetStatus(status, showAnswers);
            int question = i;
            entry.SetButtonCallback(delegate { questionManager.LoadQuestion(question); });

            resultsEntries.Add(entry);
        }


        MoveChild(ShowAnswersButton, -1);

        foreach (AnsweredLabel label in answeredLabels)
        {
            label.SetAnswered(answered, total);
        }

        foreach (ResultsLabel label in resultsLabels)
        {
            label.SetCorrect(correct, total);
        }

        ShowLabels(showAnswers);
    }

    void ShowLabels(bool showResults)
    {
        foreach (AnsweredLabel label in answeredLabels)
        {
            label.Visible = !showResults;
        }

        foreach (ResultsLabel label in resultsLabels)
        {
            label.Visible = showResults;
        }

        ShowAnswersButton.Disabled = showResults;
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
