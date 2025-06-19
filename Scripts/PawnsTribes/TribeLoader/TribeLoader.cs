using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;

/// <summary>
/// Loads tribe definitions from StreamingAssets/Savegame/Tribes and instantiates Tribe objects.
/// Expects JSON files with .json extension, each containing:
/// {
///   "tribeid": int,
///   "groupid": int,
///   "memberids": [int, int, ...]
/// }
/// </summary>
public class TribeLoader : MonoBehaviour
{
    [Header("Loader Settings")]
    [Tooltip("Relative folder path under StreamingAssets to search for tribe files.")]
    public string tribesFolder = "Savegame/Tribes";

    /// <summary>
    /// Called manually (e.g. from PawnsTribes) to load tribes in a coroutine.
    /// </summary>
    public IEnumerator LoadAllTribes()
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, tribesFolder);
        Debug.Log($"[TribeLoader] Looking for tribes in: {fullPath}");

        if (!Directory.Exists(fullPath))
        {
            Debug.LogError($"[TribeLoader] Tribes folder not found at path: {fullPath}");
            yield break;
        }

        string[] files = Directory.GetFiles(fullPath, "*.json");
        Debug.Log($"[TribeLoader] Found {files.Length} JSON file(s).");

        if (files.Length == 0)
        {
            Debug.LogWarning("[TribeLoader] No tribe files found.");
        }

        foreach (string file in files)
        {
            string filename = Path.GetFileName(file);
            Debug.Log($"[TribeLoader] Attempting to read file: {filename}");

            try
            {
                if (!File.Exists(file))
                {
                    Debug.LogError($"[TribeLoader] File not found: {file}");
                    continue;
                }

                string json = File.ReadAllText(file);

                if (string.IsNullOrWhiteSpace(json))
                {
                    Debug.LogWarning($"[TribeLoader] File {filename} is empty or unreadable.");
                    continue;
                }

                Debug.Log($"[TribeLoader] Raw JSON content from {filename}: {json}");

                TribeDefinition def = JsonUtility.FromJson<TribeDefinition>(json);

                if (def == null)
                {
                    Debug.LogError($"[TribeLoader] Failed to parse JSON from file: {filename}");
                    continue;
                }

                Debug.Log($"[TribeLoader] Deserialized tribeid={def.tribeid}, groupid={def.groupid}, member count={def.memberids?.Count ?? -1}");

                if (def.tribeid == 0 && def.groupid == 0 && (def.memberids == null || def.memberids.Count == 0))
                {
                    Debug.LogWarning($"[TribeLoader] Parsed values from {filename} look like default values. Check your JSON key casing.");
                }

                Tribe created = Tribe.CreateTribe(def.tribeid, def.groupid, def.memberids);

                if (created == null)
                {
                    Debug.LogError($"[TribeLoader] Failed to instantiate Tribe for file: {filename}");
                    continue;
                }

                created.transform.SetParent(transform);
                Debug.Log($"[TribeLoader] Successfully created tribe {def.tribeid} with {def.memberids?.Count ?? 0} member(s).");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[TribeLoader] Exception while processing {file}:\n{ex}");
            }

            yield return null;
        }

        Debug.Log($"[TribeLoader] Finished loading tribe data from {files.Length} file(s).");
    }

    [System.Serializable]
    private class TribeDefinition
    {
        public int tribeid;
        public int groupid;
        public List<int> memberids;
    }
}