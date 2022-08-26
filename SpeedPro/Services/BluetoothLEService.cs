using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;

namespace SpeedPro.Services
{
    public class BluetoothLEService : IBluetoothLEService
    {
        private const string UUID1 = "00008888-0000-1000-8000-00805f9b34fb";
        private const string UUID2 = "00008877-0000-1000-8000-00805f9b34fb";
        private const string UUID_Des = "00002902-0000-1000-8000-00805f9b34fb";

        private readonly IBluetoothLE _bluetooth;
        private readonly IAdapter _adapter;
        private readonly IList<IDevice> _scannedDevices;

        public BluetoothLEService()
        {
            _scannedDevices = new List<IDevice>();
            _bluetooth = CrossBluetoothLE.Current;
            _bluetooth.StateChanged += OnStateChanged;
            _adapter = CrossBluetoothLE.Current.Adapter;
            _adapter.DeviceDiscovered += (s, a) =>
            {

                System.Diagnostics.Debug.WriteLine("Discovered device: " + a.Device.Name);
                _scannedDevices.Add(a.Device);
            };
        }

        private void OnStateChanged(object sender, BluetoothStateChangedArgs e)
        {
            Console.WriteLine(e.NewState.ToString());
        }

        public async Task ScanDevicesAsync()
        {
            _scannedDevices.Clear();

            await _adapter.StartScanningForDevicesAsync();
            //await _adapter.DiscoverDeviceAsync(new Guid("00001101-0000-1000-8000-00805F9B34FB"));

        }

        private async Task ValidateBluetoothPermission()
        {


        }

        public async Task<bool> ConnectDeviceAsync(IDevice device)
        {

            return true;
        }

        public void SendPacket(byte[] payload)
        {
            throw new NotImplementedException();
        }

        public async Task ConnectAsync(string deviceName)
        {

        }

        ~BluetoothLEService()
        {
            _adapter.DeviceDiscovered -= (s, a) => _scannedDevices.Add(a.Device);
            _bluetooth.StateChanged -= OnStateChanged;
        }
    }
}
