using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static registry of all thought templates. Provides reusable instances.
/// </summary>
public static class ThoughtLibrary
{
    private static Dictionary<string, Thought> thoughts = new();

    static ThoughtLibrary()
    {
        // Use full type name if namespaced, otherwise class name is fine
        thoughts["danger"] = new Thought("Avoid danger", 1.5f, 80);
        thoughts["hunger"] = new Thought("Seek food", 2f, 40, "CreatureHungerBehaviour");
        thoughts["curiosity"] = new Thought("Explore surroundings", 3f, 30);
        thoughts["social"] = new Thought("Interact with others", 2f, 20);
        thoughts["rest"] = new Thought("Take a nap", 4f, 10);
    }

    public static Thought Get(string key)
    {
        if (thoughts.ContainsKey(key))
        {
            Thought baseThought = thoughts[key];
            return new Thought
            {
                description = baseThought.description,
                duration = baseThought.duration,
                priority = baseThought.priority,
                behaviourKey = baseThought.behaviourKey
            };
        }

        Debug.LogError($"ThoughtLibrary: No thought registered with key '{key}'.");
        return null;
    }

    public static List<string> GetAllKeys()
    {
        return new List<string>(thoughts.Keys);
    }
}