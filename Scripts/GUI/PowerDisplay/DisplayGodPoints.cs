using UnityEngine;
using TMPro;

public class DisplayGodPoints : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI pointsText;

    [Header("Game State Reference")]
    public string gameStateObjectName = "GameState";  // This must match your GameObject name

    private GameState gameState;

    void Start()
    {
        GameObject gsObj = GameObject.Find(gameStateObjectName);

        if (gsObj == null)
        {
            Debug.LogError($"[DisplayGodPoints] No GameObject named '{gameStateObjectName}' found.");
            return;
        }

        gameState = gsObj.GetComponent<GameState>();

        if (gameState == null)
        {
            Debug.LogError($"[DisplayGodPoints] GameObject '{gameStateObjectName}' does not have a GameState component.");
        }
    }

    void Update()
    {
        if (gameState != null && pointsText != null)
        {
            pointsText.text = $"{gameState.userPoints}";
        }
    }
}