using UnityEngine;

namespace RnRLibrary
{
    internal interface IMaterialParameter
    {
        void Parse(RESFile file, params string[] options);
        void ParseIntoMaterial(ref Material material);
    }
}