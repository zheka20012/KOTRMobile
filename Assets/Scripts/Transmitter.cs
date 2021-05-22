
using System.Collections.Generic;
using UnityEngine;

public class Transmitter : MonoBehaviour
{
    [SerializeField]
    private float SearchDistance = 50f;

    private List<IDriver> _NearestDrivers;

    private Transform _CachedTransform;

    private void Start()
    {
        _CachedTransform = GetComponent<Transform>();
        _NearestDrivers = new List<IDriver>();
        SearchDistance *= SearchDistance;
    }

    public void Open()
    {
        
    }

    private void GetNearestDriversList()
    {
        _NearestDrivers.Clear();

        for (int i = 0; i < GameManager.Instance.Drivers.Count; i++)
        {
            if (Vector3.SqrMagnitude(_CachedTransform.position -
                                     GameManager.Instance.Drivers[i].GameObject.transform.position) < SearchDistance)
            {
                _NearestDrivers.Add(GameManager.Instance.Drivers[i]);
            }
        }
    }
}
