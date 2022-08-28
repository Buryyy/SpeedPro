using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Exceptions;
using SpeedPro.Helpers;
using System.Diagnostics;
namespace SpeedPro.Services
{
    public class BluetoothLEService : IBluetoothLEService
    {

        private readonly IVehicleCommsService _vehicleCommsService;
        private readonly IBluetoothLE _bluetooth;
        private readonly IAdapter _adapter;

        private IDevice _connectedDevice;
        private ValueTuple<ICharacteristic, ICharacteristic> _characteristicsPair;
        private System.Timers.Timer _timer;
        public event EventHandler<CharacteristicUpdatedEventArgs> CharacteristicsChanged;

        public BluetoothLEService()
        {
            _bluetooth = CrossBluetoothLE.Current;
            _adapter = CrossBluetoothLE.Current.Adapter;
            _bluetooth.StateChanged += OnConnectionStateChanged;
        }

        private void OnTimerElapsed(object state)
        {
            throw new NotImplementedException();
        }

        private void OnConnectionStateChanged(object sender, BluetoothStateChangedArgs e)
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

        }

        public async Task<bool> ConnectDeviceAsync(IDevice device)
        {
            try
            {
                await _adapter.ConnectToDeviceAsync(device);

                var services = await device.GetServicesAsync();
                ICharacteristic char__1 = null;
                ICharacteristic char__2 = null;
                foreach (var service in services)
                {
                    var characteristic__1 = await service.GetCharacteristicAsync(ScooterCommsUtil.CharactericsUUID_1);
                    var characteristic__2 = await service.GetCharacteristicAsync(ScooterCommsUtil.CharactericsUUID_2);

                    if (characteristic__1 != null)
                    {
                        char__1 = characteristic__1;

                        characteristic__1.ValueUpdated += OnCharacteristicChanged;
                        Debug.Write("Found match for characteristic__1");
                    }
                    if (characteristic__2 != null)
                    {
                        char__2 = characteristic__2;

                        //  characteristic__2.ValueUpdated += OnCharacteristicChanged;
                        Debug.Write("Found match for characteristic__2");
                    }
                }
                if (char__1 != null && char__2 != null)
                {
                    _connectedDevice = device;
                    _characteristicsPair = new ValueTuple<ICharacteristic, ICharacteristic>(char__1, char__2);
                    await WriteAsync(ScooterCommsUtil.HexStrToBytes("FF55010055"));

                    _timer.Start();
                    Debug.WriteLine("Found both characteristics");
                }
                return true;
            }
            catch (DeviceConnectionException e)
            {
                return false;
            }
        }

        public async Task<bool> WriteAsync(byte[] payload)
        {
            return await _characteristicsPair.Item2.WriteAsync(payload);
        }

        public async Task DisconnectVehicleAsync()
        {
            if (_connectedDevice != null)
            {
                await _adapter.DisconnectDeviceAsync(_connectedDevice);
                _connectedDevice = null;
            }
            _characteristicsPair = default;
        }

        private void OnCharacteristicChanged(object sender, CharacteristicUpdatedEventArgs e)
        {
            Debug.WriteLine("OnCharacteristicsChanged");
            var hexStr_2 = ScooterCommsUtil.BytesToHexStr(e.Characteristic.Value, e.Characteristic.Value.Length);
            bool isValid = ScooterCommsUtil.AnalyzedHex(hexStr_2) == Convert.ToInt32(hexStr_2.Substring(hexStr_2.Length - 2, hexStr_2.Length), 16);
            if (isValid)
            {
                Debug.WriteLine("Characteristic change is valid.");
                CharacteristicsChanged.Invoke(this, e);
            }
            string substr = hexStr_2[..6];
            int val = 65535;
            if (substr.GetHashCode() == 2070318434 || substr.Equals("FF5502"))
            {
                val = 0;
            }
            if (val == 0)
            {

            }
        }

        ~BluetoothLEService()
        {
            _bluetooth.StateChanged -= OnConnectionStateChanged;
        }

    }
}