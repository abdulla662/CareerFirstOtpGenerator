using Microsoft.Extensions.Logging;
using System.Globalization;

namespace CareerFirstOtpGenerator
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            // Set Arabic
            var culture = new CultureInfo("ar");
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

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

            return builder.Build();
        }
    }
}