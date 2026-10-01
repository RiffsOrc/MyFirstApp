namespace MyFirstApp;

// Step n: implement the WorkdayTimer class, which provides custom messages for the completion of a workday.

// Step n: implement the custom Completion Message, that gets shown whenever the timer finishes

// Inheritance: WorkdayTimer receives the countdown behavior from CountdownTimer.
public class WorkdayTimer //: CountdownTimer
{
	private readonly List<string> messages =
	[
		"Laptop closed. Freedom activated!",
		"Your shift has left the chat.",
		"Great work. Future you can handle the rest tomorrow.",
		"Time to clock out and power down."
	];

	public WorkdayTimer(DateTime finishTime) //: base(finishTime)
	{
	}
}