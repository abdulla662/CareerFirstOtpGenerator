using OtpNet;

namespace CareerFirstOtpGenerator;

public static class TotpService
{
    private static byte[] GetSecret()
    {
        var s1 = "NPUAV7ZO";
        var s2 = "EPJOWOHA";
        var s3 = "OQMU4KBJ";
        var s4 = "AURV7LOH";
        return Base32Encoding.ToBytes(s1 + s2 + s3 + s4);
    }

    public static string GetCurrentCode()
    {
        var totp = new Totp(GetSecret(), step: 30);
        return totp.ComputeTotp();
    }

    public static int GetRemainingSeconds()
    {
        return 30 - (int)(DateTimeOffset.UtcNow.ToUnixTimeSeconds() % 30);
    }
}