using MyFirstApp.Models;

namespace MyFirstApp.Services;

public interface IWorkdaySettingsStore
{
	WorkdaySettings Load();

	void Save(WorkdaySettings settings);
}
