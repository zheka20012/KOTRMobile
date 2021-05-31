using System;
using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary
{
    /// <summary>
    /// TXR Texture format from king
    /// </summary>
    public partial class RnRTexture : IBinaryReadable
    {


        public Texture2D Texture
        {
            get
            {
                if (_InternalTexture == null)
                {
                    TextureDeclaration.CreateTexture(out _InternalTexture);
                }

                return _InternalTexture;
            }
        }

        private Texture2D _InternalTexture;

        private ITextureDeclaration TextureDeclaration;

        /// <inheritdoc />
        public virtual void Read(BinaryReader reader)
        {
            reader.BaseStream.Seek(12, SeekOrigin.Current);

            int width = reader.ReadUInt16();
            int height = reader.ReadUInt16();
            byte bitDepth = reader.ReadByte();

            reader.BaseStream.Seek(1, SeekOrigin.Current);

            if (bitDepth == 16)
            {
                TextureDeclaration = new Texture16Bit();
            }
            else
            {
                TextureDeclaration = new Texture8Bit();
            }

            TextureDeclaration.LoadTextureData(reader, width, height);
        }

    }
}