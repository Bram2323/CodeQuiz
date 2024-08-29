using Godot;

public abstract partial class ContainerNode : Control
{
    public abstract Control Control { get; }

    public abstract bool CanAdd(IBlock block);


    public virtual void OnAnswerShow() { }
}
