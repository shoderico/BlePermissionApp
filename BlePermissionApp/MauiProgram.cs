using BlePermissionApp.Platforms;
using BlePermissionApp.Shared.Services;
using Microsoft.Extensions.Logging;

namespace BlePermissionApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            // Register services
            builder.Services.AddSingleton<IPlatformService, PlatformService>();
            builder.Services.AddSingleton<AppServices>();
            builder.Services.AddSingleton<MainPage>();

            return builder.Build();
        }
    }
}
