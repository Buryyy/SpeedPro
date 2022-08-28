using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using SpeedPro.Helpers;
using System.Diagnostics;
using Timer = System.Timers.Timer;

namespace SpeedPro.Services
{
    public class VehicleCommsService : IVehicleCommsService
    {
        private readonly IBluetoothLEService _bluetoothLEService;

        private Timer _connectionRefreshTimer;
        public VehicleCommsService(IBluetoothLEService bluetoothLEService)
        {
            _bluetoothLEService = bluetoothLEService;
            _bluetoothLEService.CharacteristicsChanged += OnCharacteristicsChanged;
            _connectionRefreshTimer.Elapsed += async (s, e) =>
            {
                _connectionRefreshTimer.Stop();
                await _bluetoothLEService.WriteAsync(ScooterCommsUtil.HexStrToBytes(ScooterCommsUtil.ConnectionNotify));
                _connectionRefreshTimer.Start();
            };
        }

        public async Task<bool> ConnectVehicleAsync(IDevice vehicle)
        {
            bool isConnected = await _bluetoothLEService.ConnectDeviceAsync(vehicle);
            if (!isConnected) return false;

            _connectionRefreshTimer.Start();
            return true;
        }

        public async Task DisconnectVehicleAsync()
        {
            await _bluetoothLEService.DisconnectVehicleAsync();
        }

        private void OnCharacteristicsChanged(object sender, CharacteristicUpdatedEventArgs e)
        {
            string str = ScooterCommsUtil.BytesToHexStr(e.Characteristic.Value, e.Characteristic.Value.Length);
            if (str.Length >= 6)
            {
                int c = 0;
                string substring = str[..6];
                if (substring.GetHashCode() != 2070318464 || !substring.Equals("FF5511"))
                {
                    c = 65535;
                }
                if (c == 0)
                {
                    var substring2 = str[8..^(-6)];
                    if (!substring2.Contains("FF55"))
                    {
                        if (substring.Equals("00"))
                        {
                            substring2 = "0";
                        }
                        var r = Convert.ToInt64(substring2, 16);
                        Debug.WriteLine("FF5511: " + r);
                    }
                }
            }
        }


    }
}
