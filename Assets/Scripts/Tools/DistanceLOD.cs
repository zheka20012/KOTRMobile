using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DistanceLOD : MonoBehaviour
{
    [Serializable]
    public struct LOD
    {
        public List<Transform> GameObjects;
        public float Distance;

        public LOD(float distance, List<Transform> lodObjects)
        {
            GameObjects = lodObjects;
            Distance = distance;
        }

        public LOD(float distance)
        {
            GameObjects = new List<Transform>();
            Distance = distance;
        }
    }

    private LOD[] LODLevels;

    private byte _CurrentLodIndex;

    private Transform _CameraTransform;

    private void Awake()
    {

    }

    private void FixedUpdate()
    {
        if(_CameraTransform == null) FindCamera();

        for (byte i = 0; i < LODLevels.Length; i++)
        {
            if (LODLevels[i].GameObjects.Count > 0)
            {
                float sqrLength = (LODLevels[i].GameObjects[0].position - _CameraTransform.position).sqrMagnitude;

                if (sqrLength < LODLevels[i].Distance * LODLevels[i].Distance)
                {
                    ChangeLOD(i);
                }
            }

        }
    }

    private void ChangeLOD(byte newIndex)
    {
        ChangeLODState(LODLevels[_CurrentLodIndex], false);

        ChangeLODState(LODLevels[newIndex], true);
    }

    private void ChangeLODState(LOD lod, bool isEnabled)
    {
        for (int i = 0; i < lod.GameObjects.Count; i++)
        {
            lod.GameObjects[i].GetComponent<Renderer>().enabled = isEnabled;
        }
    }

    private void FindCamera()
    {
        _CameraTransform = Camera.main.transform;
    }
}
