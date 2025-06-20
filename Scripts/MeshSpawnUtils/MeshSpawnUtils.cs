using UnityEngine;

public static class MeshSpawnUtils
{
    public static GameObject SpawnPrefabOnMesh(
        GameObject prefab,
        Vector3 worldXZ,
        Transform parent = null)
    {
        if (prefab == null)
        {
            Debug.LogError("[MeshSpawnUtils] Prefab is null.");
            return null;
        }

        // 1) find the terrain generator
        var meshGen = Object.FindObjectOfType<HeightmapMeshGenerator>();
        if (meshGen == null)
        {
            Debug.LogError("[MeshSpawnUtils] No HeightmapMeshGenerator found.");
            return null;
        }

        // 2) compute surfaceY from the heightmap
        float[,] hm       = meshGen.GetHeightmap();
        float meshSize    = meshGen.meshSize;
        float heightScale = meshGen.heightScale;
        Vector3 origin    = meshGen.meshOrigin;

        float u = (worldXZ.x - origin.x) / meshSize + 0.5f;
        float v = (worldXZ.z - origin.z) / meshSize + 0.5f;
        int rows = hm.GetLength(0), cols = hm.GetLength(1);

        int iy = Mathf.Clamp(Mathf.FloorToInt(v * (rows - 1)), 0, rows - 1);
        int ix = Mathf.Clamp(Mathf.FloorToInt(u * (cols - 1)), 0, cols - 1);

        float surfaceY = origin.y + hm[iy, ix] * heightScale;
        Vector3 spawnPos = new Vector3(worldXZ.x, surfaceY, worldXZ.z);

        // 3) instantiate at that Y
        GameObject go = Object.Instantiate(prefab, spawnPos, Quaternion.identity, parent);

        // 4) first try a "Feet" locator
        Transform feet = go.transform.Find("Feet");
        if (feet != null)
        {
            float bottomY = feet.position.y;
            float offset  = surfaceY - bottomY;
            go.transform.position += Vector3.up * offset;
            return go;
        }

        // 5) no Feet? bake any SkinnedMeshRenderers
        float bottom = float.MaxValue;
        bool  found  = false;

        foreach (var skin in go.GetComponentsInChildren<SkinnedMeshRenderer>(true))
        {
            Mesh baked = new Mesh();
            skin.BakeMesh(baked);
            Vector3 worldMin = skin.transform.TransformPoint(baked.bounds.min);
            bottom = Mathf.Min(bottom, worldMin.y);
            found  = true;
        }

        // 6) fallback to MeshRenderers
        foreach (var mr in go.GetComponentsInChildren<MeshRenderer>(true))
        {
            bottom = Mathf.Min(bottom, mr.bounds.min.y);
            found  = true;
        }

        // 7) fallback to Colliders
        foreach (var col in go.GetComponentsInChildren<Collider>(true))
        {
            bottom = Mathf.Min(bottom, col.bounds.min.y);
            found  = true;
        }

        if (found)
        {
            float offset = surfaceY - bottom;
            go.transform.position += Vector3.up * offset;
        }
        else
        {
            Debug.LogWarning($"[MeshSpawnUtils] No Feet/SkinnedMesh/MeshRenderer/Collider on '{go.name}', can't align bottom.");
        }

        return go;
    }
}