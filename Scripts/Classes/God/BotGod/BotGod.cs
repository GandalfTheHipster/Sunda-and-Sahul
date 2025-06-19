using UnityEngine;

public class BotGod : God
{
    public void TakeTurn()
    {
        // Simple decision logic
        Debug.Log($"{username} (Bot) simulates turn.");
    }

    void Update()
    {

    }
}