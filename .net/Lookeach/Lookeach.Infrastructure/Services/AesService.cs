using Lookeach.Application.Services;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace Lookeach.Infrastructure.Services
{
    public class AesService: IAesService
    {
        private readonly IConfiguration _configuration;

        public AesService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public byte[] Encrypt(string plainText)
        {
            byte[] Key = Convert.FromBase64String(_configuration.GetSection("key").Value!);
            byte[] IV = Convert.FromBase64String(_configuration.GetSection("iv").Value!);
            byte[] encrypted;
            using (AesManaged aes = new AesManaged())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }
            return encrypted;
        }


        public string Decrypt(byte[] cipherText)
        {
            byte[] Key = Convert.FromBase64String(_configuration.GetSection("key").Value!);
            byte[] IV = Convert.FromBase64String(_configuration.GetSection("iv").Value!);
            string plaintext = null;
            using (AesManaged aes = new AesManaged())
            {
                ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }
    }

}
