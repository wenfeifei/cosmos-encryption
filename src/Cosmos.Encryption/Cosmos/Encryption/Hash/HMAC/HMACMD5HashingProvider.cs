﻿using System.Security.Cryptography;
using System.Text;
using Cosmos.Encryption.Core;

// ReSharper disable once CheckNamespace
namespace Cosmos.Encryption
{
    /// <summary>
    /// Hash/HMACMD5 hashing provider.
    /// Reference: Seay Xu
    ///     https://github.com/godsharp/GodSharp.Encryption/blob/master/src/GodSharp.Shared/Encryption/Hash/HMACHashingBase/HMACMD5.cs
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public sealed class HMACMD5HashingProvider : HMACHashingBase
    {
        // ReSharper disable once IdentifierTypo
        private HMACMD5HashingProvider() { }

        /// <summary>
        /// HMACMD5 encryption.
        /// </summary>
        /// <param name="data">The string to be encrypted,not null.</param>
        /// <param name="key">Encryption key,not null.</param>
        /// <param name="encoding">The <see cref="T:System.Text.Encoding"/>,default is Encoding.UTF8.</param>
        /// <returns>The encrypted string.</returns>
        public static string Signature(string data, string key, Encoding encoding = null)
            => Encrypt<HMACMD5>(data, key, encoding);

        /// <summary>
        /// Verify 
        /// </summary>
        /// <param name="comparison"></param>
        /// <param name="data">The string to be encrypted,not null.</param>
        /// <param name="key">Encryption key,not null.</param>
        /// <param name="encoding">The <see cref="T:System.Text.Encoding"/>,default is Encoding.UTF8.</param>
        /// <returns></returns>
        public static bool Verify(string comparison, string data, string key, Encoding encoding = null)
            => comparison == Signature(data, key, encoding);
    }
}