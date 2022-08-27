using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.BLE;
using Plugin.BLE.Abstractions.EventArgs;
using SpeedPro.Models;
using SpeedPro.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SpeedPro.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IBluetoothLEService _bluetoothLEService;

        public MainViewModel(IBluetoothLEService bluetoothLEService)
        {
            _bluetoothLEService = bluetoothLEService;
            CrossBluetoothLE.Current.Adapter.DeviceDiscovered += OnDeviceDiscovered;
            ScannedDevices = new ObservableCollection<BluetoothDevice>();
        }

        private void OnDeviceDiscovered(object sender, DeviceEventArgs e)
        {
            string deviceName = string.IsNullOrEmpty(e.Device.Name) ? "Unnamed device" : e.Device.Name;
            ScannedDevices.Add(new BluetoothDevice(deviceName, e.Device));
        }

        public ICommand ScanCommand => new RelayCommand(async () =>
        {
            IsScanningDevices = true;
            ScannedDevices.Clear();
            await _bluetoothLEService.ScanDevicesAsync();
            IsScanningDevices = false;
        });

        [ObservableProperty] bool isScanningDevices;
        [ObservableProperty] bool showScannedDevices;
        [ObservableProperty] ObservableCollection<BluetoothDevice> scannedDevices;
    }
}
