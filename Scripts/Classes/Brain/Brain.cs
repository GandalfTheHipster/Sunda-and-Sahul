using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    private Queue<Thought> thoughts = new();
    private bool processing = false;

    public void EnqueueThought(Thought thought)
    {
        thoughts.Enqueue(thought);
        if (!processing)
            StartCoroutine(ProcessThoughts());
    }

    private IEnumerator ProcessThoughts()
    {
        processing = true;

        while (thoughts.Count > 0)
        {
            Thought current = thoughts.Dequeue();
            Debug.Log($"{name} is thinking: {current.description}");
            yield return new WaitForSeconds(current.duration);
        }

        processing = false;
    }
}