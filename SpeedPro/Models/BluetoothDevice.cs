using Plugin.BLE.Abstractions.Contracts;

namespace SpeedPro.Models
{
    public record BluetoothDevice(string DisplayName, IDevice Device);
}
