using UnityEngine;

public abstract class CreatureBehaviour : MonoBehaviour
{
    protected Creature creature;
    protected bool isActive = false;

    protected virtual void Awake()
    {
        creature = GetComponent<Creature>();
    }

    public virtual void StartBehaviour()
    {
        isActive = true;
    }

    public virtual void StopBehaviour()
    {
        isActive = false;
    }
}