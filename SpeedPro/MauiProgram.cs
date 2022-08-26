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
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.UseMauiApp<App>().UseMauiCommunityToolkit();

		builder.Services.AddSingleton<IBluetoothLEService, BluetoothLEService>();

		builder.Services.AddSingleton<MainViewModel>();

		builder.Services.AddSingleton<MainPage>();
		return builder.Build();
	}
}
