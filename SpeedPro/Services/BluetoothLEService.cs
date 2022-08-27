using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;

namespace SpeedPro.Services
{
    public class BluetoothLEService : IBluetoothLEService
    {
        private readonly string[] PossibleUUIDs = new string[]{ "00008888-0000-1000-8000-00805f9b34fb",
        "00008877-0000-1000-8000-00805f9b34fb",
        "00002902-0000-1000-8000-00805f9b34fb"};

        private readonly IBluetoothLE _bluetooth;
        private readonly IAdapter _adapter;


        public BluetoothLEService()
        {
            _bluetooth = CrossBluetoothLE.Current;
            _adapter = CrossBluetoothLE.Current.Adapter;
            _bluetooth.StateChanged += OnStateChanged;
        }

        private void OnStateChanged(object sender, BluetoothStateChangedArgs e)
        {
            Console.WriteLine(e.NewState.ToString());
        }


        public async Task ScanDevicesAsync()
        {
#if ANDROID
if(!SpeedPro.MainActivity.IsBLEAccessGranted()) {
SpeedPro.MainActivity.RequestBlePermissions(SpeedPro.MainActivity.Activity, 0);
}
#endif

            await _adapter.StartScanningForDevicesAsync();

            //await _adapter.DiscoverDeviceAsync(new Guid("00001101-0000-1000-8000-00805F9B34FB"));
        }

        ~BluetoothLEService()
        {
            _bluetooth.StateChanged -= OnStateChanged;
        }

    }
}