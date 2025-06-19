using UnityEngine;
using TMPro;

public class DisplayGodPoints : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI pointsText;

    [Header("Target Player")]
    public string playerObjectName = "God_2_Goldenoah";

    private God playerGod;

    void Start()
    {
        GameObject playerObj = GameObject.Find(playerObjectName);

        if (playerObj == null)
        {
            Debug.LogError($"[DisplayGodPoints] No GameObject named '{playerObjectName}' found.");
            return;
        }

        playerGod = playerObj.GetComponent<God>();

        if (playerGod == null)
        {
            Debug.LogError($"[DisplayGodPoints] '{playerObjectName}' does not have a God component.");
            return;
        }
    }

    void Update()
    {
        if (playerGod != null && pointsText != null)
        {
            pointsText.text = $"{playerGod.points}";
        }
    }
}
