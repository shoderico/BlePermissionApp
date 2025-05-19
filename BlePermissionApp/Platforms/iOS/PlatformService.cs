using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlePermissionApp.Shared.Services;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;

namespace BlePermissionApp.Platforms
{
    internal class PlatformService : PlatformServiceBase
    {
        private IBluetoothLE _ble;

        public PlatformService()
        {
            _ble = CrossBluetoothLE.Current;
            _ble.StateChanged += OnStateChanged;
        }

        private async void OnStateChanged(object? sender, Plugin.BLE.Abstractions.EventArgs.BluetoothStateChangedArgs e)
        {
            Page currentPage = Shell.Current;

            try
            {
                // On iOS, IBluetoothLE.State returns BluetoothState.Unknown until IAdapter executes the first scan.
                // Therefore, checking IBluetoothLE.State in the CheckBluetoothPermissionAsync does not work.
                // So, we catch the StateChanged event and show the message.
                switch (e.NewState)
                {
                    case BluetoothState.Unavailable:
                    case BluetoothState.Unauthorized:
                    case BluetoothState.Off:
                        Console.WriteLine("Bluetooth is off, or Bluetooth permission is denied for this app");
                        if (await currentPage.DisplayAlert(
                            "Cannot use Bluetooth",
                            "Bluetooth is either turned off or the app's Bluetooth permission is not granted. Please turn on Bluetooth and grant Bluetooth permission from the app's settings." + $" State: {_ble.State}",
                            "Go to app settings",
                            "Cancel"
                        ))
                        {
                            AppInfo.ShowSettingsUI();
                        }
                        return;
                }
            }
            catch (Exception ex)
            {
                await currentPage.DisplayAlert("Error", $"An error occurred while checking Bluetooth permission: {ex.Message}", "OK");
                return;
            }
        }


        public override Task<bool> CheckBluetoothPermissionAsync(Page page)
        {
            // iOS does not need the Permission of Location Service to use Bluetooth.

            return Task.FromResult(true);
        }
    }
}
