using MyFirstApp.Services;
using MyFirstApp.ViewModels;

namespace MyFirstApp;

public partial class MainPage : ContentPage
{
	public MainPage()
		: this(new MainPageViewModel(
			Application.Current!.Dispatcher,
			new PreferencesWorkdaySettingsStore(),
			new CountdownService(),
			new AlertNotificationService()))
	{
	}

	public MainPage(MainPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
