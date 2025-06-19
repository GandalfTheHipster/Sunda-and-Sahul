using UnityEngine;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Generates a heightmap and a separate biome map via Perlin noise,
/// and provides a context menu to export both as PNGs into Assets/BiomesOutputs.
/// </summary>
[ExecuteAlways]
public class MultiNoiseBiomeGenerator : MonoBehaviour
{
    [Header("Common Settings")]
    public int resolution = 256;

    [Header("Heightmap Settings")]
    public float heightNoiseScale = 20f;
    public Vector2 heightOffset = Vector2.zero;

    [Header("Biome Noise Settings")]
    public float climateNoiseScale = 40f;
    public Vector2 climateOffset = new Vector2(1000, 0);
    public float detailNoiseScale = 5f;
    public Vector2 detailOffset = new Vector2(0, 1000);
    [Range(0, 1)] public float warpStrength = 0.2f;
    [Range(0, 1)] public float treeThreshold = 0.6f;

    [Header("Preview Materials (optional)")]
    public Material heightPreviewMat;
    public Material biomePreviewMat;

    private Texture2D heightTexture;
    private Texture2D biomeTexture;

    void Start()
    {
        BuildMaps();
    }

    [ContextMenu("Build Maps")]  
    public void BuildMaps()
    {
        heightTexture = new Texture2D(resolution, resolution, TextureFormat.RFloat, false);
        biomeTexture  = new Texture2D(resolution, resolution, TextureFormat.RGBA32, false);

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                // Height
                float h = Mathf.PerlinNoise((x + heightOffset.x) / heightNoiseScale,
                                            (y + heightOffset.y) / heightNoiseScale);

                // Climate + detail + warp
                float climate = Mathf.PerlinNoise((x + climateOffset.x) / climateNoiseScale,
                                                  (y + climateOffset.y) / climateNoiseScale);
                float detail  = Mathf.PerlinNoise((x + detailOffset.x) / detailNoiseScale,
                                                  (y + detailOffset.y) / detailNoiseScale);
                float warped  = Mathf.PerlinNoise((x + climateOffset.x) / climateNoiseScale + (detail - 0.5f) * warpStrength,
                                                  (y + climateOffset.y) / climateNoiseScale + (detail - 0.5f) * warpStrength);

                Color biomeCol = (warped > treeThreshold)
                    ? new Color(0.2f, 0.8f, 0.2f, 1f)
                    : new Color(0.5f, 0.5f, 0.5f, 1f);

                heightTexture.SetPixel(x, y, new Color(h, h, h, 1));
                biomeTexture.SetPixel(x, y, biomeCol);
            }
        }

        heightTexture.Apply();
        biomeTexture.Apply();

        if (heightPreviewMat != null)
            heightPreviewMat.mainTexture = heightTexture;
        if (biomePreviewMat != null)
            biomePreviewMat.mainTexture  = biomeTexture;

        Debug.Log($"[MultiNoiseBiomeGenerator] Generated {resolution}×{resolution} maps.");
    }

    /// <summary>
    /// Saves both textures as PNGs into Assets/BiomesOutputs.
    /// </summary>
    [ContextMenu("Export Maps to Disk")]
    public void ExportMapsToDisk()
    {
#if UNITY_EDITOR
        if (heightTexture == null || biomeTexture == null)
        {
            Debug.LogWarning("Textures not generated yet. Building maps first.");
            BuildMaps();
        }

        string folder = Path.Combine(Application.dataPath, "BiomesOutputs");
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        // Height PNG
        byte[] hBytes = heightTexture.EncodeToPNG();
        string hPath = Path.Combine(folder, "heightmap.png");
        File.WriteAllBytes(hPath, hBytes);

        // Biome PNG
        byte[] bBytes = biomeTexture.EncodeToPNG();
        string bPath = Path.Combine(folder, "biomemap.png");
        File.WriteAllBytes(bPath, bBytes);

        Debug.Log($"[MultiNoiseBiomeGenerator] Exported maps to {folder}");
        AssetDatabase.Refresh();
#else
        Debug.LogError("ExportMapsToDisk is only available in the Unity Editor.");
#endif
    }

    // Optional getters
    public Texture2D GetHeightTexture() => heightTexture;
    public Texture2D GetBiomeTexture()  => biomeTexture;
}