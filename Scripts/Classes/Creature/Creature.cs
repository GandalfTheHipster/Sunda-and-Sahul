using UnityEngine;

public class Creature : MonoBehaviour
{
    [Header("Creature Info")]
    public int entityid;
    public int age;

    [Header("Vitals")]
    public int health = 100;
    public float stomach = 100;
    public float hungerRate = 0.1f;

    [Header("Stats")]
    public int speed;
    public int strength;
    public int swimming;

    private Brain brain;

    // Prevents repeated hunger thoughts
    private bool hasTriggeredHungerThought = false;

    public virtual void DisplayInfo()
    {
        Debug.Log($"Creature EntityID: {entityid}, Age: {age}");
    }

    public virtual void Eat()
    {
        Debug.Log($"{entityid} eats.");
        stomach += 10;

        // Reset hunger thought flag once fed
        if (stomach > 50)
        {
            hasTriggeredHungerThought = false;
        }
    }

    public virtual void Kill()
    {
        Debug.Log($"{entityid} dies.");
    }

    public void Thought()
    {
        Debug.Log($"{entityid} just thought about x!");
    }

    public virtual void isHungry()
    {
        Debug.Log($"{entityid} is hungry");
    }

    void Awake()
    {
        if (GetComponent<Brain>() == null)
        {
            gameObject.AddComponent<Brain>();
        }

        stomach = 100;
        brain = GetComponent<Brain>();
    }

    void Update()
    {
        if (stomach > 0)
        {
            stomach -= hungerRate * Time.deltaTime;

            if (stomach < 50 && !hasTriggeredHungerThought)
            {
                Thought hungerThought = ThoughtLibrary.Get("hunger");
                if (hungerThought != null && brain != null)
                {
                    brain.EnqueueThought(hungerThought);
                    hasTriggeredHungerThought = true;
                }
            }
        }
        else
        {
            stomach = 0;
            Debug.Log($"{entityid} is starving!");
        }
    }
}