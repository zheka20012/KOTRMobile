using System;
using UnityEngine;

namespace RnRLibrary
{
    public class ColorParameter : IMaterialParameter
    {
        private Color _Color;

        /// <inheritdoc />
        public void Parse(RESFile file, params string[] options)
        {
            _Color = file.Palettes[0].Item.PaletteColors[Int32.Parse(options[0])-1];
        }

        /// <inheritdoc />
        public void ParseIntoMaterial(ref Material material)
        {
            material.color = _Color;
        }
    }
}