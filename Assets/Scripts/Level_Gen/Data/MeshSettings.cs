using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu()]
public class MeshSettings : UpdatableData
{
    public const int numSupportedLODs = 5;
    public const int numSupportedChunkSizes = 9;
    public const int numSupportedFlatshadedChunkSizes = 3;
    public static readonly int[] supportedChunkSizes = { 48, 72, 96, 120, 144, 168, 192, 216, 240 };
    public static readonly int[] supportedFlatshadedChunkSizes = { 48, 72, 96 };
    
    public bool useFlatShading;
    public float meshScale = 2.0f;
    
    [Range(0, numSupportedChunkSizes -1)]
    public int chunkSizeIndex;
    [Range(0, numSupportedFlatshadedChunkSizes -1)]
    public int flatshadedChunkSizeIndex;

    public int chunkCount = 5;
    
    
    //num verts per line of mesh rendered at LOD 0. Includes the 2 extra verts that are excluded from final mesh, but used for calculating normals.
    public int numVertsPerLine
    {
        get
        {
            /* these values are chosen by two constraints:
            // 1. They could not be to large, otherwise we will break the maximum on allowed vertices on a single mesh
            // 2. They have to be compatible with our LOD implementation, we provide currently 5 LOD indexes
            */
            return supportedChunkSizes[(useFlatShading) ? flatshadedChunkSizeIndex : chunkSizeIndex] + 5;
        }
    }

    public float meshWorldSize
    {
        get
        {
            return (numVertsPerLine - 3) * meshScale;
        }
    }
}
