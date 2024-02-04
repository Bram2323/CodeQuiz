
public interface IAnswerNode
{
	public void SetAnswers(string[] answers, bool caseSensitive);

	public bool HasCorrectlyAnswered();
}
