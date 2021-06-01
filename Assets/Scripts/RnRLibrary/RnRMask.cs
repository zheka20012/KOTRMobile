using System.IO;
using RnRLibrary.Utility;
using UnityEngine;

namespace RnRLibrary
{
    public class RnRMask : RnRTexture
    {
        /// <inheritdoc />
        public override Texture2D Texture
        {
            get
            {
                if (_InternalMask == null)
                {
                    if (Mask == null)
                    {
                        _InternalMask = Texture2D.whiteTexture;
                    }
                    else
                    {
                        _InternalMask = Mask.CreateTexture();
                    }
                }

                return _InternalMask;
            }
        }

        private Texture2D _InternalMask;

        private IMask Mask;

        /// <inheritdoc />
        public override void Read(BinaryReader reader)
        {
            string maskType = reader.ReadBytes(4).GetString();

            switch (maskType)
            {
                case "MS16":
                    Mask = new MS16();
                    break;
                case "MSK8":
                    Mask = new MSK8();
                    break;
                case "MASK":
                    Mask = new MSK8();
                    break;
                default:
                    return;
            }
            
            Mask.LoadTextureData(reader);
        }
    }
}