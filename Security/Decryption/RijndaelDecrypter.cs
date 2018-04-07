﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Journal.Security.Decryption
{
    public class RijndaelDecrypter:IDecrypter
    {

        public string Decrypt(string encryptedContent,string password)
        {
            var buffer = Convert.FromBase64String(encryptedContent);
            string decrypted;
            var transform = GetDecryptor(password);
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                {
                    cs.Write(buffer, 0, buffer.Length);

                    cs.FlushFinalBlock();
                    cs.Clear();
                    cs.Close();
                    decrypted = Encoding.UTF8.GetString(ms.ToArray());
                    ms.Close();
                }
            }
            return decrypted;
        }

        private ICryptoTransform GetDecryptor(string password)
        {
            var rijndael = new RijndaelManaged {Key = KeyProvider.GetKey(password), IV = KeyProvider.GetIV(password)};
           return rijndael.CreateDecryptor();
        }
    }
}
