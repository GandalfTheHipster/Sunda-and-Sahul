using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CreatureHungerBehaviour : MonoBehaviour
{
    private Creature creature;
    private NavMeshAgent agent;
    private GameObject targetBush;

    public float hungerThreshold = 50f;
    public float eatDistance = 2f;

    private bool isSeekingFood = false;

    void Awake()
    {
        creature = GetComponent<Creature>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (creature.stomach < hungerThreshold && !isSeekingFood)
        {
            FindAndSeekFood();
        }

        if (isSeekingFood && targetBush != null)
        {
            float distance = Vector3.Distance(transform.position, targetBush.transform.position);
            if (distance <= eatDistance)
            {
                agent.isStopped = true;
                creature.Eat();
                isSeekingFood = false;
                targetBush = null;
            }
        }
    }

    void FindAndSeekFood()
    {
        GameObject[] bushes = GameObject.FindGameObjectsWithTag("FoodBush");
        if (bushes.Length == 0)
        {
            Debug.LogWarning("No FoodBush found in scene!");
            return;
        }

        // Find nearest bush
        float closestDistance = float.MaxValue;
        GameObject closestBush = null;

        foreach (var bush in bushes)
        {
            float dist = Vector3.Distance(transform.position, bush.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestBush = bush;
            }
        }

        if (closestBush != null)
        {
            targetBush = closestBush;
            agent.SetDestination(targetBush.transform.position);
            agent.isStopped = false;
            isSeekingFood = true;

            // 🧠 If this is a Human, send a hunger thought
            Human human = creature as Human;
            if (human != null)
            {
                human.SendThought("I need to find food...");
            }
        }
    }
}