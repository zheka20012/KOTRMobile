using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary
{
    internal struct PixelFormatMask : IBinaryReadable
    {
        public uint RedMask;
        public uint GreenMask;
        public uint BlueMask;
        public uint AlphaMask;

        /// <inheritdoc />
        public void Read(BinaryReader reader)
        {
            var blockSize = reader.ReadUInt32() + reader.BaseStream.Position;

            RedMask = reader.ReadUInt32();
            GreenMask = reader.ReadUInt32();
            BlueMask = reader.ReadUInt32();
            AlphaMask = reader.ReadUInt32();

            reader.BaseStream.Seek(blockSize, SeekOrigin.Begin);
        }

        public RnRLibrary.TextureFormat GetTextureFormat()
        {
            if (BlueMask == 0x1F)
            {

                if (GreenMask == 0x7E0 && RedMask == 0xF800 && AlphaMask == 0x0)
                {
                    return RnRLibrary.TextureFormat.RGB565;
                }

                if (GreenMask == 0x3E0 && RedMask == 0x7C00 && AlphaMask == 0)
                {
                    return RnRLibrary.TextureFormat.RGB555;
                }
            }
            else if (BlueMask == 0xF && GreenMask == 0xF0 && RedMask == 0xF00 && AlphaMask == 0xF000)
            {
                return RnRLibrary.TextureFormat.ARGB4444;
            }
            else if (BlueMask == 0xFF && GreenMask == 0xFF00 && RedMask == 0xFF0000)
            {
                if (AlphaMask == 0)
                {
                    return RnRLibrary.TextureFormat.RGB888;
                }

                return RnRLibrary.TextureFormat.RGBA8888;
            }

            Debug.Log($"Unknown texture format: R: 0x{RedMask:x}, G: 0x{GreenMask:x}, B: 0x{BlueMask:x}, A: 0x{AlphaMask:x}");
            return RnRLibrary.TextureFormat.Unknown;
        }
    }
}