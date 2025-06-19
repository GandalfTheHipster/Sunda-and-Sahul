using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class GodLoader : MonoBehaviour
{
    [Header("God Prefabs")]
    public string prefabFolder        = "Prefabs/God/Types";
    public string humanPrefabName     = "Human";
    public string botPrefabName       = "Bot";

    [Header("Camera Prefabs")]
    [Tooltip("Relative Resources folder containing camera prefabs (e.g. 'Prefabs/God/Cameras').")]
    public string cameraPrefabFolder      = "Prefabs/God/Cameras";
    [Tooltip("Name of the camera prefab to use if JSON does not specify one.")]
    public string defaultCameraPrefabName = "DefaultCamera";

    private string godsPath;

    [System.Serializable]
    private class GodData
    {
        public int    playerid;
        public string username;
        public int    points;
        public float  pointsincome;
        public bool   human;
        public float  x, y, z;

        // ← newly added to choose camera
        public string cameraPrefabName;

        public int[]  powerids;
    }

    void Start()
    {
        godsPath = Path.Combine(Application.streamingAssetsPath, "Savegame/Gods");
        if (!Directory.Exists(godsPath))
        {
            Debug.LogWarning($"[GodLoader] Gods folder not found: {godsPath}");
            return;
        }

        foreach (string file in Directory.GetFiles(godsPath, "god_*.json"))
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

                // 1) Instantiate the God prefab (Human or Bot)
                string prefabName = data.human ? humanPrefabName : botPrefabName;
                string fullPath   = $"{prefabFolder}/{prefabName}";
                GameObject godPrefab = Resources.Load<GameObject>(fullPath);
                if (godPrefab == null)
                {
                    Debug.LogError($"[GodLoader] Prefab not found: {fullPath}");
                    continue;
                }

                Vector3 spawnPos = new Vector3(data.x, data.y, data.z);
                GameObject godInstance = Instantiate(godPrefab, spawnPos, Quaternion.identity);
                godInstance.name = $"God_{data.playerid}_{data.username}";

                // 2) Configure its God component
                God godComp = godInstance.GetComponent<God>();
                if (godComp == null)
                {
                    Debug.LogWarning($"[GodLoader] No God script on prefab {prefabName}");
                }
                else
                {
                    godComp.playerid     = data.playerid;
                    godComp.username     = data.username;
                    godComp.points       = data.points;
                    godComp.pointsincome = data.pointsincome;
                    godComp.powerids     = data.powerids;

                    // resolve powers
                    GodPower[] allPowers = FindObjectsOfType<GodPower>();
                    foreach (int id in data.powerids)
                    {
                        var match = System.Array.Find(allPowers, p => p.powerid == id);
                        if (match != null)
                            GodPower.AddPower(godComp.abilities, match);
                        else
                            Debug.LogWarning($"[GodLoader] No GodPower with ID {id} for {data.username}");
                    }
                }

                // 3) Instantiate & attach the chosen camera
                string camName = string.IsNullOrEmpty(data.cameraPrefabName)
                                 ? defaultCameraPrefabName
                                 : data.cameraPrefabName;

                string camPath = $"{cameraPrefabFolder}/{camName}";
                GameObject camPrefab = Resources.Load<GameObject>(camPath);
                if (camPrefab != null)
                {
                    var camInstance = Instantiate(camPrefab, godInstance.transform);
                    // Optionally position/rotate the camera relative to the god:
                    camInstance.transform.localPosition = Vector3.zero;
                    camInstance.transform.localRotation = Quaternion.identity;
                }
                else
                {
                    Debug.LogWarning($"[GodLoader] Camera prefab not found: {camPath}");
                }

                Debug.Log($"[GodLoader] Spawned {(data.human ? "Player" : "Bot")} {data.username} with camera '{camName}'");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[GodLoader] Error reading {file}: {ex.Message}");
            }
        }
    }
}