#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public static class PerlinHeightmapExporter
{
    private const string outputFolderRelative = "Assets/Scripts/Map/Output";
    private const string fileName = "perlin_heightmap.png";

    [MenuItem("Tools/Map/Export Perlin Heightmap")]
    public static void ExportHeightmap()
    {
        // 1. Ensure output folder exists
        string absoluteOutput = Path.Combine(Application.dataPath, "Scripts/Map/Output");
        if (!Directory.Exists(absoluteOutput))
            Directory.CreateDirectory(absoluteOutput);

        // 2. Create a temporary generator in the scene
        GameObject tempGO = new GameObject("__PerlinGenerator_Temp__");
        var generator = tempGO.AddComponent<PerlinHeightmapGenerator>();

        // (Optionally tweak these parameters before Generate())
        generator.resolution = 256;
        generator.scale = 20f;
        generator.offsetX = Random.Range(0f, 1000f);
        generator.offsetY = Random.Range(0f, 1000f);

        // 3. Generate the heightmap & grab the texture
        generator.Generate();
        Texture2D tex = generator.GetHeightmapTexture();

        // 4. Encode to PNG and write it
        byte[] png = tex.EncodeToPNG();
        string fullPath = Path.Combine(absoluteOutput, fileName);
        File.WriteAllBytes(fullPath, png);
        Debug.Log($"[PerlinHeightmapExporter] Wrote PNG to {fullPath}");

        // 5. Tell Unity to import the new asset
        AssetDatabase.Refresh();

        // 6. Clean up
        Object.DestroyImmediate(tempGO);
    }
}
#endif
