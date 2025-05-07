using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlePermissionApp.Shared.Services
{
    public class AppServices
    {
        public IPlatformService PlatformService { get; }

        public AppServices(IPlatformService platformService)
        {
            PlatformService = platformService;
        }
    }
}
