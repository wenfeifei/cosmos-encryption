﻿using System.Collections.Generic;

namespace Cosmos.Validations.Core
{
    // ReSharper disable once InconsistentNaming
    internal static class CRCTableGenerator
    {
        #region CRC16

        // ReSharper disable once InconsistentNaming
        private static readonly ushort[] CRC16Table =
        {
            0x0000, 0x1021, 0x2042, 0x3063, 0x4084, 0x50A5, 0x60C6, 0x70E7,
            0x8108, 0x9129, 0xA14A, 0xB16B, 0xC18C, 0xD1AD, 0xE1CE, 0xF1EF,
            0x1231, 0x0210, 0x3273, 0x2252, 0x52B5, 0x4294, 0x72F7, 0x62D6,
            0x9339, 0x8318, 0xB37B, 0xA35A, 0xD3BD, 0xC39C, 0xF3FF, 0xE3DE,
            0x2462, 0x3443, 0x0420, 0x1401, 0x64E6, 0x74C7, 0x44A4, 0x5485,
            0xA56A, 0xB54B, 0x8528, 0x9509, 0xE5EE, 0xF5CF, 0xC5AC, 0xD58D,
            0x3653, 0x2672, 0x1611, 0x0630, 0x76D7, 0x66F6, 0x5695, 0x46B4,
            0xB75B, 0xA77A, 0x9719, 0x8738, 0xF7DF, 0xE7FE, 0xD79D, 0xC7BC,
            0x48C4, 0x58E5, 0x6886, 0x78A7, 0x0840, 0x1861, 0x2802, 0x3823,
            0xC9CC, 0xD9ED, 0xE98E, 0xF9AF, 0x8948, 0x9969, 0xA90A, 0xB92B,
            0x5AF5, 0x4AD4, 0x7AB7, 0x6A96, 0x1A71, 0x0A50, 0x3A33, 0x2A12,
            0xDBFD, 0xCBDC, 0xFBBF, 0xEB9E, 0x9B79, 0x8B58, 0xBB3B, 0xAB1A,
            0x6CA6, 0x7C87, 0x4CE4, 0x5CC5, 0x2C22, 0x3C03, 0x0C60, 0x1C41,
            0xEDAE, 0xFD8F, 0xCDEC, 0xDDCD, 0xAD2A, 0xBD0B, 0x8D68, 0x9D49,
            0x7E97, 0x6EB6, 0x5ED5, 0x4EF4, 0x3E13, 0x2E32, 0x1E51, 0x0E70,
            0xFF9F, 0xEFBE, 0xDFDD, 0xCFFC, 0xBF1B, 0xAF3A, 0x9F59, 0x8F78,
            0x9188, 0x81A9, 0xB1CA, 0xA1EB, 0xD10C, 0xC12D, 0xF14E, 0xE16F,
            0x1080, 0x00A1, 0x30C2, 0x20E3, 0x5004, 0x4025, 0x7046, 0x6067,
            0x83B9, 0x9398, 0xA3FB, 0xB3DA, 0xC33D, 0xD31C, 0xE37F, 0xF35E,
            0x02B1, 0x1290, 0x22F3, 0x32D2, 0x4235, 0x5214, 0x6277, 0x7256,
            0xB5EA, 0xA5CB, 0x95A8, 0x8589, 0xF56E, 0xE54F, 0xD52C, 0xC50D,
            0x34E2, 0x24C3, 0x14A0, 0x0481, 0x7466, 0x6447, 0x5424, 0x4405,
            0xA7DB, 0xB7FA, 0x8799, 0x97B8, 0xE75F, 0xF77E, 0xC71D, 0xD73C,
            0x26D3, 0x36F2, 0x0691, 0x16B0, 0x6657, 0x7676, 0x4615, 0x5634,
            0xD94C, 0xC96D, 0xF90E, 0xE92F, 0x99C8, 0x89E9, 0xB98A, 0xA9AB,
            0x5844, 0x4865, 0x7806, 0x6827, 0x18C0, 0x08E1, 0x3882, 0x28A3,
            0xCB7D, 0xDB5C, 0xEB3F, 0xFB1E, 0x8BF9, 0x9BD8, 0xABBB, 0xBB9A,
            0x4A75, 0x5A54, 0x6A37, 0x7A16, 0x0AF1, 0x1AD0, 0x2AB3, 0x3A92,
            0xFD2E, 0xED0F, 0xDD6C, 0xCD4D, 0xBDAA, 0xAD8B, 0x9DE8, 0x8DC9,
            0x7C26, 0x6C07, 0x5C64, 0x4C45, 0x3CA2, 0x2C83, 0x1CE0, 0x0CC1,
            0xEF1F, 0xFF3E, 0xCF5D, 0xDF7C, 0xAF9B, 0xBFBA, 0x8FD9, 0x9FF8,
            0x6E17, 0x7E36, 0x4E55, 0x5E74, 0x2E93, 0x3EB2, 0x0ED1, 0x1EF0
        };

