using Godot;

public partial class AnsweredLabel : Control
{
    [Export]
    Label Amount, Total;

    public void SetAnswered(int amount, int total)
    {
        Amount.Text = amount.ToString();
        Total.Text = total.ToString();
    }
}
