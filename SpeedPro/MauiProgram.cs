using CommunityToolkit.Maui;
using SpeedPro.Services;
using SpeedPro.ViewModels;

namespace SpeedPro;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("fa.ttf", "fa");
				fonts.AddFont("coolvetica.ttf", "coolvetica");
				fonts.AddFont("VarelaRound-Regular.ttf", "Varela");
			});
		builder.Services.AddSingleton<IBluetoothLEService, BluetoothLEService>();

		builder.Services.AddSingleton<MainViewModel>();

		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<AppShell>();

		Routing.RegisterRoute("home", typeof(MainPage));
		return builder.Build();
	}
}
