using Godot;

public partial class CodeSegment : SegmentNode
{
    [Export]
    CodeEdit codeEdit;
    [Export]
    SyntaxHighlighter syntaxHighlighter;

    public override void _Ready()
    {
        codeEdit.SyntaxHighlighter = syntaxHighlighter;
    }

    public override void SetText(string text)
    {
        codeEdit.Text = text;
    }
}
