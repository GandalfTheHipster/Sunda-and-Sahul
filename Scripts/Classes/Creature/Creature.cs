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

    // You can later override or extend this
    public virtual void DisplayInfo()
    {
        Debug.Log($"Creature EntityID: {entityid}, Age: {age}");
    }

    public virtual void Eat()
    {
        Debug.Log($"{entityid} eats.");
        stomach += 10;
    }

    public virtual void Kill()
    {
        Debug.Log($"{entityid} dies.");
    }

    public void Thought()
    {
        age++;
        Debug.Log($"{entityid} just thought about x!");
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
        }
        else
        {
            stomach = 0;
            Debug.Log($"{entityid} is starving!");
        }
    }
}