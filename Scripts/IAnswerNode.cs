public interface IAnswerNode
{
	public void SetAnswers(AnswerOption[] answers, bool caseSensitive);

	public void ShowAnswers();

	public bool HasAnswered();

	public bool HasCorrectlyAnswered();

	public object GetUserAnswers();

	public void SetUserAnswers(object data);
}
