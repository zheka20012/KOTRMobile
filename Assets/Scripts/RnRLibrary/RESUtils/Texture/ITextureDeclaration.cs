using System.IO;
using UnityEngine;

namespace RnRLibrary
{
    internal interface ITextureDeclaration
    {
        void LoadTextureData(BinaryReader reader, int width, int height);

        void CreateTexture(out Texture2D texture);
    }
}