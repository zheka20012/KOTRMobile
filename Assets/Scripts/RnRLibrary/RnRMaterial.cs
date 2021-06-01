using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RnRLibrary
{
    public class RnRMaterial
    {
        internal static class ParameterFabric
        {
            public static IMaterialParameter Resolve(string parameter, out int paramCount)
            {
                paramCount = 0;

                switch (parameter)
                {
                    case "col":
                    {
                        paramCount = 1;
                        return new ColorParameter();
                    }
                    case "transp":
                    {
                        paramCount = 1;
                        return new TransparencyParameter();
                    }
                    case "tex":
                    {
                        paramCount = 1;
                        return new TextureParameter();
                    } 
                    case "ttx":
                    {
                        paramCount = 1;
                        return new TransparentTextureParameter();
                    }
                    case "noz":
                    {
                        return new NoZBufferParameter();
                    }
                    case "notile":
                    {
                        return new NoTileParameter();
                    }
                    case "move":
                    {
                        paramCount = 2; 
                        return new MoveParameter();
                    }
                    case "specular":
                    {
                        paramCount = 1;
                        return new SpecularParameter();
                    }
                }

                return null;
            }
        }

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

        private string MaterialName;
        private string[] _MaterialParameters;
        private List<IMaterialParameter> MaterialParameters;

        public RnRMaterial(string materialParameters)
        {
            _MaterialParameters = materialParameters.Split(new char[] { ' ' });

            MaterialName = _MaterialParameters[0];
        }

        public void InitMaterial(RESFile baseFile)
        {
            if(_MaterialParameters.Length < 1) return;

            MaterialParameters = new List<IMaterialParameter>();

            for (int i = 1; i < _MaterialParameters.Length; i++)
            {
                try
                {
                    int skipCount = 0;
                    IMaterialParameter parameter = ParameterFabric.Resolve(_MaterialParameters[i], out skipCount);

                    if (parameter == null) continue;

                    string[] matParams = new string[skipCount];
                    for (int j = 0; j < skipCount; j++)
                    {
                        matParams[j] = _MaterialParameters[i + 1 + j];
                    }
                    parameter.Parse(baseFile, matParams);

                    MaterialParameters.Add(parameter);

                    i += skipCount;
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    Debug.LogError($"{i}");
                }
                
            }
        }


        public void CreateMaterial()
        {
            _InternalMaterial = new Material(Shader.Find("RnRBaseDiffuse"));

            _InternalMaterial.name = MaterialName;

            if(MaterialParameters == null) return; 

            for (int i = 0; i < MaterialParameters.Count; i++)
            {
                if(MaterialParameters[i] == null) continue;

                MaterialParameters[i].ParseIntoMaterial(ref _InternalMaterial);
            }
        }
    }
}