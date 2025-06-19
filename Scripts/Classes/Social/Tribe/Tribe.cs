using UnityEngine;
using System.Collections.Generic;

public class Tribe : SocialGroup
{
    [Header("Tribe Info")]
    public int tribeid;

    public virtual void TribeInfo()
    {
        Debug.Log($"TribeID: {tribeid}");
    }

    public static Tribe CreateTribe(int tribeid, int groupid, List<int> memberids)
    {
        GameObject tribeGO = new GameObject($"Tribe_{tribeid}");
        Tribe newTribe = tribeGO.AddComponent<Tribe>();
        newTribe.tribeid = tribeid;
        newTribe.groupid = groupid;

        if (memberids == null)
        {
            Debug.LogWarning($"[CreateTribe] memberids was null for Tribe {tribeid}.");
            memberids = new List<int>();
        }

        foreach (int id in memberids)
        {
            newTribe.AddMember(id);
        }

        Debug.Log($"[CreateTribe] Tribe {tribeid} (GroupID: {groupid}) created with {memberids.Count} member(s).");
        return newTribe;
    }
}