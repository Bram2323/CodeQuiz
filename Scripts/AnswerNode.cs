using Godot;

public abstract partial class AnswerNode : Control
{
    public abstract void SetAnswers(AnswerOption[] answers);

    public abstract void ShowAnswers();

    public abstract bool HasAnswered();

    public abstract bool IsIncomplete();

    public abstract bool HasCorrectlyAnswered();

    public abstract object GetUserAnswers();

    public abstract void SetUserAnswers(object data);
}
