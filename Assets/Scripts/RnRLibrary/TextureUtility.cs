using System;
using UnityEngine;
using UnityTextureFormat = UnityEngine.TextureFormat;

namespace RnRLibrary
{
    public class TextureUtility
    {
        public abstract class TextureBytesConverter
        {
            public abstract UnityTextureFormat UnityTextureFormat { get; }
            public abstract byte[] ConvertBytes(byte[] inBytes, RnRTexture.PixelFormatMask mask);

            public abstract byte[] FlipBytes(byte[] inputBytes, ushort imageWidth, ushort imageHeight);

            public static TextureBytesConverter GetConverter(TextureFormat format)
            {
                switch (format)
                {
                    case TextureFormat.Unknown:
                        throw new ArgumentException("Unknown texture format!");
                    case TextureFormat.RGB555:
                        return new RGB555Converter();
                    case TextureFormat.RGB565:
                        return new RGB565Converter();
                    case TextureFormat.ARGB4444:
                        return new RGB4444Converter();
                    case TextureFormat.RGB888:
                        return new RGB888Converter();
                    case TextureFormat.RGBA8888:
                        return new RGB8888Converter();
                    default:
                        throw new ArgumentOutOfRangeException(nameof(format), format, null);
                }
            }
        }

        public class RGB555Converter : TextureBytesConverter
        {
            /// <inheritdoc />
            public override UnityTextureFormat UnityTextureFormat
            {
                get { return UnityTextureFormat.RGB24; }
            }

            /// <inheritdoc />
            public override byte[] ConvertBytes(byte[] data, RnRTexture.PixelFormatMask mask)
            {
                byte[] rgbTextureBytes = new byte[data.Length / 2 * 3];

                for (int i = 0, j = 0; i < data.Length; i += 2)
                {
                    short c = BitConverter.ToInt16(data, i);
                    rgbTextureBytes[j++] = (byte)(((c & mask.RedMask) >> 10) << 3);
                    rgbTextureBytes[j++] = (byte)(((c & mask.GreenMask) >> 5) << 3);
                    rgbTextureBytes[j++] = (byte)((c & mask.BlueMask) << 3);
                }

                return rgbTextureBytes;
            }

            /// <inheritdoc />
            public override byte[] FlipBytes(byte[] inputBytes, ushort imageWidth, ushort imageHeight)
            {
                return TextureUtility.FlipBytes(inputBytes, imageWidth, imageHeight, 3);
            }
        }

        public class RGB565Converter : TextureBytesConverter
        {
            public override UnityTextureFormat UnityTextureFormat
            {
                get { return UnityTextureFormat.RGB565; }
            }

            /// <inheritdoc />
            public override byte[] ConvertBytes(byte[] inBytes, RnRTexture.PixelFormatMask mask)
            {
                return inBytes;
            }

            /// <inheritdoc />
            public override byte[] FlipBytes(byte[] inputBytes, ushort imageWidth, ushort imageHeight)
            {
                return TextureUtility.FlipBytes(inputBytes, imageWidth, imageHeight, 2);
            }
        }

        public class RGB4444Converter : TextureBytesConverter
        {
            public override UnityTextureFormat UnityTextureFormat
            {
                get { return UnityTextureFormat.ARGB4444; }
            }
            /// <inheritdoc />
            public override byte[] ConvertBytes(byte[] inBytes, RnRTexture.PixelFormatMask mask)
            {
                return inBytes;
            }

            /// <inheritdoc />
            public override byte[] FlipBytes(byte[] inputBytes, ushort imageWidth, ushort imageHeight)
            {
                return TextureUtility.FlipBytes(inputBytes, imageWidth, imageHeight, 2);
            }
        }

        public class RGB8888Converter : TextureBytesConverter
        {
            public override UnityTextureFormat UnityTextureFormat
            {
                get { return UnityTextureFormat.RGBA32; }
            }
            /// <inheritdoc />
            public override byte[] ConvertBytes(byte[] inBytes, RnRTexture.PixelFormatMask mask)
            {
                byte[] rgbTextureBytes = new byte[inBytes.Length / 2 * 4];

                for (int i = 0, j = 0; i < inBytes.Length; i += 2)
                {

                    short c = BitConverter.ToInt16(inBytes, i);
                    rgbTextureBytes[j++] = (byte)(((c & mask.RedMask) >> 8) << 4);
                    rgbTextureBytes[j++] = (byte)(((c & mask.GreenMask) >> 4) << 4);
                    rgbTextureBytes[j++] = (byte)(((c & mask.BlueMask)) << 4);
                    rgbTextureBytes[j++] = (byte)(((c & mask.AlphaMask) >> 12) << 4);

                }

                return rgbTextureBytes;
            }

            /// <inheritdoc />
            public override byte[] FlipBytes(byte[] inputBytes, ushort imageWidth, ushort imageHeight)
            {
                return TextureUtility.FlipBytes(inputBytes, imageWidth, imageHeight, 4);
            }
        }

        public class RGB888Converter : TextureBytesConverter
        {
            public override UnityTextureFormat UnityTextureFormat
            {
                get { return UnityTextureFormat.RGB24; }
            }
            /// <inheritdoc />
            public override byte[] ConvertBytes(byte[] inBytes, RnRTexture.PixelFormatMask mask)
            {
                byte[] rgbTextureBytes = new byte[inBytes.Length / 2 * 3];

                for (int i = 0, j = 0; i < inBytes.Length; i += 2)
                {

                    short c = BitConverter.ToInt16(inBytes, i);
                    rgbTextureBytes[j++] = (byte)(((c & mask.RedMask) >> 8) << 4);
                    rgbTextureBytes[j++] = (byte)(((c & mask.GreenMask) >> 4) << 4);
                    rgbTextureBytes[j++] = (byte)(((c & mask.BlueMask)) << 4);
                }

                return rgbTextureBytes;
            }

            /// <inheritdoc />
            public override byte[] FlipBytes(byte[] inputBytes, ushort imageWidth, ushort imageHeight)
            {
                return TextureUtility.FlipBytes(inputBytes, imageWidth, imageHeight, 3);
            }
        }

        protected static byte[] FlipBytes(byte[] inputBytes, ushort imageWidth, ushort imageHeight, byte componentsCount)
        {
            byte[] flippedBytes = new byte[inputBytes.Length];

            for (int i = 0; i < imageWidth; i++)
            {
                for (int j = 0; j < imageHeight; j++)
                {
                    for (int k = 0; k < componentsCount; k++)
                    {
                        flippedBytes[(i + j * imageWidth) * componentsCount + k] = inputBytes[(i + (imageHeight - 1 - j) * imageWidth) * componentsCount + k];
                    }
                }
            }

            return flippedBytes;
        }
    }
}