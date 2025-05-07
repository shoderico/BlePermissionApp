using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlePermissionApp.Shared.Services;
using Plugin.BLE.Abstractions.Contracts;

namespace BlePermissionApp.Platforms
{
    internal class PlatformService : PlatformServiceBase
    {
        public override async Task<bool> CheckBluetoothPermissionAsync(Page page)
        {
            try
            {
                if (BluetoothLE.State != BluetoothState.On)
                {
                    // Permissions.CheckStatusAsync<Permissions.Bluetooth>() always returns PermissionStatus.Granted.
                    Console.WriteLine("Bluetooth is off, or Bluetooth permission is denied for this app");
                    if (await page.DisplayAlert(
                        "Cannot use Bluetooth",
                        "Bluetooth is either turned off or the app's Bluetooth permission is not granted. Please turn on Bluetooth and grant Bluetooth permission from the app's settings.",
                        "Go to app settings",
                        "Cancel"
                    ))
                    {
                        AppInfo.ShowSettingsUI();
                    }
                    return false;
                }

                var permissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (permissionStatus != PermissionStatus.Granted)
                {
                    permissionStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    if (permissionStatus != PermissionStatus.Granted)
                    {
                        if (await page.DisplayAlert(
                            "Location permission required",
                            "Location permission is required to use Bluetooth functionality. Please allow it from Settings.",
                            "Go to Settings",
                            "Cancel"
                        ))
                        {
                            AppInfo.ShowSettingsUI();
                        }

                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                await page.DisplayAlert("Error", $"An error occurred while checking Bluetooth permission: {ex.Message}", "OK");
                return false;
            }

            return true;
        }
    }
}
