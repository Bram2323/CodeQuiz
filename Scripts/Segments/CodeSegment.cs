using Godot;
using System;

public partial class CodeSegment : Node, ISegmentNode
{
	[Export]
	CodeEdit codeEdit;
	[Export]
	SyntaxHighlighter syntaxHighlighter;

    public override void _Ready()
    {
        codeEdit.SyntaxHighlighter = syntaxHighlighter;
    }

    public void SetText(string text)
	{
		codeEdit.Text = text;
		codeEdit.HighlightCurrentLine = false;
		//codeEdit.Editable = false;
	}
}
