using Plugin.BLE.Abstractions.Contracts;

namespace SpeedPro.Models
{
    public record BluetoothLEDevice(string DisplayName, IDevice Device);
}
