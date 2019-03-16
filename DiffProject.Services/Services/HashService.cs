using DiffProject.Services.Interfaces;
using System.Text;

namespace DiffProject.Services.Services
{
    public class HashService : IHashService
    {
        public string CreateHash(string input)
        {
            //using MD5 because is fast and it´s not a securty issue in the scope
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
