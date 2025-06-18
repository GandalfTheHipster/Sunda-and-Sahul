using UnityEngine;

public class Herd : SocialGroup
{
    [Header("Herd Info")]
    public int herdid;

    public virtual void HerdID()
    {
        Debug.Log($"HerdID: {herdid}");
    }
}