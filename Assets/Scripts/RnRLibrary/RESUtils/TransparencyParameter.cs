using System;
using System.Globalization;
using UnityEngine;

namespace RnRLibrary
{
    public class TransparencyParameter : IMaterialParameter
    {
        private float _TransparencyLevel;
        /// <inheritdoc />
        public void Parse(RESFile file, params string[] options)
        {
            _TransparencyLevel = Single.Parse(options[0], CultureInfo.InvariantCulture);
        }

        /// <inheritdoc />
        public void ParseIntoMaterial(ref Material material)
        {
            material.shader = Shader.Find("RnRTransparentDiffuse"); //TODO: Custom shader
            var color = material.color;
            color.a = _TransparencyLevel;
            material.color = color;
        }
    }
}