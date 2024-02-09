using Godot;
using System;
using System.Collections.Generic;

public partial class QuestionScreen : Control
{
	[Export]
	PackedScene TitleSegment, TextSegment, CodeSegment, LineSegment;
	[Export]
	PackedScene ListAnswer, MultiAnswer, SingleAnswer, LineAnswer;


	List<Node> nodes = new();
	IAnswerNode currentAnswer;


	public void LoadQuestion(Question question)
	{
		ClearQuestion();

		if (!string.IsNullOrWhiteSpace(question.Title))
        {
            CreateSegmentFromType(SegmentType.Title).SetText(question.Title);
		}

        foreach (Segment segment in question.Segments)
		{
			ISegmentNode segmentNode = CreateSegmentFromType(segment.Type);
			if (segmentNode == null)
			{
				CreateSegmentFromType(SegmentType.Text).SetText("Unsuported segment type!");
				continue;
			}

			segmentNode.SetText(segment.Text);
		}

		Answer answer = question.Answer;
		IAnswerNode answerNode = CreateAnswerFromType(answer.Type);

		answerNode?.SetAnswers(answer.Answers, answer.CaseSensitive);
		currentAnswer = answerNode;
	}

	public void ClearQuestion()
	{
		foreach (Node node in nodes) node.QueueFree();
		nodes.Clear();
	}

	public void ShowAnswers()
	{
		currentAnswer?.ShowAnswers();
	}

	public bool HasAnswered()
	{
		if (currentAnswer == null) return false;
		return currentAnswer.HasAnswered();
	}

	public bool HasCorrectlyAnswered()
	{
        if (currentAnswer == null) return true;
		return currentAnswer.HasCorrectlyAnswered();
    }

	public object GetUserAnswers()
	{
		if (currentAnswer == null) return null;
		return currentAnswer.GetUserAnswers();
	}

	public void SetUserAnswers(object data)
	{
		currentAnswer?.SetUserAnswers(data);
	}



	ISegmentNode CreateSegmentFromType(SegmentType segmentType)
	{
        return segmentType switch
        {
			SegmentType.Title => CreateSegment(TitleSegment),
            SegmentType.Text => CreateSegment(TextSegment),
            SegmentType.Code => CreateSegment(CodeSegment),
            SegmentType.Line => CreateSegment(LineSegment),
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
            AnswerType.Multi => CreateAnswer(MultiAnswer),
            AnswerType.Single => CreateAnswer(SingleAnswer),
            AnswerType.Line => CreateAnswer(LineAnswer),
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
