using Android.Bluetooth;
using Android.Util;
using Java.Util;
using SpeedPro.Helpers;
using SpeedPro.Platforms.Android.Bluetooth;
using System.Buffers;

namespace SpeedPro.Services
{
    public partial class BluetoothService
    {
        private const string Name = "BluetoothChat";

        private readonly BluetoothAdapter _bluetoothAdapter;

        private BluetoothSocket _socket;
        private ConnectionState _connectionState;

        private BluetoothGattCharacteristic _gattCharacterics__char1;
        private BluetoothGattCharacteristic _gattCharacterics__char2;
        private BluetoothGatt _bluetoothGatt;

        private Task _receiverTask;
        public BluetoothService()
        {
            _bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
        }

        public void DiscoverDevice()
        {
            if (!_bluetoothAdapter.IsEnabled)
            {
                return;
            }

        }

        public void Connect(string deviceName)
        {
            Task.Run(async () =>
            {
                var device = _bluetoothAdapter.BondedDevices.FirstOrDefault(c => c.Name == deviceName);
                if (device == null)
                {
                    Log.Debug("BluetoothService", "Unable to find bonded device.");
                    return;
                }
                _socket = device.CreateRfcommSocketToServiceRecord(
                    UUID.FromString("00001101-0000-1000-8000-00805F9B34FB"));
                await _socket.ConnectAsync();

                if (_socket == null && !_socket.IsConnected)
                {
                    Log.Debug("BluetoothService", "Unable to open socket.");
                    return;
                }
                _receiverTask = Task.Run(async () => await ReadStreamAsync());
            });

        }

        private async Task ReadStreamAsync()
        {
            while (_socket != null && _socket.IsConnected)
            {
                byte[] buffer = ArrayPool<byte>.Shared.Rent(1024);

                int position = await _socket.InputStream.ReadAsync(buffer, 0, buffer.Length);

            }
        }

        public void Write(string hexStr)
        {
            byte[] payload = ScooterCommunicationUtil.HexStrToBytes(hexStr);

            _gattCharacterics__char2.SetValue(payload);

            //await _socket.OutputStream.WriteAsync(payload, 0, payload.Length);
            _bluetoothGatt.WriteCharacteristic(_gattCharacterics__char2);
        }

        public ConnectionState GetConnectionState() => _connectionState;
        public void SetConnectionState(ConnectionState state) => _connectionState = state;


        public void SendPacket(byte[] payload)
        {
            // _socket.
        }
    }
}
