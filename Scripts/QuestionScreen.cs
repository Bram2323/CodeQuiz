using Godot;
using System;
using System.Collections.Generic;

public partial class QuestionScreen : Control
{
    [Export]
    PackedScene TitleSegment, TextSegment, CodeSegment, LineSegment;
    [Export]
    PackedScene ListAnswer, MultiAnswer, SingleAnswer, LineAnswer;


    List<SegmentNode> segments = new();
    List<AnswerNode> answers = new();


    public void LoadQuestion(Question question)
    {
        ClearQuestion();

        foreach (IBlock block in question.Blocks)
        {
            if (block is Segment segment) CreateSegmentFromType(segment.Type)?.SetText(segment.Text);
            else if (block is Answer answer) CreateAnswerFromType(answer.Type)?.SetAnswers(answer.Answers);
        }
    }

    public void ClearQuestion()
    {
        foreach (SegmentNode segment in segments) segment.QueueFree();
        foreach (AnswerNode answer in answers) answer.QueueFree();
        segments.Clear();
        answers.Clear();
    }

    public void ShowAnswers()
    {
        foreach (AnswerNode answer in answers)
        {
            answer.ShowAnswers();
        }
    }

    public bool HasAnswered()
    {
        foreach (AnswerNode answer in answers)
        {
            if (!answer.HasAnswered()) return false;
        }
        return true;
    }

    public bool HasCorrectlyAnswered()
    {
        foreach (AnswerNode answer in answers)
        {
            if (!answer.HasCorrectlyAnswered()) return false;
        }
        return true;
    }

    public object[] GetUserAnswers()
    {
        List<object> data = new();
        foreach(AnswerNode answer in answers)
        {
            data.Add(answer.GetUserAnswers());
        }
        return data.ToArray();
    }

    public void SetUserAnswers(object[] data)
    {
        if (data.Length != answers.Count) return;

        for (int i = 0; i < data.Length; i++)
        {
            answers[i].SetUserAnswers(data[i]);
        }
    }



    SegmentNode CreateSegmentFromType(SegmentType segmentType)
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

    SegmentNode CreateSegment(PackedScene packedScene)
    {
        SegmentNode segment = packedScene.Instantiate<SegmentNode>();
        
        AddChild(segment);
        segments.Add(segment);

        return segment;
    }


    AnswerNode CreateAnswerFromType(AnswerType answerType)
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

    AnswerNode CreateAnswer(PackedScene packedScene)
    {
        AnswerNode answer = packedScene.Instantiate<AnswerNode>();

        answer.SizeFlagsVertical = answers.Count == 0 ? SizeFlags.ShrinkEnd | SizeFlags.Expand : SizeFlags.ShrinkEnd;

        AddChild(answer);
        answers.Add(answer);

        return answer;
    }
}
