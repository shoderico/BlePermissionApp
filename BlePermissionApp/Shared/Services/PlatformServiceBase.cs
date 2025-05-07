using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;

namespace BlePermissionApp.Shared.Services
{
    public abstract class PlatformServiceBase : IPlatformService
    {
        public IBluetoothLE BluetoothLE { get; }
        public PlatformServiceBase()
        {
            BluetoothLE = CrossBluetoothLE.Current;

        }
        public virtual Task<bool> CheckBluetoothPermissionAsync(Page page)
        {
            return new Task<bool>(()=> { return false; });
        }
    }
}
