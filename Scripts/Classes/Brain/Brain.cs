using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Brain that schedules thoughts with priority, aging, and time slicing.
/// Triggers associated behaviours defined in the Thought.
/// </summary>
public class Brain : MonoBehaviour
{
    private List<QueuedThought> thoughtQueue = new();
    private bool processing = false;

    [Header("Scheduler Settings")]
    public float timeSlice = 1.0f; // Simulated CPU slice
    public int maxPriority = 100;

    [Header("Debug")]
    public bool debugLogging = false;

    public void EnqueueThought(Thought thought)
    {
        QueuedThought incoming = new QueuedThought(thought);

        if (thoughtQueue.Contains(incoming))
        {
            if (debugLogging)
            {
                Debug.Log($"{name}: Thought '{thought.description}' is already in the queue. Skipping.");
            }
            return;
        }

        thoughtQueue.Add(incoming);

        if (!processing)
            StartCoroutine(ProcessThoughts());
    }

    private IEnumerator ProcessThoughts()
    {
        processing = true;

        while (thoughtQueue.Count > 0)
        {
            QueuedThought next = GetHighestEffectivePriorityThought();
            if (next == null) break;

            if (debugLogging)
            {
                Debug.Log($"{name} is thinking: {next.thought.description} " +
                          $"(Priority: {next.thought.priority}, Effective: {next.EffectivePriority}, Remaining: {next.remainingTime:F2}s)");
            }

            TryActivateBehaviour(next.thought.behaviourKey);

            float slice = Mathf.Min(timeSlice, next.remainingTime);
            yield return new WaitForSeconds(slice);
            next.remainingTime -= slice;

            if (next.remainingTime <= 0)
            {
                thoughtQueue.Remove(next);
                if (debugLogging)
                {
                    Debug.Log($"{name} finished thinking: {next.thought.description}");
                }
            }
        }

        processing = false;
    }

    private QueuedThought GetHighestEffectivePriorityThought()
    {
        QueuedThought best = null;
        int bestPriority = -1;

        foreach (var qt in thoughtQueue)
        {
            int effective = qt.EffectivePriority;
            if (effective > bestPriority)
            {
                best = qt;
                bestPriority = effective;
            }
        }

        return best;
    }

    private void TryActivateBehaviour(string behaviourKey)
    {
        if (string.IsNullOrEmpty(behaviourKey))
            return;

        var type = System.Type.GetType(behaviourKey);
        if (type == null)
        {
            Debug.LogWarning($"{name}: Could not resolve type '{behaviourKey}'. Ensure it's fully qualified.");
            return;
        }

        var found = GetComponent(type);
        if (found is CreatureBehaviour behaviour)
        {
            behaviour.StartBehaviour();
            if (debugLogging)
                Debug.Log($"{name} activated behaviour: {behaviourKey}");
        }
        else
        {
            Debug.LogWarning($"{name} has no valid behaviour '{behaviourKey}' or it doesn't inherit CreatureBehaviour.");
        }
    }
}