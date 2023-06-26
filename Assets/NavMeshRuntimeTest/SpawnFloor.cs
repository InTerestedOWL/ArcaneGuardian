using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnFloor : MonoBehaviour {
    [SerializeField] private GameObject tilePrefab;
    List<NavMeshSurface> surfaces;
    // Start is called before the first frame update
    void Start() {
        surfaces = new List<NavMeshSurface>();
        if (tilePrefab) {
            for (int i = -1; i < 10; i++) {
                Vector3 tilePosition = new Vector3(i, 0, 0);
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;
                surfaces.Add(tile.GetComponent<NavMeshSurface>());
                Vector3 tilePosition2 = new Vector3(i, 0, 2);
                GameObject tile2 = Instantiate(tilePrefab, tilePosition2, Quaternion.identity) as GameObject;
                surfaces.Add(tile2.GetComponent<NavMeshSurface>());
                Vector3 tilePosition3 = new Vector3(i, 0, -2);
                GameObject tile3 = Instantiate(tilePrefab, tilePosition3, Quaternion.identity) as GameObject;
                surfaces.Add(tile3.GetComponent<NavMeshSurface>());
            }

            foreach (NavMeshSurface surface in surfaces) {
                surface.BuildNavMesh();
            }
        }
    }

    // Update is called once per frame
    void Update() {
        
    }
}
