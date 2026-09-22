using Microsoft.Extensions.DependencyInjection;

namespace MyFirstApp;

public partial class AppShell : Shell
{
    public AppShell(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        HomeContent.ContentTemplate = new DataTemplate(() => serviceProvider.GetRequiredService<MainPage>());
    }
}
