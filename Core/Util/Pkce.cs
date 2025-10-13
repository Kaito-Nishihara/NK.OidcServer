using System.Security.Cryptography;
namespace Core.Util;

public static class Pkce
{
    public static string HashS256(string verifier)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(System.Text.Encoding.ASCII.GetBytes(verifier));
        return Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }
}