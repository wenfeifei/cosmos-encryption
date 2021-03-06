using System;
using System.Security.Cryptography;
using System.Text;
using Cosmos.Encryption.Core.Internals.Extensions;
using Cosmos.Optionals;

/*
 * Reference to:
 *     https://github.com/stulzq/RSAUtil/blob/master/XC.RSAUtil/RsaXmlUtil.cs
 *     Author:Zhiqiang Li
 */

namespace Cosmos.Encryption.Core
{
    /// <summary>
    /// RSA Xml util
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class RSAXmlUtil : RSABase
    {
        /// <summary>
        /// RSA encryption
        /// SHA256 hash algorithm to use the key length of at least 2048
        /// </summary>
        /// <param name="keySize">Key length in bits:</param>
        /// <param name="privateKey">private Key</param>
        /// <param name="publicKey">public Key</param>
        public RSAXmlUtil(string publicKey, string privateKey = null, int keySize = 2048)
            : this(Encoding.UTF8, publicKey, privateKey, keySize) { }

        /// <summary>
        /// RSA encryption
        /// SHA256 hash algorithm to use the key length of at least 2048
        /// </summary>
        /// <param name="encoding">Data coding</param>
        /// <param name="keySize">Key length in bits:</param>
        /// <param name="privateKey">private Key</param>
        /// <param name="publicKey">public Key</param>
        public RSAXmlUtil(Encoding encoding, string publicKey, string privateKey = null, int keySize = 2048)
        {
            if (string.IsNullOrEmpty(privateKey) && string.IsNullOrEmpty(publicKey))
            {
                throw new ArgumentException("Public and private keys must not be empty at the same time");
            }

            if (!string.IsNullOrEmpty(privateKey))
            {
#if NET451
                PrivateRsa = new RSACryptoServiceProvider {KeySize = keySize};
#else
                PrivateRsa = RSA.Create();
                PrivateRsa.KeySize = keySize;
#endif
                PrivateRsa.FromLvccXmlString(privateKey);
            }

            if (!string.IsNullOrEmpty(publicKey))
            {
#if NET451
                PublicRsa = new RSACryptoServiceProvider {KeySize = keySize};
#else
                PublicRsa = RSA.Create();
                PublicRsa.KeySize = keySize;
#endif
                PublicRsa.FromLvccXmlString(publicKey);
            }

            DataEncoding = encoding.SafeValue();
        }
    }
}