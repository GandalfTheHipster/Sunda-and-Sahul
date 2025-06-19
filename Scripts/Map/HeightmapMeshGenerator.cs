using UnityEngine;
using UnityEngine.AI;

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

    [Header("Tree Spawning")]
    [Tooltip("Prefab to spawn for each tree.")]
    public GameObject treePrefab;

    [Tooltip("Exact color in biome map to match for spawning trees.")]
    public Color treeColor = new Color(0.2f, 0.8f, 0.2f, 1f);

    [Tooltip("Max color difference tolerance for matching (0–1).")]
    [Range(0f, 0.1f)]
    public float colorTolerance = 0.01f;

    [Tooltip("Spacing in pixels between tree spawn checks. Higher = fewer trees.")]
    [Range(1, 16)]
    public int spacing = 1;

    [Tooltip("Path under Resources to biome map PNG (omit extension), e.g. 'BiomesOutputs/biomemap'.")]
    public string biomeMapResourcePath = "BiomesOutputs/biomemap";

    private PerlinHeightmapGenerator heightGen;
    private MeshFilter meshFilter;
    private Texture2D biomeMap;

    void Awake()
    {
        heightGen  = GetComponent<PerlinHeightmapGenerator>();
        meshFilter = GetComponent<MeshFilter>();
    }

    void Start()
    {
        // Generate terrain mesh
        transform.position = meshOrigin;
        heightGen.Generate();
        CreateMeshFromHeightmap();

        // Load biome map and spawn trees
        LoadBiomeMap();
        SpawnTreesWithSpacing();

        // Optionally bake NavMesh via NavMeshBaker
        if (bakeNavMesh)
        {
            NavMeshBaker.BakeNavMesh(
                meshFilter,
                meshOrigin,
                meshSize,
                heightScale
            );
        }
    }

    void CreateMeshFromHeightmap()
    {
        float[,] hm = heightGen.GetHeightmap();
        int resX = hm.GetLength(1);
        int resY = hm.GetLength(0);

        Mesh mesh = new Mesh { name = "HeightmapMesh" };
        Vector3[] vertices = new Vector3[resX * resY];
        Vector2[] uvs      = new Vector2[resX * resY];

        for (int y = 0; y < resY; y++)
        {
            for (int x = 0; x < resX; x++)
            {
                int i = y * resX + x;
                float u = (float)x / (resX - 1);
                float v = (float)y / (resY - 1);
                float posX = (u - 0.5f) * meshSize;
                float posZ = (v - 0.5f) * meshSize;
                float posY = hm[y, x] * heightScale;

                vertices[i] = new Vector3(posX, posY, posZ);
                uvs[i]      = new Vector2(u, v);
            }
        }

        int quadCount = (resX - 1) * (resY - 1);
        int[] tris = new int[quadCount * 6];
        int t = 0;
        for (int y = 0; y < resY - 1; y++)
        {
            for (int x = 0; x < resX - 1; x++)
            {
                int i = y * resX + x;
                tris[t++] = i;
                tris[t++] = i + resX;
                tris[t++] = i + resX + 1;
                tris[t++] = i;
                tris[t++] = i + resX + 1;
                tris[t++] = i + 1;
            }
        }

        mesh.vertices  = vertices;
        mesh.triangles = tris;
        mesh.uv        = uvs;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
        if (meshMaterial != null)
        {
            GetComponent<MeshRenderer>().material = meshMaterial;
        }

        Debug.Log($"[HeightmapMeshGenerator] Mesh ({resX}×{resY}) created at {meshOrigin}.");
    }

    void LoadBiomeMap()
    {
        biomeMap = Resources.Load<Texture2D>(biomeMapResourcePath);
        if (biomeMap == null)
        {
            Debug.LogError($"Could not load biome map at Resources/{biomeMapResourcePath}.png");
        }
    }

    void SpawnTreesWithSpacing()
    {
        if (treePrefab == null || biomeMap == null)
            return;

        int width  = biomeMap.width;
        int height = biomeMap.height;

        for (int y = 0; y < height; y += spacing)
        {
            for (int x = 0; x < width; x += spacing)
            {
                Color c = biomeMap.GetPixel(x, y);
                if (!ColorsApproxEqual(c, treeColor, colorTolerance))
                    continue;

                float u = (float)x / (width  - 1);
                float v = (float)y / (height - 1);
                float worldX = meshOrigin.x + (u - 0.5f) * meshSize;
                float worldZ = meshOrigin.z + (v - 0.5f) * meshSize;

                MeshSpawnUtils.SpawnPrefabOnMesh(
                    treePrefab,
                    new Vector3(worldX, 0f, worldZ),
                    transform
                );
            }
        }

        Debug.Log($"[HeightmapMeshGenerator] Trees spawned with spacing {spacing}.");
    }

    bool ColorsApproxEqual(Color a, Color b, float tol)
    {
        return Mathf.Abs(a.r - b.r) < tol
            && Mathf.Abs(a.g - b.g) < tol
            && Mathf.Abs(a.b - b.b) < tol;
    }

    /// <summary>
    /// Expose the underlying heightmap for utilities.
    /// </summary>
    public float[,] GetHeightmap()
    {
        return heightGen.GetHeightmap();
    }
}