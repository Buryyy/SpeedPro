using Plugin.BLE.Abstractions.Contracts;

namespace SpeedPro.Services
{
    public interface IVehicleCommsService
    {
        Task<bool> ConnectVehicleAsync(IDevice vehicle);
    }
}
