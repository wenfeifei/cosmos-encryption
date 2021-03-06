/*
 * Reference to:
 *     https://github.com/toolgood/RCX/blob/master/ToolGood.RcxTest/ToolGood.RcxCrypto/RCX.cs
 *     Author: ToolGood
 *     GitHub: https://github.com/toolgood
 */

using System;
using System.Text;
using Cosmos.Encryption.Abstractions;
using Cosmos.Optionals;

// ReSharper disable once CheckNamespace
namespace Cosmos.Encryption
{
    /// <summary>
    /// Symmetric/RCX encryption.
    /// Reference: https://github.com/toolgood/RCX/
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public sealed class RCXEncryptionProvider : ISymmetricEncryption
    {
        // ReSharper disable once InconsistentNaming
        private const int KEY_LENGTH = 256;

        private RCXEncryptionProvider() { }

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="encoding"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string Encrypt(string data, string key, Encoding encoding = null, RCXOrder order = RCXOrder.ASC)
        {
            encoding = encoding.SafeValue();
            return Convert.ToBase64String(EncryptCore(encoding.GetBytes(data), encoding.GetBytes(key), order));
        }

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="encoding"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string Encrypt(byte[] data, string key, Encoding encoding = null, RCXOrder order = RCXOrder.ASC)
        {
            encoding = encoding.SafeValue();
            return Convert.ToBase64String(EncryptCore(data, encoding.GetBytes(key), order));
        }

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] data, byte[] key, RCXOrder order = RCXOrder.ASC)
        {
            return EncryptCore(data, key, order);
        }

        /// <summary>
        /// Decrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="encoding"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string Decrypt(string data, string key, Encoding encoding = null, RCXOrder order = RCXOrder.ASC)
        {
            encoding = encoding.SafeValue();
            return encoding.GetString(EncryptCore(Convert.FromBase64String(data), encoding.GetBytes(key), order));
        }

        /// <summary>
        /// Decrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] data, byte[] key, RCXOrder order = RCXOrder.ASC)
        {
            return EncryptCore(data, key, order);
        }

        private static unsafe byte[] EncryptCore(byte[] data, byte[] pass, RCXOrder order)
        {
            byte[] mBox = GetKey(pass, KEY_LENGTH);
            byte[] output = new byte[data.Length];
            //int i = 0, j = 0;

            if (order == RCXOrder.ASC)
            {
                fixed (byte* _mBox = &mBox[0])
                    fixed (byte* _data = &data[0])
                        fixed (byte* _output = &output[0])
                        {
                            var length = data.Length;
                            int i = 0, j = 0;
                            for (Int64 offset = 0; offset < length; offset++)
                            {
                                i = (++i) & 0xFF;
                                j = (j + *(_mBox + i)) & 0xFF;

                                byte a = *(_data + offset);
                                byte c = (byte) (a ^ *(_mBox + ((*(_mBox + i) + *(_mBox + j)) & 0xFF)));
                                *(_output + offset) = c;

                                byte temp = *(_mBox + a);
                                *(_mBox + a) = *(_mBox + c);
                                *(_mBox + c) = temp;
                                j = (j + a + c);
                            }
                        }
            }
            else
            {
                fixed (byte* _mBox = &mBox[0])
                    fixed (byte* _data = &data[0])
                        fixed (byte* _output = &output[0])
                        {
                            var length = data.Length;
                            int i = 0, j = 0;
                            for (int offset = data.Length - 1; offset >= 0; offset--)
                            {
                                i = (++i) & 0xFF;
                                j = (j + *(_mBox + i)) & 0xFF;

                                byte a = *(_data + offset);
                                byte c = (byte) (a ^ *(_mBox + ((*(_mBox + i) + *(_mBox + j)) & 0xFF)));
                                *(_output + offset) = c;

                                byte temp = *(_mBox + a);
                                *(_mBox + a) = *(_mBox + c);
                                *(_mBox + c) = temp;
                                j = (j + a + c);
                            }
                        }
            }

            // byte[] mBox = GetKey(pass, KEY_LENGTH);
            // byte[] output = new byte[data.Length];
            // int i = 0, j = 0;
            //
            // if (order == RCXOrder.ASC) {
            //     for (int offset = 0; offset < data.Length; offset++) {
            //         i = (++i) & 0xFF;
            //         j = (j + mBox[i]) & 0xFF;
            //
            //         byte a = data[offset];
            //         byte c = (byte) (a ^ mBox[(mBox[i] + mBox[j]) & 0xFF]);
            //         output[offset] = c;
            //
            //         byte temp2 = mBox[c];
            //         mBox[c] = mBox[a];
            //         mBox[a] = temp2;
            //         j = (j + a + c);
            //     }
            // } else {
            //     for (int offset = data.Length - 1; offset >= 0; offset--) {
            //         i = (++i) & 0xFF;
            //         j = (j + mBox[i]) & 0xFF;
            //
            //         byte a = data[offset];
            //         byte c = (byte) (a ^ mBox[(mBox[i] + mBox[j]) & 0xFF]);
            //         output[offset] = c;
            //
            //         byte temp2 = mBox[c];
            //         mBox[c] = mBox[a];
            //         mBox[a] = temp2;
            //         j = (j + a + c);
            //     }
            // }

            return output;
        }

        private static unsafe byte[] GetKey(byte[] pass, int kLen)
        {
            byte[] mBox = new byte[kLen];
            fixed (byte* _mBox = &mBox[0])
            {
                for (long i = 0; i < kLen; i++)
                {
                    *(_mBox + i) = (byte) i;
                }

                long j = 0;
                var length = pass.Length;
                fixed (byte* _pass = &pass[0])
                {
                    for (long i = 0; i < kLen; i++)
                    {
                        j = (j + *(_mBox + i) + *(_pass + (i % length))) % kLen;
                        byte temp = *(_mBox + i);
                        *(_mBox + i) = *(_mBox + j);
                        *(_mBox + j) = temp;
                    }
                }
            }

            //for (Int64 i = 0; i < kLen; i++) {
            //    mBox[i] = (byte)i;
            //}
            //Int64 j = 0;
            //for (Int64 i = 0; i < kLen; i++) {
            //    j = (j + mBox[i] + pass[i % pass.Length]) % kLen;
            //    byte temp = mBox[i];
            //    mBox[i] = mBox[j];
            //    mBox[j] = temp;
            //}
            return mBox;
        }
    }
}