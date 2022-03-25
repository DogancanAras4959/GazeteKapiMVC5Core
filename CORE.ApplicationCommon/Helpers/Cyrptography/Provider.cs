using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CORE.ApplicationCommon.Helpers.Cyrptography
{
    public class Provider
    {
        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

        public Provider(string key)
        {
            tdes.Key = TruncateHash(key, tdes.KeySize / 8);
            tdes.IV = TruncateHash("", tdes.BlockSize / 8);
        }

        private byte[] TruncateHash(string key, int length)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] keybytes = Encoding.Unicode.GetBytes(key);
            byte[] hash = sha1.ComputeHash(keybytes);
            Array.Resize(ref hash, length);

            return hash;

        }

        public string Encrypt(string text)
        {
            byte[] plaintTextBytes = Encoding.Unicode.GetBytes(text);
            MemoryStream ms = new MemoryStream();
            CryptoStream encStream = new CryptoStream(ms, tdes.CreateEncryptor(), CryptoStreamMode.Write);
            encStream.Write(plaintTextBytes, 0, plaintTextBytes.Length);
            encStream.FlushFinalBlock();

            return Convert.ToBase64String(ms.ToArray());
        }

        public string Decrypt(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
            {
                throw new Exception("Çözümlenecek metin bulunamadı");
            }
            try
            {
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                MemoryStream ms = new MemoryStream();
                CryptoStream decStream = new CryptoStream(ms, tdes.CreateDecryptor(), CryptoStreamMode.Write);
                decStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                decStream.FlushFinalBlock();

                return Encoding.Unicode.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {

                throw new Exception("Şifrelenmiş veri okunurken hata oluştu" + ex.Message);
            }
        }

        public static string GetHash(string input)
        {
            HashAlgorithm ha = new SHA256CryptoServiceProvider();
            byte[] byteValue = Encoding.UTF8.GetBytes(input);
            byte[] byteHash = ha.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }
    }
}
