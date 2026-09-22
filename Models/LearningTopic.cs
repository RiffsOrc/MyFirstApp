namespace MyFirstApp.Models;

public sealed class LearningTopic
{
	public LearningTopic(string title, string description)
	{
		Title = title;
		Description = description;
	}

	public string Title { get; }

	public string Description { get; }
}
