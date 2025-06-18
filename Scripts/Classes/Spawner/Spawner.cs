using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Creature Info")]
    public int spawnerid;

    // You can later override or extend this
    public virtual void DisplayInfo()
    {
        Debug.Log($"Creature EntityID: {spawnerid}, Age: {spawnerid}");
    }

    public void Thought()
    {
        Debug.Log($"{spawnerid} just thought about x!");
    }
}