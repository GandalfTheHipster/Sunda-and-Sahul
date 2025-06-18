using UnityEngine;

public class Human : Creature
{
    [Header("Human Info")]
    public int humanid;
    public string firstname;
    public string middlename;
    public string lastname;

    [Header("Human Info")]
    public int tribeid;

    [Header("Mental Health")]
    public int leadership;

    [Header("Human Attributes")]
    public int stress;
    public int stimulation;
    public int love;
    public int sex;

    [Header("Human Skills")]
    // Crafting Skills
    public int fletching;
    public int knapping;
    public int tanning;
    public int weaving;
    public int cordage;
    // Survival Skills
    public int foraging;
    public int hunting;
    public int fishing;
    public int firemaking;
    public int tracking;
    public int shelterBuilding;
    public int cooking;
    // Art & Culture Skills
    public int painting;
    public int music;
    public int ritual;
    // Other
    public int animalTaming;

    // You can later override or extend this
    public virtual void Speak()
    {
        Debug.Log($"{firstname} says hello.");
    }

    public void SendThought(string description, float duration = 2f)
    {
        Brain brain = GetComponent<Brain>();
        if (brain != null)
        {
            Thought thought = new Thought(description, duration);
            brain.EnqueueThought(thought);
            Debug.Log($"{firstname} had a thought: {description}");
        }
        else
        {
            Debug.LogWarning($"{firstname} has no brain component attached!");
        }
    }


    // Example for later updating
    public void HaveBirthday()
    {
        age++;
        Debug.Log($"{firstname} just turned {age}!");
    }
}