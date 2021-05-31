using UnityEngine;

namespace RnRLibrary
{
    internal class NoTileParameter : IMaterialParameter
    {
        /// <inheritdoc />
        public void Parse(RESFile file, params string[] options)
        {
            
        }

        /// <inheritdoc />
        public void ParseIntoMaterial(ref Material material)
        {
            material.mainTexture.wrapMode = TextureWrapMode.Clamp;
        }
    }
}