using UnityEngine;

namespace RnRLibrary
{
    public class RnRMaterial
    {
        public Material Material
        {
            get
            {
                if (_InternalMaterial == null)
                {
                    CreateMaterial();
                }

                return _InternalMaterial;
            }
        }

        private Material _InternalMaterial;

        public RnRMaterial()
        {

        }


        public void CreateMaterial()
        {

        }
    }
}