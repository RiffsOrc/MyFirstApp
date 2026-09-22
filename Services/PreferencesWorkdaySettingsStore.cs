using MyFirstApp.Models;

namespace MyFirstApp.Services;

public sealed class PreferencesWorkdaySettingsStore : IWorkdaySettingsStore
{
	private const string FinishMinutesKey = "finish_minutes";
	private const int DefaultFinishMinutes = 17 * 60;

	public WorkdaySettings Load()
	{
		var finishMinutes = Preferences.Default.Get(FinishMinutesKey, DefaultFinishMinutes);

		if (finishMinutes < 0 || finishMinutes >= 24 * 60)
		{
			finishMinutes = DefaultFinishMinutes;
		}

		return new WorkdaySettings
		{
			FinishHour = finishMinutes / 60,
			FinishMinute = finishMinutes % 60
		};
	}

	public void Save(WorkdaySettings settings)
	{
		var finishMinutes = (settings.FinishHour * 60) + settings.FinishMinute;
		Preferences.Default.Set(FinishMinutesKey, finishMinutes);
	}
}
