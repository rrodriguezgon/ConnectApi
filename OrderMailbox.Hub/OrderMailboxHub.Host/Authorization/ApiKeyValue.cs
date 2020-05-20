using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OrderMailboxHub.Host.Authorization
{
    /// <summary>
    /// Api Key:
    /// "{ObjectIdentifier}:{CountryId}"
    /// </summary>
    public class ApiKeyValue : IApiKeyValue
    {
        public string EncryptionKey { get; set; }

        public string ObjectIdentifier { get; set; }

        public string CountryId { get; set; }

        public ApiKeyValue(string apiKey, string encryptionKey)
        {
            EncryptionKey = encryptionKey;
            loadValues(decrypt(apiKey, encryptionKey));
        }

        public ApiKeyValue(string objectIdentifier, string countryId, string encryptionKey)
        {
            ObjectIdentifier = objectIdentifier;
            CountryId = countryId;
            EncryptionKey = encryptionKey;
        }

        private void loadValues(string key)
        {
            // "{ObjectIdentifier}:{CountryId}"
            var keyArray = key.Split(':');
            if (keyArray.Length == 2)
            {
                ObjectIdentifier = keyArray[0];
                CountryId = keyArray[1];
            }
            else
            {
                throw new ArgumentException($"Api Key lenght = '{keyArray.Length}' is invalid, must be equal to '2'.");
            }
        }

        public string ToEncryptString()
        {
            return encrypt(this.ToString(), EncryptionKey);
        }

        public override string ToString()
        {
            return $"{ObjectIdentifier}:{CountryId}";
        }

        private string decrypt(string cipherText, string key)
        {
            var data = Convert.FromBase64String(cipherText);

            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;

                using (var cryptoTransform = aes.CreateDecryptor())
                {
                    return Encoding.UTF8.GetString(cryptoTransform.TransformFinalBlock(data, 0, data.Length));
                }
            }
        }

        private string encrypt(string text, string keyString)
        {
            var data = Encoding.UTF8.GetBytes(text);
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.ECB;
                aes.Key = key;
                aes.Padding = PaddingMode.PKCS7;
                aes.GenerateIV();

                using (var cryptoTransform = aes.CreateEncryptor())
                {
                    return Convert.ToBase64String(cryptoTransform.TransformFinalBlock(data, 0, data.Length));
                }
            }
        }
    }
}
