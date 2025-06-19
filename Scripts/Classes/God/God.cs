using UnityEngine;
using System.Collections.Generic;

public class God : MonoBehaviour
{
    [Header("Player Info")]
    public int playerid;
    public string username;

    [Header("Player Stats")]
    public int points;
    public float pointsincome;

    [Header("Player Abillities")]
    public List<GodPower> abilities = new();

    public virtual void DisplayInfo()
    {
        Debug.Log($"PlayerID: {playerid}, Username: {username}");
    }

    public virtual void DisplayPoints()
    {
        Debug.Log($"Username: {username}, Points: {points}");
    }

    public virtual void Eat()
    {
        Debug.Log($"{username} eats.");
    }

    public virtual void Kill()
    {
        Debug.Log($"{username} dies.");
    }

    public virtual void JoinGame()
    {
        Debug.Log($"{username} joins the game.");
    }

    void Awake()
    {
        JoinGame(); // 🔥 Automatically called on initialization
        DisplayPoints();
    }

    void Update()
    {
        // Optional: future input handling
    }
}