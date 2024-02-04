using Godot;
using System;

public partial class TextSegment : RichTextLabel, ISegmentNode
{
	public void SetText(string text)
	{
		Text = text;
	}
}
