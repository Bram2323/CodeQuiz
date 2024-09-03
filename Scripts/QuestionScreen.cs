using Godot;
using System.Collections.Generic;

public partial class QuestionScreen : Control
{
    [Export]
    Control Container;
    [ExportGroup("Segments")]
    [Export]
    PackedScene TitleSegment, TextSegment, CodeSegment, LineSegment;
    [ExportGroup("Answers")]
    [Export]
    PackedScene ListAnswer, MultiAnswer, SingleAnswer, LineAnswer;
    [ExportGroup("Containers")]
    [Export]
    PackedScene PanelContainer, ExplanationContainer;


    List<SegmentNode> segments = new();
    List<ContainerNode> containers = new();
    List<AnswerNode> answers = new();


    public void LoadQuestion(Question question)
    {
        ClearQuestion();

        foreach (IBlock block in question.Blocks)
        {
            CreateBlock(block, Container);
        }
    }

    public void ClearQuestion()
    {
        foreach (SegmentNode segment in segments) segment.QueueFree();
        foreach (AnswerNode answer in answers) answer.QueueFree();
        foreach (ContainerNode container in containers) container.QueueFree();
        segments.Clear();
        answers.Clear();
        containers.Clear();
    }

    public void ShowAnswers()
    {
        foreach (AnswerNode answer in answers)
        {
            answer.ShowAnswers();
        }

        foreach (ContainerNode container in containers)
        {
            container.OnAnswerShow();
        }
    }

    public bool HasAnswered()
    {
        foreach (AnswerNode answer in answers)
        {
            if (answer.HasAnswered()) return true;
        }
        return false;
    }

    public bool IsIncomplete()
    {
        foreach (AnswerNode answer in answers)
        {
            if (answer.IsIncomplete()) return true;
        }
        return false;
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
        foreach (AnswerNode answer in answers)
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




    void CreateBlock(IBlock block, Control parent)
    {
        if (block is Segment segment) CreateSegmentFromType(segment.Type, parent)?.SetText(segment.Text);
        else if (block is Answer answer) CreateAnswerFromType(answer.Type, parent)?.SetAnswers(answer.Answers);
        else if (block is Container container) CreateContainer(container, parent);
    }


    SegmentNode CreateSegmentFromType(SegmentType segmentType, Control parent)
    {
        return segmentType switch
        {
            SegmentType.Title => CreateSegmentNode(TitleSegment, parent),
            SegmentType.Text => CreateSegmentNode(TextSegment, parent),
            SegmentType.Code => CreateSegmentNode(CodeSegment, parent),
            SegmentType.Line => CreateSegmentNode(LineSegment, parent),
            _ => null,
        };
    }

    SegmentNode CreateSegmentNode(PackedScene packedScene, Control parent)
    {
        SegmentNode segment = packedScene.Instantiate<SegmentNode>();

        parent.AddChild(segment);
        segments.Add(segment);

        return segment;
    }


    AnswerNode CreateAnswerFromType(AnswerType answerType, Control parent)
    {
        return answerType switch
        {
            AnswerType.List => CreateAnswerNode(ListAnswer, parent),
            AnswerType.Multi => CreateAnswerNode(MultiAnswer, parent),
            AnswerType.Single => CreateAnswerNode(SingleAnswer, parent),
            AnswerType.Line => CreateAnswerNode(LineAnswer, parent),
            _ => null,
        };
    }

    AnswerNode CreateAnswerNode(PackedScene packedScene, Control parent)
    {
        AnswerNode answer = packedScene.Instantiate<AnswerNode>();

        answer.SizeFlagsVertical = answers.Count == 0 ? SizeFlags.ShrinkEnd | SizeFlags.Expand : SizeFlags.ShrinkEnd;

        parent.AddChild(answer);
        answers.Add(answer);

        return answer;
    }



    ContainerNode CreateContainer(Container container, Control parent)
    {
        ContainerNode containerNode = CreateContainerFromType(container.Type, parent);
        if (containerNode == null) return null;

        foreach (IBlock block in container.Blocks)
        {
            if (!containerNode.CanAdd(block)) continue;

            CreateBlock(block, containerNode.Control);
        }

        return containerNode;
    }

    ContainerNode CreateContainerFromType(ContainerType containerType, Control parent)
    {
        return containerType switch
        {
            ContainerType.Explanation => CreateContainerNode(ExplanationContainer, parent),
            ContainerType.Panel => CreateContainerNode(PanelContainer, parent),
            _ => null,
        };
    }

    ContainerNode CreateContainerNode(PackedScene packedScene, Control parent)
    {
        ContainerNode container = packedScene.Instantiate<ContainerNode>();

        parent.AddChild(container);
        containers.Add(container);

        return container;
    }


}
