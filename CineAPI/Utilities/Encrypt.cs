using System.Security.Cryptography;
using System.Text;

namespace CineAPI.Utilities;

public class Encrypt
{
    public static string GetSHA256(string str)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.ASCII.GetBytes(str);
            byte[] hash = sha256.ComputeHash(bytes);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
