using Microsoft.Extensions.Logging;

namespace CareerFirstOtpGenerator
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
                    fonts.AddFont("Cairo-Regular.ttf", "CairoRegular");
                    fonts.AddFont("Cairo-Bold.ttf", "CairoBold");
                    fonts.AddFont("Cairo-SemiBold.ttf", "CairoSemiBold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}