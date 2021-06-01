using System.IO;
using UnityEngine;

namespace RnRLibrary
{
    public class MSK8 : IMask
    {
        private Color32[] Palette;

        private byte[] ImageData;

        private ushort Width, Height;

        /// <inheritdoc />
        public void LoadTextureData(BinaryReader reader)
        {
            Width = reader.ReadUInt16();
            Height = reader.ReadUInt16();

            //Palette = reader.ReadBytes(768); //256 colors * 3 components

            Palette = new Color32[256];

            for (int i = 0; i < Palette.Length; i++)
            {
                Palette[i] = new Color32(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), 255);
            }

            ImageData = reader.DecodeRLE(Width, Height);
        }

        /// <inheritdoc />
        public Texture2D CreateTexture()
        {
            byte[] textureBytes = new byte[Width * Height * 3];

            for (int i = 0, j = 0; i < ImageData.Length; i++)
            {
                textureBytes[j++] = Palette[ImageData[i]].r;
                textureBytes[j++] = Palette[ImageData[i]].g;
                textureBytes[j++] = Palette[ImageData[i]].b;
            }

            Texture2D texture = new Texture2D(Width, Height, UnityEngine.TextureFormat.RGB24, false);

            texture.LoadRawTextureData(TextureUtility.FlipBytes(textureBytes, Width, Height, 3));

            texture.Apply();

            return texture;
        }
    }
}