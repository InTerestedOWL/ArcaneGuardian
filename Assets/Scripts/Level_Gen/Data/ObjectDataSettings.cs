using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ObjectDataSettings : UpdatableData
{
    public int minObjectsPerTile;
    public int maxObjectsPerTile;
    public GameObject[] placeableObjects;
    public Vector3[] placeableObjectSizes;
}
