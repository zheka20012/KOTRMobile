using System.IO;
using UnityEngine;

namespace RnRLibrary
{
    public class MASK : IMask
    {
        private ushort Width, Height;
        /// <inheritdoc />
        public void LoadTextureData(BinaryReader reader)
        {
            Width = reader.ReadUInt16();
            Height = reader.ReadUInt16();
        }

        /// <inheritdoc />
        public Texture2D CreateTexture()
        {
            throw new System.NotImplementedException();
        }
    }
}