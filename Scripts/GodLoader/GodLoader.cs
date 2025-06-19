using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Loads God player data from StreamingAssets/Savegame/Gods and instantiates the appropriate prefab.
/// </summary>
public class GodLoader : MonoBehaviour
{
    [Header("God Prefabs")]
    public string prefabFolder = "Prefabs/God/Types";
    public string humanPrefabName = "Human";
    public string botPrefabName = "Bot";

    private string godsPath;

    [System.Serializable]
    private class GodData
    {
        public int playerid;
        public string username;
        public int points;
        public float pointsincome;
        public bool human;
        public float x, y, z;
    }

    void Start()
    {
        godsPath = Path.Combine(Application.streamingAssetsPath, "Savegame/Gods");

        if (!Directory.Exists(godsPath))
        {
            Debug.LogWarning($"[GodLoader] Gods folder not found: {godsPath}");
            return;
        }

        string[] files = Directory.GetFiles(godsPath, "god_*.json");
        if (files.Length == 0)
        {
            Debug.LogWarning("[GodLoader] No god_*.json files found.");
            return;
        }

        foreach (string file in files)
        {
            try
            {
                string json = File.ReadAllText(file);
                GodData data = JsonUtility.FromJson<GodData>(json);

                if (data == null)
                {
                    Debug.LogError($"[GodLoader] Failed to parse JSON in {file}");
                    continue;
                }

                string prefabName = data.human ? humanPrefabName : botPrefabName;
                string fullPath = $"{prefabFolder}/{prefabName}";
                GameObject prefab = Resources.Load<GameObject>(fullPath);

                if (prefab == null)
                {
                    Debug.LogError($"[GodLoader] Prefab not found: {fullPath}");
                    continue;
                }

                Vector3 position = new Vector3(data.x, data.y, data.z);
                GameObject instance = Instantiate(prefab, position, Quaternion.identity);
                instance.name = $"God_{data.playerid}_{data.username}";

                God godComponent = instance.GetComponent<God>();
                if (godComponent != null)
                {
                    godComponent.playerid = data.playerid;
                    godComponent.username = data.username;
                    godComponent.points = data.points;
                    godComponent.pointsincome = data.pointsincome;
                    Debug.Log($"[GodLoader] Spawned {(data.human ? "Player" : "Bot")} {data.username} at {position}");
                }
                else
                {
                    Debug.LogWarning($"[GodLoader] No God script found on prefab: {prefabName}");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[GodLoader] Error reading {file}: {ex.Message}");
            }
        }
    }
}