using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Fluent.Infrastructure.Utilities
{
    public class DesCrypto
    {
        private readonly byte[] _byIV;
        private readonly byte[] _byKey;
        /// <summary>
        /// DES对称加密
        /// </summary>
        /// <param name="key">key 且必须为8字节</param>
        /// <param name="iv">密钥，且必须为8字节</param>
        public DesCrypto(string key, string iv)
        {
            _byKey = System.Text.Encoding.ASCII.GetBytes(key);
            _byIV = System.Text.Encoding.ASCII.GetBytes(iv);

        }
        public string Encryptor(string content)
        {
            using (DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider())
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(content);
                int i = cryptoProvider.KeySize;
                using (MemoryStream ms = new MemoryStream())
                {

                    using (CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(_byKey, _byIV), CryptoStreamMode.Write))
                    {
                        cst.Write(data, 0, data.Length);
                        cst.FlushFinalBlock();
                        return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
                    }

                }
            }
        }
        public string Decryptor(string crypto)
        {
            using (DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider())
            {
                byte[] byCrypto = Convert.FromBase64String(crypto);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(_byKey, _byIV), CryptoStreamMode.Write))
                    {
                        cst.Write(byCrypto, 0, byCrypto.Length);
                        cst.FlushFinalBlock();
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
        }
    }
}
