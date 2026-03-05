using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;
using System.Runtime.Versioning; // 1. Bu kütüphaneyi ekledik

namespace Tufanlar.UI
{
    public static class MauiProgram
    {
        // 2. Bu 3 satır, Visual Studio'ya "Ben bu kodun her yerde çalışacağını biliyorum, sus" der.
        [SupportedOSPlatform("android")]
        [SupportedOSPlatform("ios")]
        [SupportedOSPlatform("windows")]
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseBarcodeReader()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}