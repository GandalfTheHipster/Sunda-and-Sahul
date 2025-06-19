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

    [Header("Player Abilities")]
    [Tooltip("Raw power IDs read from JSON")]
    public int[] powerids;               // ← Make sure this line is present

    [Tooltip("Resolved GodPower components")]
    public List<GodPower> abilities = new List<GodPower>();

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
}