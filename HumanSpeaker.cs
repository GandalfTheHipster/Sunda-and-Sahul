using UnityEngine;

public class HumanSpeaker : MonoBehaviour
{
    void Start()
    {
        // Find all objects with a Human component in the scene
        Human[] allHumans = FindObjectsOfType<Human>();

        // Call Speak on each
        foreach (Human human in allHumans)
        {
            human.Speak();
        }
    }
}