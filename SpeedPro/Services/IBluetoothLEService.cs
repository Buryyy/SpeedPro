using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;

namespace SpeedPro.Services
{
    public interface IBluetoothLEService
    {
        event EventHandler<CharacteristicUpdatedEventArgs> CharacteristicsChanged;

        Task<bool> ConnectDeviceAsync(IDevice device);
        Task DisconnectVehicleAsync();
        Task ScanDevicesAsync();
        Task<bool> WriteAsync(byte[] payload);
    }
}
