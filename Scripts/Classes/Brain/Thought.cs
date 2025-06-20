using UnityEngine;

[System.Serializable]
public class Thought
{
    public string description;
    public float duration;
    public int priority;
    public string behaviourKey; // NEW: For dynamic behaviour activation

    public Thought(string description, float duration, int priority, string behaviourKey = null)
    {
        this.description = description;
        this.duration = duration;
        this.priority = priority;
        this.behaviourKey = behaviourKey;
    }

    public Thought() {}
}