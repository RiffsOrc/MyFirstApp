namespace MyFirstApp;

// Inheritance: WorkdayTimer receives the countdown behavior from CountdownTimer.
public class WorkdayTimer : CountdownTimer
{
	private readonly List<string> messages =
	[
		"Laptop closed. Freedom activated!",
		"Your shift has left the chat.",
		"Great work. Future you can handle the rest tomorrow.",
		"Time to clock out and power down."
	];

	public WorkdayTimer(DateTime finishTime) : base(finishTime)
	{
	}

	public override string GetCompletionMessage(int completedWorkdays)
	{
		int messageNumber = (completedWorkdays - 1) % messages.Count;

		// This loop demonstrates checking each item in a list.
		for (int index = 0; index < messages.Count; index++)
		{
			if (index == messageNumber)
			{
				return messages[index];
			}
		}

		return "The workday is over!";
	}
}