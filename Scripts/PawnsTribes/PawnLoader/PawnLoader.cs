using System.IO;
using System.Collections;
using UnityEngine;

/// <summary>
/// Loads pawn data from StreamingAssets/Savegame/Pawns,
/// spawns prefabs aligned to terrain mesh using MeshSpawnUtils,
/// and initializes their Human component.
/// </summary>
public class PawnLoader : MonoBehaviour
{
    [System.Serializable]
    public class PawnData
    {
        public int entityid;
        public int humanid;
        public string species; // Prefab name & script type
        public string firstname;
        public string middlename;
        public string lastname;
        public int tribeid;
        public float health;
        public int age;
        public float stomach;
        public float x, y, z;
    }

    private string savePath;

    public IEnumerator LoadAllPawns()
    {
        savePath = Path.Combine(Application.streamingAssetsPath, "Savegame/Pawns");
        if (!Directory.Exists(savePath))
        {
            Debug.LogWarning($"Pawn save directory not found at: {savePath}");
            yield break;
        }

        var files = Directory.GetFiles(savePath, "*.json");
        foreach (var file in files)
        {
            var json = File.ReadAllText(file);
            var data = JsonUtility.FromJson<PawnData>(json);
            SpawnPawn(data);
            yield return null;
        }

        Debug.Log($"Finished loading {files.Length} pawn(s).");
    }

    void SpawnPawn(PawnData data)
    {
        // Load prefab
        string prefabPath = $"Prefabs/Human/Species/{data.species}";
        GameObject prefab = Resources.Load<GameObject>(prefabPath);
        if (prefab == null)
        {
            Debug.LogError($"Prefab not found at: {prefabPath}");
            return;
        }

        // Spawn aligned to terrain
        Vector3 spawnXZ = new Vector3(data.x, 0f, data.z);
        GameObject instance = MeshSpawnUtils.SpawnPrefabOnMesh(prefab, spawnXZ, transform);

        // **No fallback to saved Y anymore**
        if (instance == null)
        {
            Debug.LogError($"Failed to spawn pawn '{data.species}' at XZ=({data.x},{data.z})");
            return;
        }

        instance.name = $"[{data.humanid}] {data.firstname} {data.lastname}";

        // Initialize the Human script
        if (instance.TryGetComponent<Human>(out var human))
        {
            human.entityid  = data.entityid;
            human.humanid   = data.humanid;
            human.firstname = data.firstname;
            human.middlename= data.middlename;
            human.lastname  = data.lastname;
            human.tribeid   = data.tribeid;
            human.health    = (int)data.health;
            human.age       = data.age;
            human.stomach   = data.stomach;

            Debug.Log($"Spawned {data.species}: {data.firstname} at {instance.transform.position}");
        }
        else
        {
            Debug.LogWarning($"Spawned {data.species}, but no Human script was found.");
        }
    }
}