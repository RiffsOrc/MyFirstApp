namespace MyFirstApp.Services;

public sealed class CountdownService
{
	public DateTime GetNextFinishTime(TimeSpan finishTime, DateTime now)
	{
		var nextFinishTime = now.Date.Add(finishTime);

		if (nextFinishTime <= now)
		{
			nextFinishTime = nextFinishTime.AddDays(1);
		}

		return nextFinishTime;
	}

	public TimeSpan GetRemainingTime(TimeSpan finishTime, DateTime now) =>
		GetNextFinishTime(finishTime, now) - now;
}
