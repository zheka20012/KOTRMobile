using System;
using System.IO;

namespace RnRLibrary
{
    internal static class MaskUtility
    {
        public static byte[] DecodeRLE(this BinaryReader reader, ushort width, ushort height, byte componentsCount = 1)
        {
            byte[] decodedBytes = new byte[width*height*componentsCount];

            for (int i = 0, j = 0; j < decodedBytes.Length;)
            {
                byte rleByte = reader.ReadByte();

                if (rleByte < 128)
                {
                    byte[] readBytes = reader.ReadBytes(rleByte*componentsCount);
                    Array.Copy(readBytes, 0, decodedBytes, j, rleByte * componentsCount);
                    j += rleByte * componentsCount;
                    i += rleByte * componentsCount;
                }
                else
                {
                    j += (rleByte - 128) * componentsCount;
                    i += (rleByte - 128) * componentsCount;
                }
            }

            return decodedBytes;
        }
    }
}