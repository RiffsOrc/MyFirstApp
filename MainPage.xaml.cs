namespace MyFirstApp;

public partial class MainPage : ContentPage
{
	private const string FinishTimeKey = "finish_time";
	private const string ActiveFinishKey = "active_finish";
	private const string CompletedWorkdaysKey = "completed_workdays";

	private readonly IDispatcherTimer countdownLoop;
	private WorkdayTimer? workdayTimer;
	private int completedWorkdays;

	public MainPage()
	{
		InitializeComponent();

		// Loops fill the drop-down lists with 00-23 hours and 00-59 minutes.
		for (int hour = 0; hour < 24; hour++)
		{
			HourPicker.Items.Add(hour.ToString("00"));
		}

		for (int minute = 0; minute < 60; minute++)
		{
			MinutePicker.Items.Add(minute.ToString("00"));
		}

		TimeSpan savedTime = TimeSpan.FromTicks(
			Preferences.Default.Get(FinishTimeKey, new TimeSpan(17, 0, 0).Ticks));
		HourPicker.SelectedIndex = savedTime.Hours;
		MinutePicker.SelectedIndex = savedTime.Minutes;

		completedWorkdays = Preferences.Default.Get(CompletedWorkdaysKey, 0);
		ShowCompletedWorkdays();

		countdownLoop = Dispatcher.CreateTimer();
		countdownLoop.Interval = TimeSpan.FromSeconds(1);
		countdownLoop.Tick += OnCountdownTick;

		RestoreCountdown();
	}

	private void OnStartClicked(object? sender, EventArgs e)
	{
		TimeSpan finishTimeOfDay = new TimeSpan(HourPicker.SelectedIndex, MinutePicker.SelectedIndex, 0);
		DateTime finishTime = DateTime.Today.Add(finishTimeOfDay);

		if (finishTime <= DateTime.Now)
		{
			finishTime = finishTime.AddDays(1);
		}

		Preferences.Default.Set(FinishTimeKey, finishTimeOfDay.Ticks);
		Preferences.Default.Set(ActiveFinishKey, finishTime.ToString("O"));
		StartCountdown(finishTime);
	}

	private void StartCountdown(DateTime finishTime)
	{
		workdayTimer = new WorkdayTimer(finishTime);
		StatusLabel.Text = $"Work ends at {finishTime:h:mm tt}.";
		UpdateCountdown();
		countdownLoop.Start();
	}

	private async void OnCountdownTick(object? sender, EventArgs e)
	{
		if (workdayTimer is null)
		{
			return;
		}

		UpdateCountdown();

		if (workdayTimer.IsFinished)
		{
			countdownLoop.Stop();
			Preferences.Default.Remove(ActiveFinishKey);
			completedWorkdays++;
			Preferences.Default.Set(CompletedWorkdaysKey, completedWorkdays);
			ShowCompletedWorkdays();

			string message = workdayTimer.GetCompletionMessage(completedWorkdays);
			StatusLabel.Text = message;
			await DisplayAlertAsync("Workday complete", message, "Clock out");
		}
	}

	private void UpdateCountdown()
	{
		if (workdayTimer is null)
		{
			return;
		}

		workdayTimer.Update(DateTime.Now);
		CountdownLabel.Text = workdayTimer.GetTimeText();
	}

	private void RestoreCountdown()
	{
		string savedFinishTime = Preferences.Default.Get(ActiveFinishKey, string.Empty);

		if (DateTime.TryParse(savedFinishTime, out DateTime finishTime))
		{
			StartCountdown(finishTime);
		}
	}

	private void ShowCompletedWorkdays()
	{
		CompletedCountLabel.Text = $"Completed workdays: {completedWorkdays}";
	}
}
