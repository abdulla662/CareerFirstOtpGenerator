using System.Globalization;
using System.Resources;

namespace CareerFirstOtpGenerator;

public static class LocalizationManager
{
    private static readonly ResourceManager ResourceManager =
        new ResourceManager("CareerFirstOtpGenerator.Localization.AppResources",
            typeof(LocalizationManager).Assembly);

    public static string Get(string key)
    {
        return ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? key;
    }
}