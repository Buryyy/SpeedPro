using Plugin.BLE.Abstractions.Contracts;

namespace SpeedPro.Services
{
    public interface IBluetoothLEService
    {
        Task<bool> ConnectDeviceAsync(IDevice device);
        Task ScanDevicesAsync();
    }
}
