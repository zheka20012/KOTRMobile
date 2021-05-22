using System;
using UnityEngine;

namespace RnRLibrary
{
    public class TextureParameter : IMaterialParameter
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

            material.mainTexture = _Texture.Texture;
        }
    }
}