﻿using System;
using System.IO;
using System.Text;
using Cosmos.Optionals;
using Cosmos.Validations.Abstractions;
using Cosmos.Validations.Core;

namespace Cosmos.Validations
{
    /// <summary>
    /// CRC32
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public sealed class CRC32 : ICRC<CRC32, uint, int>
    {
        /// <summary>
        /// Value
        /// </summary>
        public uint Value { get; set; } = CRC32CheckingProvider.Seed;

        // ReSharper disable once InconsistentNaming
        private uint[] CRCTable { get; } = CRCTableGenerator.GenerationCRC32Table();

        /// <summary>
        /// Reset
        /// </summary>
        /// <returns></returns>
        public CRC32 Reset()
        {
            Value = CRC32CheckingProvider.Seed;
            return this;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public CRC32 Update(int value)
        {
            Value = CRCTable[(Value ^ value) & 0xFF] ^ (Value >> 8);
            return this;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public CRC32 Update(string value, Encoding encoding = null)
        {
            return Update(
                string.IsNullOrWhiteSpace(value)
                    ? CRCTableGenerator.EmptyBytes()
                    : encoding.SafeValue().GetBytes(value));
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public CRC32 Update(byte[] buffer, int offset = 0, long count = -1)
        {
            Checker.Buffer(buffer);

            if (count <= 0 || count > buffer.Length)
            {
                count = buffer.Length;
            }

            if (offset < 0 || offset + count > buffer.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            while (--count >= 0)
            {
                Value = CRCTable[(Value ^ buffer[offset++]) & 0xFF] ^ (Value >> 8);
            }

            return this;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public CRC32 Update(Stream stream, long count = -1)
        {
            Checker.Stream(stream);

            if (count <= 0)
            {
                count = long.MaxValue;
            }

            while (--count >= 0)
            {
                var b = stream.ReadByte();
                if (b == -1) break;

                Value = CRCTable[(Value ^ b) & 0xFF] ^ (Value >> 8);
            }

            return this;
        }
    }
}