        #endregion

        #region CRC32

        // ReSharper disable once InconsistentNaming
        private const uint CRC32_POLY = 0xEDB88320;

        // ReSharper disable once InconsistentNaming
        private static readonly uint[] CRC32Table;

        private static class InternalCrc32Helper
        {
            public static uint[] MakeTable(uint poly)
            {
                var ret = new uint[256];
                for (uint i = 0; i < 256; i++)
                {
                    var r = i;
                    for (var j = 0; j < 8; j++)
                        if ((r & 1) != 0)
                            r = (r >> 1) ^ poly;
                        else
                            r >>= 1;
                    ret[i] = r;
                }

                return ret;
            }
        }

        #endregion

        #region CRC64

        /// <summary>
        /// The ISO polynomial, defined in ISO 3309 and used in HDLC.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const ulong CRC64_ISO_POLY = 0xD800000000000000;

        /// <summary>
        /// The ECMA polynomial, defined in ECMA 182.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const ulong CRC64_ECMA_POLY = 0xC96C5795D7870F42;

        // ReSharper disable once InconsistentNaming
        private static readonly ulong[][] CRC64ISOTable;

        // ReSharper disable once InconsistentNaming
        private static readonly ulong[][] CRC64ECMATable;

        private static class InternalCrc64Helper
        {
            public static ulong[][] MakeTable(ulong poly)
            {
                return makeSlicingBy8Table(makeTable(poly));
            }

            // ReSharper disable once InconsistentNaming
            private static ulong[] makeTable(ulong poly)
            {
                var t = new ulong[256];
                for (int i = 0; i < 256; i++)
                {
                    var crc = (ulong) i;
                    for (int j = 0; j < 8; j++)
                    {
                        if ((crc & 1) == 1)
                            crc = (crc >> 1) ^ poly;
                        else
                            crc >>= 1;
                    }

                    t[i] = crc;
                }

                return t;
            }

            // ReSharper disable once InconsistentNaming
            private static ulong[][] makeSlicingBy8Table(ulong[] t)
            {
                var helperTable = createTables(256, 8);
                helperTable[0] = t;
                for (int i = 0; i < 256; i++)
                {
                    var crc = t[i];
                    for (int j = 1; j < 8; j++)
                    {
                        crc = t[crc & 0xff] ^ (crc >> 8);
                        helperTable[j][i] = crc;
                    }
                }

                return helperTable;
            }

            // ReSharper disable once InconsistentNaming
            private static ulong[][] createTables(int sizeInner, int sizeOuter)
            {
                var l = new List<ulong[]>();
                for (int i = 0; i < sizeOuter; i++)
                {
                    l.Add(new ulong[sizeInner]);
                }

                return l.ToArray();
            }
        }

        #endregion

        static CRCTableGenerator()
        {
            CRC32Table = InternalCrc32Helper.MakeTable(CRC32_POLY);
            CRC64ISOTable = InternalCrc64Helper.MakeTable(CRC64_ISO_POLY);
            CRC64ECMATable = InternalCrc64Helper.MakeTable(CRC64_ECMA_POLY);
        }

        private static byte[] _emptyBytes = new byte[0];

        public static byte[] EmptyBytes() => _emptyBytes;

        // ReSharper disable once InconsistentNaming
        public static ushort[] GenerationCRC16Table() => CRC16Table;

        // ReSharper disable once InconsistentNaming
        public static uint[] GenerationCRC32Table() => CRC32Table;

        // ReSharper disable once InconsistentNaming
        public static ulong[] GenerationCRC64Table(ulong poly)
        {
            return poly switch
            {
                CRC64_ISO_POLY  => CRC64ISOTable[0],
                CRC64_ECMA_POLY => CRC64ECMATable[0],
                _               => InternalCrc64Helper.MakeTable(poly)[0]
            };
        }
    }
}