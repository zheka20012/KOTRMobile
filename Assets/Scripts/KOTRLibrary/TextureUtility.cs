﻿using System;
using UnityEngine;

namespace KOTRLibrary
{
    public class TextureUtility
    {
        public abstract class TextureBytesConverter
        {
            public abstract TextureFormat UnityTextureFormat { get; }
            public abstract byte[] ConvertBytes(byte[] inBytes, KOTRTexture.PixelFormatMask mask);

            public abstract byte[] FlipBytes(byte[] inputBytes, ushort imageWidth, ushort imageHeight);

            public static TextureBytesConverter GetConverter(KOTRTextureFormat format)
            {
                switch (format)
                {
                    case KOTRTextureFormat.Unknown:
                        throw new ArgumentException("Unknown texture format!");
                        break;
                    case KOTRTextureFormat.RGB555:
                        return new RGB555Converter();
                        break;
                    case KOTRTextureFormat.RGB565:
                        return new RGB565Converter();
                        break;
                    case KOTRTextureFormat.ARGB4444:
                        return new RGB4444Converter();
                        break;
                    case KOTRTextureFormat.RGB888:
                        return new RGB888Converter();
                        break;
                    case KOTRTextureFormat.RGBA8888:
                        return new RGB8888Converter();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(format), format, null);
                }
            }
        }

        public class RGB555Converter : TextureBytesConverter
        {
            /// <inheritdoc />
            public override TextureFormat UnityTextureFormat
            {
                get { return TextureFormat.RGB24; }
            }

            /// <inheritdoc />
            public override byte[] ConvertBytes(byte[] data, KOTRTexture.PixelFormatMask mask)
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
            public override TextureFormat UnityTextureFormat
            {
                get { return TextureFormat.RGB565; }
            }

            /// <inheritdoc />
            public override byte[] ConvertBytes(byte[] inBytes, KOTRTexture.PixelFormatMask mask)
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
            public override TextureFormat UnityTextureFormat
            {
                get { return TextureFormat.ARGB4444; }
            }
            /// <inheritdoc />
            public override byte[] ConvertBytes(byte[] inBytes, KOTRTexture.PixelFormatMask mask)
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
            public override TextureFormat UnityTextureFormat
            {
                get { return TextureFormat.RGBA32; }
            }
            /// <inheritdoc />
            public override byte[] ConvertBytes(byte[] inBytes, KOTRTexture.PixelFormatMask mask)
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
            public override TextureFormat UnityTextureFormat
            {
                get { return TextureFormat.RGB24; }
            }
            /// <inheritdoc />
            public override byte[] ConvertBytes(byte[] inBytes, KOTRTexture.PixelFormatMask mask)
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