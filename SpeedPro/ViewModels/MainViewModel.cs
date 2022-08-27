using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.BLE;
using Plugin.BLE.Abstractions.EventArgs;
using SpeedPro.Models;
using SpeedPro.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
            ScannedDevices = new ObservableCollection<BluetoothLEDevice>();
        }

        private void OnDeviceDiscovered(object sender, DeviceEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Device.Name)) return;
            ScannedDevices.Add(new BluetoothLEDevice(e.Device.Name, e.Device));

            if (!IsDeviceListVisible)
            {
                IsDeviceListVisible = true;
            }
        }

        public ICommand ScanCommand => new RelayCommand(async () =>
        {
            if (IsScanningDevices) return;
            IsDeviceListVisible = false;
            IsScanningDevices = true;

            ScannedDevices.Clear();
            await _bluetoothLEService.ScanDevicesAsync();

            IsScanningDevices = false;
        });

        public void OnDeviceSelected()
        {
            if (_selectedDevice is null) return;
            IsConnectingVehicle = true;
            Task.Run(async () =>
            {

                bool isVehicleConnected = await _bluetoothLEService.ConnectDeviceAsync(SelectedDevice.Device);
                if (isVehicleConnected)
                {
                    Debug.WriteLine("Connected to vehicle.");
                }
                IsConnectingVehicle = false;
            });
        }

        #region Binding Properties
        [ObservableProperty] bool _isConnectingVehicle;
        [ObservableProperty] bool _isScanningDevices;
        [ObservableProperty] bool _isDeviceListVisible;

        [ObservableProperty] ObservableCollection<BluetoothLEDevice> _scannedDevices;

        private BluetoothLEDevice _selectedDevice;
        public BluetoothLEDevice SelectedDevice
        {
            get => _selectedDevice;
            set
            {
                if (!IsConnectingVehicle)
                {
                    _selectedDevice = value;
                    OnDeviceSelected();
                    OnPropertyChanged();
                }
            }
        }
        #endregion
    }
}
