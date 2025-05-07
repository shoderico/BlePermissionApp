using BlePermissionApp.Shared.Services;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BlePermissionApp
{
    public partial class MainPage : ContentPage
    {
        private IBluetoothLE _bluetoothLE;
        private IAdapter _adapter;
        private ObservableCollection<IDevice> _devices;
        private IDevice _connectedDevice;
        private IService _gattService;
        private ICharacteristic _gattCharacteristic;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this; // Set the page as the BindingContext
            _bluetoothLE = CrossBluetoothLE.Current;
            _adapter = CrossBluetoothLE.Current.Adapter;
            _devices = new ObservableCollection<IDevice>();
            DevicesList.ItemsSource = _devices;

            // Handle discovered devices
            _adapter.DeviceDiscovered += (s, a) =>
            {
                if (a.Device.Name?.Contains("ESP_GATTS_DEMO") == true)
                {
                    MainThread.BeginInvokeOnMainThread(() => _devices.Add(a.Device));
                }
            };
        }

        // Handle scan button click
        private async void OnScanClicked(object sender, EventArgs e)
        {
            StatusLabel.Text = "Status: Scanning...";
            _devices.Clear();

            try
            {
                await _adapter.StartScanningForDevicesAsync();
                StatusLabel.Text = "Status: Scan completed";
            }
            catch (Exception ex)
            {
                StatusLabel.Text = $"Status: Scan failed - {ex.Message}";
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        // Connect to a BLE device
        private async Task ConnectToDeviceAsync(IDevice device)
        {
            if (device == null) return;

            try
            {
                StatusLabel.Text = $"Status: Connecting to {device.Name}...";
                await _adapter.ConnectToDeviceAsync(device);
                _connectedDevice = device;

                // Retrieve service and characteristic
                _gattService = await _connectedDevice.GetServiceAsync(Guid.Parse("000000FF-0000-1000-8000-00805F9B34FB"));
                _gattCharacteristic = await _gattService.GetCharacteristicAsync(Guid.Parse("0000FF01-0000-1000-8000-00805F9B34FB"));

                // Enable notifications if supported
                if (_gattCharacteristic.CanUpdate)
                {
                    _gattCharacteristic.ValueUpdated += (s, args) =>
                    {
                        var data = args.Characteristic.Value;
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            StatusLabel.Text = $"Status: Received: {BitConverter.ToString(data)}";
                        });
                    };
                    await _gattCharacteristic.StartUpdatesAsync();
                }

                StatusLabel.Text = $"Status: Connected to {device.Name}";
                SendButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                StatusLabel.Text = $"Status: Connection failed - {ex.Message}";
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        // Handle send data button click
        private async void OnSendDataClicked(object sender, EventArgs e)
        {
            if (_gattCharacteristic == null || string.IsNullOrEmpty(DataEntry.Text))
                return;

            try
            {
                var data = System.Text.Encoding.UTF8.GetBytes(DataEntry.Text);
                await _gattCharacteristic.WriteAsync(data);
                StatusLabel.Text = $"Status: Sent: {DataEntry.Text}";
            }
            catch (Exception ex)
            {
                StatusLabel.Text = $"Status: Send failed - {ex.Message}";
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private ICommand _connectCommand;
        // Command for connecting to a device
        public ICommand ConnectCommand => _connectCommand ??= new Command<IDevice>(async (device) =>
        {
            System.Diagnostics.Debug.WriteLine($"ConnectCommand called for device: {device?.Name}");
            await ConnectToDeviceAsync(device);
        });
    }
}