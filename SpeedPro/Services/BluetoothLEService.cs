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
        private readonly string[] PossibleUUIDs = new string[]{ "00008888-0000-1000-8000-00805f9b34fb",
        "00008877-0000-1000-8000-00805f9b34fb",
        "00002902-0000-1000-8000-00805f9b34fb"};

        private readonly IBluetoothLE _bluetooth;
        private readonly IAdapter _adapter;

        private IDevice _connectedDevice;

        private ValueTuple<ICharacteristic, ICharacteristic> _characteristicsPair;

        public BluetoothLEService()
        {
            _bluetooth = CrossBluetoothLE.Current;
            _adapter = CrossBluetoothLE.Current.Adapter;
            _bluetooth.StateChanged += OnConnectionStateChanged;
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
        /**
         *   public void initData() {
        versionsModel();
        this.baseOnBluetoothListener = new BaseOnBluetoothListener() {
            public void onCharacteristicChanged(BluetoothGatt bluetoothGatt, BluetoothGattCharacteristic bluetoothGattCharacteristic) {
                BasicInformationActivity.this.analysisBluetooth(CodeFormat.bytesToHexStringTwo(bluetoothGattCharacteristic.getValue(), bluetoothGattCharacteristic.getValue().length));
            }
        };
        QBlueToothManager.getInstance().addBluetoothListener(this.baseOnBluetoothListener);
    }
*/
        public async Task<bool> ConnectDeviceAsync(IDevice device)
        {
            try
            {
                await _adapter.ConnectToDeviceAsync(device);

                var services = await device.GetServicesAsync();
                ICharacteristic char__1 = null;
                ICharacteristic char__2 = null;
                var char1_guid = Guid.Parse("00008888-0000-1000-8000-00805f9b34fb");
                var char2_guid = Guid.Parse("00008877-0000-1000-8000-00805f9b34fb");
                foreach (var service in services)
                {
                    var characteristic__1 = await service.GetCharacteristicAsync(char1_guid);
                    var characteristic__2 = await service.GetCharacteristicAsync(char2_guid);

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
                    /*  await Task.Run(async () =>
                        {
                            while (true)
                            {
                                int startMs = DateTime.UtcNow.Millisecond;
                                var payload = ScooterCommunicationUtil.HexStrToBytes("FF55160200006C");
                                var didWrite = await WriteAsync(payload);
                                if (didWrite)
                                {
                                    Debug.WriteLine("Write payload to scooter successfully");
                                }
                                int elapsedMs = DateTime.UtcNow.Millisecond;
                                await Task.Delay(600);
                            }
                        });*/
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
            var hexStr_2 = ScooterCommunicationUtil.BytesToHexStr(e.Characteristic.Value, e.Characteristic.Value.Length);
            bool isValid = ScooterCommunicationUtil.AnalyzedHex(hexStr_2) == Convert.ToInt32(hexStr_2.Substring(hexStr_2.Length - 2, hexStr_2.Length), 16);
            if (isValid)
            {
                Debug.WriteLine("Characteristic change is valid.");
            }
        }

        ~BluetoothLEService()
        {
            _bluetooth.StateChanged -= OnConnectionStateChanged;
        }

    }
}