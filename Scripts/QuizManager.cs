using Godot;
using System;
using System.Collections.Generic;

public partial class QuizManager : Node
{
    [Export]
    QuestionScreen QuestionScreen;
    [Export]
    ResultsScreen ResultsScreen;

    [Export]
    ConfirmationDialog QuitConfirmation;
    [Export]
    ConfirmationDialog ResultsConfirmation;

    [Export]
    LineEdit IndexEdit;
    [Export]
    Label TotalQuestions;
    [Export]
    Button MainMenuButton, ResultsButton;
    [Export]
    Button NextButton, PreviousButton;

    Question currentQuestion;

    bool quizMode = false;
    bool showAnswers = false;
    Question[] questions = Array.Empty<Question>();
    int currentIndex = 0;

    Dictionary<Question, object[]> answers = new();
    Dictionary<Question, QuestionStatus> questionStatuses = new();


    public override void _Ready()
    {
        MainMenuButton.Pressed += AskToQuit;
        QuitConfirmation.Confirmed += LoadMainMenu;
        ResultsButton.Pressed += LoadResultsScreen;
        ResultsConfirmation.Confirmed += ShowResults;
        IndexEdit.TextSubmitted += OnIndexEdit;
        IndexEdit.FocusExited += OnIndexFocusLost;
        NextButton.Pressed += Next;
        PreviousButton.Pressed += Previous;
    }


    public void LoadQuiz(Quiz quiz)
    {
        quizMode = true;
        showAnswers = false;
        questions = quiz.GetAllQuestions();
        TotalQuestions.Text = questions.Length + "";
        IndexEdit.Editable = true;
        answers = new();
        questionStatuses = new();

        SetResultsScreenVisible(false);

        foreach (Question question in questions)
        {
            questionStatuses[question] = QuestionStatus.Incomplete;
        }

        LoadQuestion(0);
    }

    public void LoadSingleQuestion(Question question)
    {
        quizMode = false;
        showAnswers = false;
        questions = new Question[] { question };
        TotalQuestions.Text = "1";
        IndexEdit.Editable = false;
        answers = new();
        questionStatuses = new();

        SetResultsScreenVisible(false);

        questionStatuses[question] = 0;

        LoadQuestion(0);
    }


    public void LoadQuestion(int index)
    {
        if (index < 0 || index >= questions.Length) return;

        SetResultsScreenVisible(false);

        currentIndex = index;
        IndexEdit.Text = (index + 1).ToString();

        LoadQuestion(questions[index]);
    }

    void LoadQuestion(Question question)
    {
        SaveCurrentQuestionData();

        QuestionScreen.LoadQuestion(question);
        if (answers.ContainsKey(question)) QuestionScreen.SetUserAnswers(answers[question]);
        if (showAnswers) QuestionScreen.ShowAnswers();

        currentQuestion = question;

        UpdateButtons();
    }

    void SaveCurrentQuestionData()
    {
        if (currentQuestion == null || showAnswers) return;

        answers[currentQuestion] = QuestionScreen.GetUserAnswers();

        QuestionStatus status = 0;
        if (QuestionScreen.HasAnswered()) status |= QuestionStatus.Answered;
        if (QuestionScreen.IsIncomplete()) status |= QuestionStatus.Incomplete;
        if (QuestionScreen.HasCorrectlyAnswered()) status |= QuestionStatus.Correct;
        questionStatuses[currentQuestion] = status;
    }

    void UpdateButtons()
    {
        PreviousButton.Disabled = currentIndex == 0;
        NextButton.Disabled = currentIndex >= questions.Length - 1;
    }


    void LoadResultsScreen()
    {
        SaveCurrentQuestionData();

        if (quizMode) SetResultsScreenVisible(!ResultsScreen.Visible);
        else ShowResults();
    }

    int[] IncompleteQuestions()
    {
        List<int> incompleteQuestions = new(0);

        for (int i = 0; i < questions.Length; i++)
        {
            if ((questionStatuses[questions[i]] & QuestionStatus.Incomplete) != 0)
            {
                incompleteQuestions.Add(i + 1);
            }
        }

        return incompleteQuestions.ToArray();
    }

    int[] UnansweredQuestions()
    {
        List<int> unansweredQuestions = new(0);

        for (int i = 0; i < questions.Length; i++)
        {
            if ((questionStatuses[questions[i]] & QuestionStatus.Answered) == 0)
            {
                unansweredQuestions.Add(i + 1);
            }
        }

        return unansweredQuestions.ToArray();
    }

    public void AskToShowResults()
    {
        string text;
        if (UnansweredQuestions().Length != 0) text = "Warning!\nSome questions are unanswered!";
        else if (IncompleteQuestions().Length != 0) text = "Warning!\nSome questions are incomplete!";
        else text = "Show results?";

        ResultsConfirmation.DialogText = text;
        ResultsConfirmation.PopupCentered();
    }

    void ShowResults()
    {
        showAnswers = true;
        if (quizMode) ReloadScreens();
        else LoadQuestion(0);
    }

    void SetResultsScreenVisible(bool visible)
    {
        ResultsScreen.Visible = visible;
        QuestionScreen.Visible = !visible;

        if (visible)
        {
            QuestionStatus[] statuses = new QuestionStatus[questions.Length];
            for (int i = 0; i < questions.Length; i++)
            {
                Question question = questions[i];
                statuses[i] = questionStatuses[question];
            }
            ResultsScreen.LoadQuestions(statuses, showAnswers, this);
        }
    }

    void ReloadScreens()
    {
        bool resultsVisible = ResultsScreen.Visible;

        LoadQuestion(currentIndex);

        SetResultsScreenVisible(resultsVisible);
    }


    void Next()
    {
        LoadQuestion(currentIndex + 1);
    }

    void Previous()
    {
        LoadQuestion(currentIndex - 1);
    }


    void OnIndexEdit(string indexString)
    {
        IndexEdit.Text = (currentIndex + 1).ToString();
        IndexEdit.ReleaseFocus();

        if (!int.TryParse(indexString, out int index)) return;
        LoadQuestion(index - 1);
    }

    void OnIndexFocusLost()
    {
        OnIndexEdit(IndexEdit.Text);
    }


    void AskToQuit()
    {
        QuitConfirmation.PopupCentered();
    }

    void LoadMainMenu()
    {
        Node mainScene = GD.Load<PackedScene>("res://Recources/Scenes/MainMenu.tscn").Instantiate();
        GetTree().Root.AddChild(mainScene);

        QueueFree();
    }
}
