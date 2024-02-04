using Godot;
using System;
using System.Collections.Generic;

public partial class QuestionGenerator : Node
{
	[Export]
	PackedScene TitleSegment, TextSegment, CodeSegment;
	[Export]
	PackedScene ListAnswer;


	List<Node> nodes = new();

    public override void _Ready()
    {
		Segment[] segments = new Segment[] {
            new(SegmentType.Code, "void test(){\n    boolean test = false;\n}"),
            new(SegmentType.Text, "This is some text!"),
			new(SegmentType.Code, "void test(){\n    boolean test = false;\n}"),
		};
		Answer answer = new(AnswerType.List, new string[] {
			"byte",
			"short",
			"int",
			"long",
			"float",
			"double",
			"char",
			"boolean",
		}, false);
		Question question = new("Test Question", segments, answer);

		LoadQuestion(question);
    }


	public void LoadQuestion(Question question)
	{
		ClearQuestion();

		ISegmentNode title = CreateSegmentFromType(SegmentType.Title);
		if (!string.IsNullOrWhiteSpace(question.Title)) title.SetText(question.Title);
		else title.SetText("Question:");

        foreach (Segment segment in question.Segments)
		{
			ISegmentNode segmentNode = CreateSegmentFromType(segment.Type);
			if (segmentNode == null)
			{
				segmentNode = CreateSegmentFromType(SegmentType.Text);
				segmentNode.SetText("Unsuported segment type!");
				continue;
			}

			segmentNode.SetText(segment.Text);
		}

		Answer answer = question.Answer;
		IAnswerNode answerNode = CreateAnswerFromType(answer.Type);

		answerNode.SetAnswers(answer.Answers, answer.CaseSensitive);
	}

	public void ClearQuestion()
	{
		foreach (Node node in nodes) node.QueueFree();
		nodes.Clear();
	}


	ISegmentNode CreateSegmentFromType(SegmentType segmentType)
	{
        return segmentType switch
        {
			SegmentType.Title => CreateSegment(TitleSegment),
            SegmentType.Text => CreateSegment(TextSegment),
            SegmentType.Code => CreateSegment(CodeSegment),
            _ => null,
        };
    }

    ISegmentNode CreateSegment(PackedScene packedScene)
	{
		Node segment = packedScene.Instantiate();
		if (segment is not ISegmentNode segmentInterface) throw new ArgumentException("Scene was not of type " + nameof(ISegmentNode) + "!", nameof(packedScene));

		AddChild(segment);
		nodes.Add(segment);

		return segmentInterface;
	}


    IAnswerNode CreateAnswerFromType(AnswerType answerType)
    {
        return answerType switch
        {
            AnswerType.List => CreateAnswer(ListAnswer),
            _ => null,
        };
    }

    IAnswerNode CreateAnswer(PackedScene packedScene)
    {
        Node answer = packedScene.Instantiate();
        if (answer is not IAnswerNode answerInterface) throw new ArgumentException("Scene was not of type " + nameof(IAnswerNode) + "!", nameof(packedScene));

        AddChild(answer);
        nodes.Add(answer);

        return answerInterface;
    }
}
