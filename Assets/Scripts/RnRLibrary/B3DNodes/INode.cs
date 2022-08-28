using System.IO;
using UnityEngine;

namespace RnRLibrary.B3DNodes
{
    public interface INode
    {
        string Name { get; }

        void Read(BinaryReader reader);

        Transform ProcessNode(Transform parentTransform, B3DFile file);
    }
}