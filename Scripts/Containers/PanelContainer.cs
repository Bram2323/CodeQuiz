using Godot;

public partial class PanelContainer : ContainerNode
{
    [Export]
    Control Container;

    public override Control Control => Container;

    public override bool CanAdd(IBlock block)
    {
        return block is not Answer;
    }
}
