using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Layouts;

namespace BlePermissionApp.Shared.Services
{
    public interface IPlatformService
    {
        Task<bool> CheckBluetoothPermissionAsync(Page page);
    }
}
