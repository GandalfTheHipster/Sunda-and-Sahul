using UnityEngine;

/// <summary>
/// Holds the current game state, including username, time, and points.
/// </summary>
public class GameState : MonoBehaviour
{
    [Header("Player Info")]
    public string username = "Unknown";

    [Header("Game Progress")]
    public float inGameTime = 0f;  // Time in seconds
    public int userPoints = 0;

    /// <summary>
    /// Call this method to initialize the game state.
    /// </summary>
    public void Initialize(string user)
    {
        username = user;
        inGameTime = 0f;
        userPoints = 0;
    }

    /// <summary>
    /// Adds points to the user's score.
    /// </summary>
    public void AddPoints(int points)
    {
        userPoints += points;
    }

    /// <summary>
    /// Updates the in-game time.
    /// </summary>
    private void Update()
    {
        inGameTime += Time.deltaTime;
    }

    /// <summary>
    /// Display the current state (for debugging).
    /// </summary>
    public void PrintState()
    {
        Debug.Log($"User: {username} | Time: {inGameTime:F2}s | Points: {userPoints}");
    }
}