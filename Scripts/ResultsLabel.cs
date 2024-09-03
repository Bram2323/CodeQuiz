using Godot;

public partial class ResultsLabel : Control
{
    [Export]
    Label Amount, Total, Percent;

    public void SetCorrect(int correct, int total)
    {
        Amount.Text = correct.ToString();
        Total.Text = total.ToString();

        double percent = (double)correct / total * 100d;
        if (total == 0) percent = 0;
        Percent.Text = "(" + percent.ToString("##0.#") + "%)";
    }
}
