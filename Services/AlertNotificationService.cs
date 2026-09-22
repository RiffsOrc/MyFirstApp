namespace MyFirstApp.Services;

public sealed class AlertNotificationService : INotificationService
{
	public Task ShowCountdownCompleteAsync()
	{
		var title = DeviceInfo.Platform == DevicePlatform.WinUI ? "Desktop message" : "Phone reminder";
		var page = Application.Current?.Windows.FirstOrDefault()?.Page;

		if (page is null)
		{
			return Task.CompletedTask;
		}

		return MainThread.InvokeOnMainThreadAsync(() =>
			page.DisplayAlertAsync(title, "You are done for the day. Time to switch to your next activity.", "OK"));
	}
}
