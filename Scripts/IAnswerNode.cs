
public interface IAnswerNode
{
	public void SetAnswers(AnswerOption[] answers, bool caseSensitive);

	public void ShowAnswers();

	public bool HasCorrectlyAnswered();
}
