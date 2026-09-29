using OtpNet;
using System.Security.Cryptography;
using System.Text;

namespace CareerFirstOtpGenerator;

public static class TotpService
{
    private static byte[] GetSecret()
    {
        var p1 = "JBS";
        var p2 = "WY3";
        var p3 = "DPE";
        var p4 = "HPK";
        var p5 = "3PX";
        var p6 = "PQR";
        var p7 = "STU";

        var combined = p1 + p2 + p3 + p4 + p5 + p6 + p7;
        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(combined));
        return hash[..20];
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