using System;
using System.Globalization;
using UnityEngine;

namespace RnRLibrary
{
    internal class PowerParameter : IMaterialParameter
    {
        private float _Power;

        /// <inheritdoc />
        public void Parse(RESFile file, params string[] options)
        {
            _Power = Single.Parse(options[0], CultureInfo.InvariantCulture);
        }

        /// <inheritdoc />
        public void ParseIntoMaterial(ref Material material)
        {
            material.SetFloat("_Power", _Power);
        }
    }
}