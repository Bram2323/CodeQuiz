using Godot;

public partial class QuestionManager : Node
{
	[Export]
	QuestionGenerator QuestionGenerator;

	[Export]
	Button NextButton, PreviousButton;


    public override void _Ready()
    {
        NextButton.Pressed += Next;
        PreviousButton.Pressed += Previous;
    }
    
    public void LoadQuestion(Question question)
    {
        QuestionGenerator.LoadQuestion(question);
    }

    void Next()
    {
        QuestionGenerator.ShowAnswers();
    }

    void Previous()
    {
        Node mainScene = GD.Load<PackedScene>("res://Recources/Scenes/MainMenu.tscn").Instantiate();
        GetTree().Root.AddChild(mainScene);

        QueueFree();
    }
}
