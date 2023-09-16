
using UnityEngine;
using UnityEngine.AI;

public static class RandomPointOnNavMesh
{
    public static Vector3 GetPoinntForPlayerAndPOIOnNavMesh(MeshSettings meshSettings)
    {
        Vector3 result;
        while (!RandomPoint(meshSettings, out result))
        {
        }

        return result;
    }
    

    static bool RandomPoint(MeshSettings meshSettings, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = Vector3.zero + Random.insideUnitSphere * 10.0f;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
    
    
    private static  Vector3 RandomPointAboveTerrain(MeshSettings meshSettings) {
        return new Vector3(
            UnityEngine.Random.Range((5 * meshSettings.numVertsPerLine) - meshSettings.numVertsPerLine / 2, (5 * meshSettings.numVertsPerLine) + meshSettings.numVertsPerLine / 2),
            2,
            UnityEngine.Random.Range((5 * meshSettings.numVertsPerLine) - meshSettings.numVertsPerLine / 2, (5 * meshSettings.numVertsPerLine) + meshSettings.numVertsPerLine / 2)
        );
    }
}
