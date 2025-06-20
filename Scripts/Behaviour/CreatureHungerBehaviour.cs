using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CreatureHungerBehaviour : CreatureBehaviour
{
    private NavMeshAgent agent;
    private GameObject targetBush;

    public float hungerThreshold = 50f;
    public float eatDistance = 2f;
    private bool isSeekingFood = false;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!isActive) return;

        if (creature.stomach < hungerThreshold && !isSeekingFood)
        {
            creature.isHungry();
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

    public override void StartBehaviour()
    {
        base.StartBehaviour();
        isSeekingFood = false;
        targetBush = null;
    }

    public override void StopBehaviour()
    {
        base.StopBehaviour();
        isSeekingFood = false;
        targetBush = null;
        agent.isStopped = true;
    }

    private void FindAndSeekFood()
    {
        GameObject[] bushes = GameObject.FindGameObjectsWithTag("FoodBush");
        if (bushes.Length == 0)
        {
            Debug.LogWarning("No FoodBush found in scene!");
            return;
        }

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
        }
    }
}
