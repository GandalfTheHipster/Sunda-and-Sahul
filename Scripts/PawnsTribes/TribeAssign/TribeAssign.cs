using UnityEngine;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Re-applies member assignments for each Tribe in the scene based on
/// the same JSON files used by TribeLoader. This syncs Tribe membership
/// to match the "memberids" field exactly.
/// </summary>
public class TribeAssign : MonoBehaviour
{
    [Header("Assignment Settings")]
    [Tooltip("Folder under StreamingAssets/Savegame/Tribes to read tribe files from.")]
    public string tribeDataFolder = "Savegame/Tribes";

    public void AssignAll()
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, tribeDataFolder);
        if (!Directory.Exists(fullPath))
        {
            Debug.LogError($"[TribeAssign] Tribe folder not found: {fullPath}");
            return;
        }

        string[] files = Directory.GetFiles(fullPath, "*.json");
        if (files.Length == 0)
        {
            Debug.LogWarning("[TribeAssign] No tribe files found to apply assignments.");
        }

        foreach (string file in files)
        {
            try
            {
                string json = File.ReadAllText(file);
                TribeDefinition def = JsonUtility.FromJson<TribeDefinition>(json);
                if (def == null)
                {
                    Debug.LogError($"[TribeAssign] Failed to parse file: {file}");
                    continue;
                }

                Tribe target = FindTribe(def.tribeid);
                if (target == null)
                {
                    Debug.LogWarning($"[TribeAssign] No Tribe with ID {def.tribeid} found in scene.");
                    continue;
                }

                HashSet<int> desired = new HashSet<int>(def.memberids ?? new List<int>());
                HashSet<int> current = new HashSet<int>();

                foreach (Creature c in target.members)
                    current.Add(c.entityid);

                // Add missing
                foreach (int id in desired)
                {
                    if (!current.Contains(id))
                        target.AddMember(id);
                }

                // Remove excess
                foreach (int id in current)
                {
                    if (!desired.Contains(id))
                        target.RemoveMember(id);
                }

                Debug.Log($"[TribeAssign] Synced Tribe {def.tribeid} to {desired.Count} members.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[TribeAssign] Error in file '{file}': {ex.Message}");
            }
        }
    }

    private Tribe FindTribe(int tribeid)
    {
        foreach (Tribe t in FindObjectsOfType<Tribe>())
        {
            if (t.tribeid == tribeid)
                return t;
        }
        return null;
    }

    [System.Serializable]
    private class TribeDefinition
    {
        public int tribeid;
        public int groupid;
        public List<int> memberids;
    }
}