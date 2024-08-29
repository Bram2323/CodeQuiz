using Godot;

public partial class ExplanationContainer : ContainerNode
{
    [Export]
    Control Container;

    public override Control Control => Container;

    public override bool CanAdd(IBlock block)
    {
        return block is not Answer;
    }

    public override void OnAnswerShow()
    {
        Visible = true;
    }
}
