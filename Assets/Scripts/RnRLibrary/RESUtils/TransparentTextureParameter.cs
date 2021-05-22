using System;
using UnityEngine;

namespace RnRLibrary
{
    public class TransparentTextureParameter : IMaterialParameter
    {
        private RnRTexture _Texture;

        /// <inheritdoc />
        public void Parse(RESFile file, params string[] options)
        {
            _Texture = file.TextureFiles[Int32.Parse(options[0])-1].Item;
        }

        /// <inheritdoc />
        public void ParseIntoMaterial(ref Material material)
        {
            material.shader = Shader.Find("RnRTransparentDiffuse"); //TODO: Custom shader
            material.mainTexture = _Texture.Texture;
        }
    }
}