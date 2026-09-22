namespace MyFirstApp.Models;

public sealed class WorkdaySettings
{
	public int FinishHour { get; set; } = 17;

	public int FinishMinute { get; set; }

	public TimeSpan FinishTime
	{
		get => new(FinishHour, FinishMinute, 0);
		set
		{
			FinishHour = value.Hours;
			FinishMinute = value.Minutes;
		}
	}
}
