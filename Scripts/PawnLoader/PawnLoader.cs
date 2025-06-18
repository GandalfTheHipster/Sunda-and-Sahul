using System.IO;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        // Path to StreamingAssets/Savegame/Pawns
        savePath = Path.Combine(Application.streamingAssetsPath, "Savegame/Pawns");

        if (!Directory.Exists(savePath))
        {
            Debug.LogWarning($"Savegame directory not found at: {savePath}");
            return;
        }

        string[] files = Directory.GetFiles(savePath, "*.json");

        foreach (string file in files)
        {
            string json = File.ReadAllText(file);
            PawnData data = JsonUtility.FromJson<PawnData>(json);
            SpawnPawn(data);
        }
    }

    void SpawnPawn(PawnData data)
    {
        string prefabPath = $"Prefabs/Human/Species/{data.species}";
        GameObject prefab = Resources.Load<GameObject>(prefabPath);

        if (prefab == null)
        {
            Debug.LogError($"Prefab not found at: {prefabPath}");
            return;
        }

        Vector3 position = new Vector3(data.x, data.y, data.z);
        GameObject instance = Instantiate(prefab, position, Quaternion.identity);

        // Name the GameObject after the human
        instance.name = $"[{data.humanid}] {data.firstname} {data.lastname}";

        // Populate all base Human fields, regardless of species
        Human human = instance.GetComponent<Human>();
        if (human != null)
        {
            human.entityid = data.entityid;
            human.humanid = data.humanid;
            human.firstname = data.firstname;
            human.middlename = data.middlename;
            human.lastname = data.lastname;
            human.tribeid = data.tribeid;
            human.health = (int)data.health;
            human.age = data.age;
            human.stomach = data.stomach;

            Debug.Log($"Spawned {data.species}: {data.firstname} at {position}");
        }
        else
        {
            Debug.LogWarning($"Spawned {data.species}, but no Human script was found.");
        }
    }
}