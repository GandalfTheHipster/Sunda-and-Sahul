using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(PerlinHeightmapGenerator))]
public class HeightmapMeshGenerator : MonoBehaviour
{
    [Header("Mesh Settings")]
    [Tooltip("Size of the mesh in world units.")]
    public float meshSize = 10f;

    [Tooltip("Overall strength of the height displacement.")]
    public float heightScale = 2f;

    [Header("Spawn Position")]
    [Tooltip("World position where the mesh will be placed.")]
    public Vector3 meshOrigin = Vector3.zero;

    [Header("Preview Material (optional)")]
    [Tooltip("Material to apply to the generated mesh.")]
    public Material meshMaterial;

    [Header("NavMesh Baking")]
    [Tooltip("Automatically bake a NavMesh over the generated mesh at runtime.")]
    public bool bakeNavMesh = false;

    private PerlinHeightmapGenerator heightGen;
    private MeshFilter meshFilter;

    // For runtime NavMesh data
    private NavMeshData navMeshData;
    private NavMeshDataInstance navMeshInstance;

    void Awake()
    {
        heightGen = GetComponent<PerlinHeightmapGenerator>();
        meshFilter = GetComponent<MeshFilter>();
    }

    void Start()
    {
        transform.position = meshOrigin;
        heightGen.Generate();
        CreateMeshFromHeightmap();

        if (bakeNavMesh)
            BakeRuntimeNavMesh();
    }

    void CreateMeshFromHeightmap()
    {
        float[,] hm = heightGen.GetHeightmap();
        int resX = hm.GetLength(1);
        int resY = hm.GetLength(0);

        Mesh mesh = new Mesh { name = "HeightmapMesh" };
        Vector3[] vertices = new Vector3[resX * resY];
        Vector2[] uvs = new Vector2[resX * resY];

        for (int y = 0; y < resY; y++)
        {
            for (int x = 0; x < resX; x++)
            {
                int idx = y * resX + x;
                float u = (float)x / (resX - 1);
                float v = (float)y / (resY - 1);
                float posX = (u - 0.5f) * meshSize;
                float posZ = (v - 0.5f) * meshSize;
                float posY = hm[y, x] * heightScale;

                vertices[idx] = new Vector3(posX, posY, posZ);
                uvs[idx] = new Vector2(u, v);
            }
        }

        int quadCount = (resX - 1) * (resY - 1);
        int[] triangles = new int[quadCount * 6];
        int t = 0;
        for (int y = 0; y < resY - 1; y++)
        {
            for (int x = 0; x < resX - 1; x++)
            {
                int i = y * resX + x;
                triangles[t++] = i;
                triangles[t++] = i + resX;
                triangles[t++] = i + resX + 1;
                triangles[t++] = i;
                triangles[t++] = i + resX + 1;
                triangles[t++] = i + 1;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;

        if (meshMaterial != null)
            GetComponent<MeshRenderer>().material = meshMaterial;

        Debug.Log($"[HeightmapMeshGenerator] Generated mesh ({resX}×{resY}) at {meshOrigin} with size {meshSize}.");
    }

    void BakeRuntimeNavMesh()
    {
        // Collect NavMesh sources from this GameObject's mesh
        List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();
        NavMeshBuildMarkup markup = new NavMeshBuildMarkup();
        markup.root = transform;
        markup.overrideArea = false;
        markup.ignoreFromBuild = false;
        NavMeshBuilder.CollectSources(
            transform, LayerMask.GetMask("Default"),
            NavMeshCollectGeometry.RenderMeshes, 0,
            new List<NavMeshBuildMarkup> { markup }, sources);

        // Define bounds based on mesh size and height
        Bounds bounds = new Bounds(meshOrigin, new Vector3(meshSize, heightScale, meshSize));

        // Build and add NavMeshData
        navMeshData = NavMeshBuilder.BuildNavMeshData(NavMesh.GetSettingsByID(0), sources, bounds, meshOrigin, Quaternion.identity);
        navMeshInstance = NavMesh.AddNavMeshData(navMeshData);

        Debug.Log("[HeightmapMeshGenerator] Runtime NavMesh baked.");
    }
}