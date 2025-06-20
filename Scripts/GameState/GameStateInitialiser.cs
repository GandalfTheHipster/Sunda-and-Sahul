using UnityEngine;

public class GameStateInitialiser : MonoBehaviour
{
    [Header("Initial Values")]
    public string initialUsername = "Goldenoah";
    public float initialTime = 0f;
    public int initialPoints = 100;

    void Start()
    {
        // Find or create the GameState GameObject
        GameObject gameStateObj = GameObject.Find("GameState");
        if (gameStateObj == null)
        {
            gameStateObj = new GameObject("GameState");
        }

        // Get or add GameState component
        GameState gameState = gameStateObj.GetComponent<GameState>();
        if (gameState == null)
        {
            gameState = gameStateObj.AddComponent<GameState>();
        }

        // Set values
        gameState.username = initialUsername;
        gameState.inGameTime = initialTime;
        gameState.userPoints = initialPoints;

        Debug.Log($"[GameStateInitialiser] Initialized with username={initialUsername}, time={initialTime}, points={initialPoints}");
    }
}