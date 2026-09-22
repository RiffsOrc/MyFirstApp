using Microsoft.Extensions.DependencyInjection;
using MyFirstApp.Services;
using MyFirstApp.ViewModels;

namespace MyFirstApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<CountdownService>();
		builder.Services.AddSingleton<IWorkdaySettingsStore, PreferencesWorkdaySettingsStore>();
		builder.Services.AddSingleton<INotificationService, AlertNotificationService>();
		builder.Services.AddSingleton<MainPageViewModel>();
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<AppShell>();

		return builder.Build();
	}
}
