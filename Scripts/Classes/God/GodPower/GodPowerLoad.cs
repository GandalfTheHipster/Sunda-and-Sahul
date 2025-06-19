using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class GodPowerLoad : MonoBehaviour
{
    [Tooltip("Relative folder in StreamingAssets containing JSON files (e.g., 'GodPowers').")]
    public string jsonFolder = "GodPowers";

    // Holds loaded GodPower instances
    public List<GodPower> loadedPowers = new List<GodPower>();

    void Start()
    {
        LoadAndInstantiatePowers();
    }

    /// <summary>
    /// Reads all JSON files in the specified folder, deserializes them,
    /// and creates GameObjects with the specified Power script attached.
    /// </summary>
    public void LoadAndInstantiatePowers()
    {
        string folderPath = Path.Combine(Application.streamingAssetsPath, jsonFolder);
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError($"[GodPowerLoad] Folder not found: {folderPath}");
            return;
        }

        foreach (var file in Directory.GetFiles(folderPath, "*.json"))
        {
            try
            {
                string json = File.ReadAllText(file);
                GodPowerData data = JsonUtility.FromJson<GodPowerData>(json);
                InstantiateGodPower(data);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[GodPowerLoad] Failed to load '{file}': {ex.Message}");
            }
        }
    }

    void InstantiateGodPower(GodPowerData data)
    {
        // Create the GameObject
        var go = new GameObject($"GodPower_{data.powerid}_{data.powername}");
        go.transform.SetParent(transform);

        // Try to find your specified script type
        Type componentType = FindTypeByName(data.script);
        if (componentType == null || !typeof(MonoBehaviour).IsAssignableFrom(componentType))
        {
            Debug.LogError($"[GodPowerLoad] Script '{data.script}' not found or not a MonoBehaviour. Falling back to GodPower.");
            componentType = typeof(GodPower);
        }

        // Add the component dynamically
        var comp = go.AddComponent(componentType) as GodPower;
        if (comp == null)
        {
            Debug.LogError($"[GodPowerLoad] Failed to add component of type {componentType}. Ensure it inherits GodPower.");
            return;
        }

        // Populate the fields
        comp.powerid   = data.powerid;
        comp.powername = data.powername;
        comp.damage    = data.damage;

        loadedPowers.Add(comp);
        Debug.Log($"[GodPowerLoad] Instantiated '{comp.powername}' using script '{componentType.Name}'");
    }

    /// <summary>
    /// Searches all loaded assemblies for a type matching the provided name.
    /// </summary>
    Type FindTypeByName(string typeName)
    {
        // First try the simple lookup
        var t = Type.GetType(typeName);
        if (t != null) return t;

        // Otherwise search every assembly
        return AppDomain.CurrentDomain.GetAssemblies()
            .Select(a => a.GetType(typeName))
            .FirstOrDefault(x => x != null);
    }
}

/// <summary>
/// Matches the JSON structure (including your new "script" field).
/// </summary>
[Serializable]
public class GodPowerData
{
    public int    powerid;
    public string powername;
    public int    damage;
    [Tooltip("Name of the MonoBehaviour class to attach (e.g. 'FireballPower').")]
    public string script;
}