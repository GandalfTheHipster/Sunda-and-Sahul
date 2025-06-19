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

    /// <summary>
    /// Adds a Creature to the group by its entity ID.
    /// </summary>
    public void AddMember(int entityid)
    {
        if (members.Exists(c => c.entityid == entityid))
        {
            Debug.LogWarning($"Entity {entityid} is already in group {groupid}.");
            return;
        }
        Creature toAdd = FindCreatureByID(entityid);
        if (toAdd != null)
        {
            members.Add(toAdd);
            Debug.Log($"{toAdd.name} (ID: {entityid}) added to group {groupid}.");
        }
        else
        {
            Debug.LogWarning($"No creature with ID {entityid} found in scene.");
        }
    }

    /// <summary>
    /// Removes a Creature from the group by its entity ID.
    /// </summary>
    public void RemoveMember(int entityid)
    {
        Creature toRemove = members.Find(c => c.entityid == entityid);
        if (toRemove != null)
        {
            members.Remove(toRemove);
            Debug.Log($"{toRemove.name} (ID: {entityid}) removed from group {groupid}.");
        }
        else
        {
            Debug.LogWarning($"Entity {entityid} not found in group {groupid}.");
        }
    }

    /// <summary>
    /// Overload: remove by reference.
    /// </summary>
    public void RemoveMember(Creature creature)
    {
        if (members.Contains(creature))
        {
            members.Remove(creature);
            Debug.Log($"{creature.name} removed from group {groupid}.");
        }
        else
        {
            Debug.LogWarning($"{creature.name} is not in group {groupid}.");
        }
    }

    /// <summary>
    /// Lists all members in the group.
    /// </summary>
    public void ListEntities()
    {
        Debug.Log($"Group {groupid} Members:");
        foreach (Creature c in members)
            Debug.Log($"- {c.name} (ID: {c.entityid})");
    }

    /// <summary>
    /// Helper to locate a Creature in the scene by its entity ID.
    /// </summary>
    private Creature FindCreatureByID(int entityid)
    {
        foreach (Creature c in FindObjectsOfType<Creature>())
        {
            if (c.entityid == entityid)
                return c;
        }
        return null;
    }
}