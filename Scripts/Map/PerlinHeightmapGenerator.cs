using UnityEngine;

public class PerlinHeightmapGenerator : MonoBehaviour
{
    [Header("Heightmap Settings")]
    [Tooltip("Resolution of the square heightmap (will be resolution×resolution).")]
    public int resolution = 256;

    [Tooltip("Controls the zoom of the noise. Larger = more stretched out hills.")]
    public float scale = 20f;

    [Tooltip("Random offset on the X axis to vary the noise.")]
    public float offsetX = 0f;

    [Tooltip("Random offset on the Y axis to vary the noise.")]
    public float offsetY = 0f;

    [Header("Preview (optional)")]
    [Tooltip("If you want to preview the generated heightmap, assign a Material here.")]
    public Material previewMaterial;

    // The generated raw height values [0…1]
    private float[,] heightmap;

    // The generated Texture2D (grayscale)
    private Texture2D heightmapTexture;

    void Start()
    {
        Generate();
    }

    /// <summary>
    /// Generates both the float[,] heightmap and the preview texture.
    /// </summary>
    public void Generate()
    {
        // Allocate arrays
        heightmap = new float[resolution, resolution];
        heightmapTexture = new Texture2D(resolution, resolution, TextureFormat.RGB24, false);
        heightmapTexture.wrapMode = TextureWrapMode.Clamp;

        // Fill in Perlin noise data
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                float sampleX = (x + offsetX) / scale;
                float sampleY = (y + offsetY) / scale;
                float noise = Mathf.PerlinNoise(sampleX, sampleY);
                heightmap[y, x] = noise;
                heightmapTexture.SetPixel(x, y, new Color(noise, noise, noise));
            }
        }
        heightmapTexture.Apply();

        // Apply to preview material if provided
        if (previewMaterial != null)
            previewMaterial.mainTexture = heightmapTexture;

        Debug.Log($"[PerlinHeightmapGenerator] Generated {resolution}×{resolution} heightmap.");
    }

    /// <summary>
    /// Exposes the raw height values (0…1).
    /// </summary>
    public float[,] GetHeightmap() => heightmap;

    /// <summary>
    /// Exposes the generated grayscale texture.
    /// </summary>
    public Texture2D GetHeightmapTexture() => heightmapTexture;
}