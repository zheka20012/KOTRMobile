using System;
using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary
{
    // 16-bit RLE-encoded RGB565 image
    internal class MS16 : IMask
    {
        private ushort Width;
        private ushort Height;

        private byte[] ImageData;

        private PixelFormatMask _PixelFormatMask;

        /// <inheritdoc />
        public void LoadTextureData(BinaryReader reader)
        {
            Width = reader.ReadUInt16();
            Height = reader.ReadUInt16();

            reader.BaseStream.Seek(768, SeekOrigin.Current); //Empty Data

            ImageData = reader.DecodeRLE(Width, Height, 2);

            if (reader.ReadBytes(4).GetString() != "PFRM")
            {
                Debug.Log($"Wrong Image Type! PFRM Section not fount at position {reader.BaseStream.Position}");

                return;
            }

            _PixelFormatMask = reader.Read<PixelFormatMask>();
        }

        /// <inheritdoc />
        public Texture2D CreateTexture()
        {
            TextureUtility.TextureBytesConverter converter;

            try
            {
                converter = TextureUtility.TextureBytesConverter.GetConverter(_PixelFormatMask.GetTextureFormat());
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return Texture2D.whiteTexture;
            }

            Texture2D texture = new Texture2D(Width, Height, converter.UnityTextureFormat, false);

            texture.LoadRawTextureData(converter.FlipBytes(converter.ConvertBytes(ImageData, _PixelFormatMask), Width, Height));

            texture.Apply(false, false);

            return texture;
        }


    }
}