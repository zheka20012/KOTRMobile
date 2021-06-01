using System.IO;
using UnityEngine;

namespace RnRLibrary
{
    internal interface IMask
    {
        void LoadTextureData(BinaryReader reader);

        Texture2D CreateTexture();
    }
}