using System;
using System.Globalization;
using UnityEngine;

namespace RnRLibrary
{
    internal class MoveParameter : IMaterialParameter
    {
        private float _MoveX;
        private float _MoveY;
        /// <inheritdoc />
        public void Parse(RESFile file, params string[] options)
        {
            _MoveX = Single.Parse(options[0], CultureInfo.InvariantCulture);
            _MoveY = Single.Parse(options[1], CultureInfo.InvariantCulture);
        }

        /// <inheritdoc />
        public void ParseIntoMaterial(ref Material material)
        {
            //TODO: create custom material
            material.SetFloat("_MoveX", _MoveX);
            material.SetFloat("_MoveY", _MoveY);
        }
    }
}