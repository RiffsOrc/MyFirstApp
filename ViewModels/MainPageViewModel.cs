using System.Collections.ObjectModel;
using MyFirstApp.Models;
using MyFirstApp.Services;

namespace MyFirstApp.ViewModels;

public sealed class MainPageViewModel : ObservableObject
{
	private readonly CountdownService _countdownService;
	private readonly INotificationService _notificationService;
	private readonly WorkdaySettings _settings;
	private readonly IWorkdaySettingsStore _settingsStore;
	private readonly IDispatcherTimer _timer;
	private string _countdownText = "00:00:00";
	private bool _isCountdownRunning;
	private string _savedSelectionText = string.Empty;
	private TimeSpan _selectedFinishTime;
	private string _statusMessage = "Choose a finish time and press Start countdown.";
	private DateTime? _targetFinishTime;

	public MainPageViewModel(
		IDispatcher dispatcher,
		IWorkdaySettingsStore settingsStore,
		CountdownService countdownService,
		INotificationService notificationService)
	{
		_settingsStore = settingsStore;
		_countdownService = countdownService;
		_notificationService = notificationService;
		_settings = _settingsStore.Load();
		_selectedFinishTime = _settings.FinishTime;

		_timer = dispatcher.CreateTimer();
		_timer.Interval = TimeSpan.FromSeconds(1);
		_timer.Tick += OnTimerTick;

		StartCountdownCommand = new Command(StartCountdown);
		StopCountdownCommand = new Command(StopCountdown);
		LearningTopics = new ObservableCollection<LearningTopic>();

		LoadLearningTopics();
		RefreshSavedSelectionText();
		StatusMessage = "The app remembers the last chosen time so learners can explore persistence.";
	}

	public string CountdownText
	{
		get => _countdownText;
		private set => SetProperty(ref _countdownText, value);
	}

	public bool IsCountdownRunning
	{
		get => _isCountdownRunning;
		private set => SetProperty(ref _isCountdownRunning, value);
	}

	public ObservableCollection<LearningTopic> LearningTopics { get; }

	public string SavedSelectionText
	{
		get => _savedSelectionText;
		private set => SetProperty(ref _savedSelectionText, value);
	}

	public TimeSpan SelectedFinishTime
	{
		get => _selectedFinishTime;
		set
		{
			if (!SetProperty(ref _selectedFinishTime, value))
			{
				return;
			}

			RefreshSavedSelectionText();

			if (!IsCountdownRunning)
			{
				StatusMessage = "Press Start countdown to begin the timer.";
			}
		}
	}

	public Command StartCountdownCommand { get; }

	public Command StopCountdownCommand { get; }

	public string StatusMessage
	{
		get => _statusMessage;
		private set => SetProperty(ref _statusMessage, value);
	}

	private void CompleteCountdown()
	{
		_timer.Stop();
		IsCountdownRunning = false;
		CountdownText = "00:00:00";
		StatusMessage = "Countdown complete. A reminder was shown to the user.";
		_targetFinishTime = null;
		_ = _notificationService.ShowCountdownCompleteAsync();
	}

	private void LoadLearningTopics()
	{
		var starterTopics = new[]
		{
			new LearningTopic("Variables and classes", "The selected time lives in properties, and the app state is grouped into classes."),
			new LearningTopic("Inheritance", "MainPageViewModel inherits from ObservableObject so property changes update the UI."),
			new LearningTopic("If/else logic", "The countdown decides whether the chosen time is later today or needs to move to tomorrow."),
			new LearningTopic("Loops and collections", "The starter topics are stored in a collection and can be extended in lessons."),
			new LearningTopic("Persistence", "The last chosen finish time is saved locally with Preferences so it survives app restarts.")
		};

		foreach (var topic in starterTopics)
		{
			LearningTopics.Add(topic);
		}
	}

	private void OnTimerTick(object? sender, EventArgs e)
	{
		if (_targetFinishTime is null)
		{
			return;
		}

		var remaining = _targetFinishTime.Value - DateTime.Now;

		if (remaining <= TimeSpan.Zero)
		{
			CompleteCountdown();
			return;
		}

		CountdownText = $"{(int)remaining.TotalHours:00}:{remaining.Minutes:00}:{remaining.Seconds:00}";
	}

	private void RefreshSavedSelectionText() =>
		SavedSelectionText = $"Saved finish time: {SelectedFinishTime:hh\\:mm}";

	private void StartCountdown()
	{
		_settings.FinishTime = SelectedFinishTime;
		_settingsStore.Save(_settings);
		_targetFinishTime = _countdownService.GetNextFinishTime(SelectedFinishTime, DateTime.Now);

		var startsTomorrow = _targetFinishTime.Value.Date > DateTime.Now.Date;

		if (startsTomorrow)
		{
			StatusMessage = "That time has already passed today, so the countdown now targets tomorrow.";
		}
		else
		{
			StatusMessage = "Countdown running for today's finish time.";
		}

		IsCountdownRunning = true;
		RefreshSavedSelectionText();
		OnTimerTick(this, EventArgs.Empty);
		_timer.Start();
	}

	private void StopCountdown()
	{
		_timer.Stop();
		IsCountdownRunning = false;
		_targetFinishTime = null;
		CountdownText = "00:00:00";
		StatusMessage = "Countdown stopped. You can change the time and start again.";
	}
}
