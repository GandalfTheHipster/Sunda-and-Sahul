using UnityEngine;

public class Diprotodon : Animal
{
    public override void Crow()
    {
        Debug.Log($"{animalname} wallows.");
    }

    public void Wallow()
    {
        Debug.Log($"{animalname} wallows.");
    }

        void Awake()
    {
        health = 200; // Set Diprotodon-specific health value
    }
}