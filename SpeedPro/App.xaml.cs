using SpeedPro.Helpers;

namespace SpeedPro;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		App.Current.UserAppTheme = AppTheme.Light;
		MainPage = ServiceHelper.GetService<AppShell>();
	}
}
