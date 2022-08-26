using CommunityToolkit.Mvvm.Input;
using SpeedPro.Services;
using System.Windows.Input;

namespace SpeedPro.ViewModels
{
    public class MainViewModel
    {
        //create private field and initialise it from constructor from BluetoothLEService
        private IBluetoothLEService _bluetoothLEService;

        public MainViewModel(IBluetoothLEService bluetoothLEService)
        {
            _bluetoothLEService = bluetoothLEService;
        }

        public ICommand ScanCommand => new RelayCommand(async () =>
        {

            await _bluetoothLEService.ScanDevicesAsync();
        });
    }
}
