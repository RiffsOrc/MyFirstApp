namespace MyFirstApp;

// A base class contains behavior that other kinds of countdowns can reuse.
public abstract class CountdownTimer
{
	public DateTime FinishTime { get; }
	public TimeSpan TimeRemaining { get; private set; }
	public bool IsFinished => TimeRemaining == TimeSpan.Zero;

	protected CountdownTimer(DateTime finishTime)
	{
		FinishTime = finishTime;
		Update(DateTime.Now);
	}

	public void Update(DateTime currentTime)
	{
		if (currentTime >= FinishTime)
		{
			TimeRemaining = TimeSpan.Zero;
		}
		else
		{
			TimeRemaining = FinishTime - currentTime;
		}
	}

	public string GetTimeText()
	{
		int hours = (int)TimeRemaining.TotalHours;
		return $"{hours:00}:{TimeRemaining.Minutes:00}:{TimeRemaining.Seconds:00}";
	}

	public abstract string GetCompletionMessage(int completedWorkdays);
}