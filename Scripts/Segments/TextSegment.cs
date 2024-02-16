using Godot;

public partial class TextSegment : SegmentNode
{
    [Export]
    RichTextLabel label;

    public override void SetText(string text)
    {
        label.Text = text;
    }
}
