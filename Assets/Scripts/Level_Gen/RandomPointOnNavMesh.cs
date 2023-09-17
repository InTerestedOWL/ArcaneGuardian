
using UnityEngine;
using UnityEngine.AI;

public static class RandomPointOnNavMesh
{
    public static bool found = false;
    public static int i = 0;
    public const int TIMES = 15;
    public static Vector3 GetPoinntForPlayerAndPOIOnNavMesh(MeshSettings meshSettings)
    {
        Vector3 result;
        i = 0;
        while (!RandomPoint(meshSettings, out result) &&  i < TIMES)
        {
            i++;
        }

        if (i < TIMES)
        {
            found = true;
        }

        return result;
    }
    

    static bool RandomPoint(MeshSettings meshSettings, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = RandomPointAboveTerrain(meshSettings) + Random.insideUnitSphere * 10.0f;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = new Vector3(hit.position.x,hit.position.y+0.1f,hit.position.z);
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
    
    
    private static  Vector3 RandomPointAboveTerrain(MeshSettings meshSettings) {
        return new Vector3(
            UnityEngine.Random.Range((5 * meshSettings.numVertsPerLine) - meshSettings.numVertsPerLine / 2, (5 * meshSettings.numVertsPerLine) + meshSettings.numVertsPerLine / 2),
            0,
            UnityEngine.Random.Range((5 * meshSettings.numVertsPerLine) - meshSettings.numVertsPerLine / 2, (5 * meshSettings.numVertsPerLine) + meshSettings.numVertsPerLine / 2)
        );
    }
}
