using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace Fleet.Helpers;

[ExcludeFromCodeCoverage]
public class PasswordGeneratorHelper
{
     public static string GenerateRandomPassword(int length = 8)
    {
        const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        StringBuilder result = new(length);
        byte[] randomBytes = new byte[length];

        // Gera números aleatórios seguros para a senha
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        // Mapeia os bytes aleatórios para caracteres válidos
        foreach (byte b in randomBytes)
        {
            result.Append(validChars[b % validChars.Length]);
        }

        return result.ToString();
    }
}
