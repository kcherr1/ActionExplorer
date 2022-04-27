using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExtendedPlayerActions : MonoBehaviour {
  private List<TreeInstance> treeList;
  public GameObject fallingTreePrefab;

  private void Start() {
    treeList = new List<TreeInstance>(Terrain.activeTerrain.terrainData.treeInstances);
    print(treeList.Count);
  }

  public void OnClick() {
    //Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    if (Physics.Raycast(ray, out RaycastHit hit, 12)) {
      if (hit.transform.gameObject.CompareTag("NPC")) {
        hit.transform.gameObject.GetComponent<NPC>().StartTalking();
      }
      else if (hit.transform.gameObject.CompareTag("NPC Button")) {
        hit.transform.gameObject.GetComponent<NPCDialogButton>().Click();
      }
      else if (hit.transform.gameObject.CompareTag("Collectable")) {
        hit.transform.gameObject.GetComponent<Collectable>().Collect();
      }

      /** original code found at: https://forum.unity.com/threads/finally-removing-trees-and-the-colliders.110354/
         * this has been modified from the original
         */
      else if (hit.transform.name == Terrain.activeTerrain.name) {
        print("here");
        // We hit the "terrain"! Now, how high is the ground at that point?
        float sampleHeight = Terrain.activeTerrain.SampleHeight(hit.point);

        // If the height of the exact point we clicked/chopped at or below ground level, all we did
        // was chop dirt.
        if (hit.point.y <= sampleHeight + 0.01f) {
          return;
        }

        TerrainData terrainData = Terrain.activeTerrain.terrainData;
        TreeInstance[] treeInstances = terrainData.treeInstances;

        // Our current closest tree initializes to far away
        float closestDist = float.MaxValue;
        // Track our closest tree's position
        Vector3 closestTreePosition = new Vector3();
        // Let's find the closest tree to the place we chopped and hit something
        int closestTreeIndex = 0;

        for (int i = 0; i < treeInstances.Length; i++) {
          TreeInstance currentTree = treeInstances[i];
          // The the actual world position of the current tree we are checking
          Vector3 currentTreeWorldPosition = Vector3.Scale(currentTree.position, terrainData.size) + Terrain.activeTerrain.transform.position;

          // Find the distance between the current tree and whatever we hit when chopping
          float distance = Vector3.Distance(currentTreeWorldPosition, hit.point);

          // Is this tree even closer?
          if (distance < closestDist) {
            closestDist = distance;
            closestTreeIndex = i;
            closestTreePosition = currentTreeWorldPosition;
          }
        }

        // Remove the tree from the terrain tree list
        treeList.RemoveAt(closestTreeIndex);
        terrainData.treeInstances = treeList.ToArray();

        // Now refresh the terrain, getting rid of the darn collider
        float[,] heights = terrainData.GetHeights(0, 0, 0, 0);
        terrainData.SetHeights(0, 0, heights);

        // Put a falling tree in its place
        Instantiate(fallingTreePrefab, closestTreePosition + (Vector3.up * 1), Quaternion.identity);
      }
    }
  }
}