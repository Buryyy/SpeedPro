using SpeedPro.ViewModels;

namespace SpeedPro;

public partial class MainPage : ContentPage
{
	private readonly MainViewModel viewModel;
	public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
		this.viewModel = viewModel;
		BindingContext = this.viewModel;
	}


}

