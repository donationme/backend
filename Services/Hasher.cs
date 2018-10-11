using System.Text;
using System.Security.Cryptography;
namespace SADJZ.Services
{

    public class Hasher
    {
        public static string SHA256(string src)
        {
            var crypt = new SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(src));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
    }


}