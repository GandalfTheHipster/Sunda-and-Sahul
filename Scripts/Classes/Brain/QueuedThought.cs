using UnityEngine;

/// <summary>
/// Wraps a Thought with metadata needed for scheduling, such as remaining time and effective priority.
/// </summary>
public class QueuedThought
{
    public Thought thought;
    public float enqueuedTime;
    public float remainingTime;

    public QueuedThought(Thought t)
    {
        thought = t;
        enqueuedTime = Time.time;
        remainingTime = t.duration;
    }

    public int EffectivePriority => thought.priority + Mathf.FloorToInt(Time.time - enqueuedTime);

    public override string ToString()
    {
        return $"[{thought.description}] Priority: {thought.priority}, Effective: {EffectivePriority}, Remaining: {remainingTime:F2}s";
    }

    public override bool Equals(object obj)
    {
        if (obj is not QueuedThought other) return false;
        return thought.description == other.thought.description &&
               thought.priority == other.thought.priority &&
               thought.behaviourKey == other.thought.behaviourKey;
    }

    public override int GetHashCode()
    {
        return (thought.description, thought.priority, thought.behaviourKey).GetHashCode();
    }
}