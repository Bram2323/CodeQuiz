using System;
using System.Collections.Generic;

public class QuestionBuilder
{
    List<IBlock> blocks = new();

    List<string> currentSegment = new();
    bool inCodeBlock = false;

    bool inAnswerBlock = false;
    AnswerType currentAnswerType = AnswerType.Empty;
    List<AnswerOption> currentAnswerOptions = new();


    public void AddLines(IEnumerable<string> lines)
    {
        foreach (string line in lines) AddLine(line);
    }

    public void AddLine(string line)
    {
        if (!inAnswerBlock) AddSegmentLine(line);
        else AddAnswerLine(line);
    }

    public Question ToQuestion()
    {
        FinalizeSegment();
        FinalizeAnswer();

        return new(blocks.ToArray());
    }


    void AddSegmentLine(string line)
    {
        if (inCodeBlock)
        {
            if (line == "```") SetCodeBlock(false);
            else AddTextSegment(line);

            return;
        }

        if (line.StartsWith("# ")) AddTitleSegment(line[2..]);
        else if (line == "---") AddLineSegment();
        else if (line == "```") SetCodeBlock(true);
        else if (line.StartsWith("=== ")) EnableAnswerBlock(line[4..]);
        else AddTextSegment(line);
    }

    void AddTitleSegment(string title)
    {
        FinalizeSegment();
        blocks.Add(new Segment(SegmentType.Title, title));
    }

    void AddLineSegment()
    {
        FinalizeSegment();
        blocks.Add(new Segment(SegmentType.Line, ""));
    }

    void AddTextSegment(string text)
    {
        currentSegment.Add(text);
    }

    void SetCodeBlock(bool enabled)
    {
        if (inCodeBlock == enabled) return;

        FinalizeSegment();
        inCodeBlock = enabled;
    }

    void FinalizeSegment()
    {
        string text = string.Join('\n', currentSegment).Trim();

        if (!string.IsNullOrEmpty(text)) blocks.Add(new Segment(inCodeBlock ? SegmentType.Code : SegmentType.Text, text));

        currentSegment.Clear();
        inCodeBlock = false;
    }


    void EnableAnswerBlock(string stringType)
    {
        if (inAnswerBlock) return;

        FinalizeSegment();

        inAnswerBlock = true;
        if (!Enum.TryParse(stringType, true, out currentAnswerType)) currentAnswerType = AnswerType.Empty;
    }

    void DisableAnswerBlock()
    {
        inAnswerBlock = false;
        FinalizeAnswer();
    }

    void AddAnswerLine(string line)
    {
        if (line == "===") DisableAnswerBlock();
        else if (line.StartsWith("+ ")) AddAnswer(line[2..], true, false);
        else if (line.StartsWith("++ ")) AddAnswer(line[2..], true, true);
        else if (line.StartsWith("- ")) AddAnswer(line[2..], false, false);
        else if (line.StartsWith("-- ")) AddAnswer(line[2..], false, true);
    }

    void AddAnswer(string text, bool correct, bool caseSensitive)
    {
        currentAnswerOptions.Add(new(text, correct, caseSensitive));
    }

    void FinalizeAnswer()
    {
        blocks.Add(new Answer(currentAnswerType, currentAnswerOptions.ToArray()));

        currentAnswerType = AnswerType.Empty;
        currentAnswerOptions.Clear();
    }
}
