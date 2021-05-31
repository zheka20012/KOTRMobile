using UnityEngine;

namespace RnRLibrary
{
    internal class NoZBufferParameter : IMaterialParameter
    {
        /// <inheritdoc />
        public void Parse(RESFile file, params string[] options)
        {
            
        }

        /// <inheritdoc />
        public void ParseIntoMaterial(ref Material material)
        {
            //TODO: Custom material support zwrite switching
            material.SetInt("_ZWrite", 0);
        }
    }
}