using System;
using UnityEngine;

namespace RnRLibrary
{
    public class SpecularParameter : IMaterialParameter
    {
        private float _Power;

        /// <inheritdoc />
        public void Parse(RESFile file, params string[] options)
        {
            _Power = Single.Parse(options[0]);
        }

        /// <inheritdoc />
        public void ParseIntoMaterial(ref Material material)
        {
            material.SetFloat("_Shininess", _Power);
        }
    }
}