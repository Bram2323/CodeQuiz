using Godot;

public partial class InputField : Node
{
    [Export]
    LineEdit LineEdit;
    [Export]
    Button TextButton;

    string correctAnswer = null;
    bool caseSensitive = false;

    public void SetAnswer(string answer, bool caseSensitive)
    {
        TextButton.Text = "?";
        correctAnswer = answer;
        this.caseSensitive = caseSensitive;

        TextButton.Disabled = false;
        TextButton.ButtonPressed = false;
    }

    public void ShowAnswer()
    {
        LineEdit.Editable = false;

        TextButton.Text = correctAnswer;
        if (HasCorrectlyAnswered()) TextButton.Disabled = true;
        else TextButton.ButtonPressed = true;
    }

    public bool HasCorrectlyAnswered()
    {
        if (caseSensitive) return correctAnswer == LineEdit.Text.Trim();
        else return correctAnswer.ToLower() == LineEdit.Text.Trim().ToLower();
    }

    public string GetText()
    {
        return LineEdit.Text;
    }

    public void SetText(string text)
    {
        LineEdit.Text = text;
    }
}
