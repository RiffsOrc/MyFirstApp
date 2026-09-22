# Work Finish Timer

This beginner .NET MAUI app counts down to the end of a workday. The selected
finish time and number of completed workdays are saved between app launches.

## Programming concepts

- **Variables:** `MainPage.xaml.cs` stores the timer and completed-workday count.
- **If/else:** `CountdownTimer.Update` decides whether time remains.
- **Lists and loops:** `WorkdayTimer.GetCompletionMessage` loops through funny messages.
- **Classes:** each C# file contains a class with one clear job.
- **Inheritance:** `WorkdayTimer` inherits reusable behavior from `CountdownTimer`.
- **Objects:** `MainPage` creates a `WorkdayTimer` object when the user starts the timer.
- **Persistence:** MAUI `Preferences` saves small values on the device.

Start with `MainPage.xaml` for the screen, then read `MainPage.xaml.cs`,
`CountdownTimer.cs`, and `WorkdayTimer.cs` in that order.