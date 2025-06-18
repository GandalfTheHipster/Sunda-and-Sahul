using UnityEngine;
using System.Collections.Generic;

public class SocialGroup : MonoBehaviour
{
    [Header("Group Info")]
    public int groupid;

    [Header("Members")]
    public List<Creature> members = new();

    public virtual void DisplayInfo()
    {
        Debug.Log($"GroupID: {groupid}, Member Count: {members.Count}");
    }

    public void AddMember(Creature creature)
    {
        if (!members.Contains(creature))
        {
            members.Add(creature);
            Debug.Log($"{creature.name} added to group {groupid}.");
        }
    }

    public void RemoveMember(Creature creature)
    {
        if (members.Contains(creature))
        {
            members.Remove(creature);
            Debug.Log($"{creature.name} removed from group {groupid}.");
        }
    }

    public void ListEntities()
    {
        Debug.Log($"Group {groupid} Members:");
        foreach (Creature c in members)
        {
            Debug.Log($"- {c.name}");
        }
    }
}
