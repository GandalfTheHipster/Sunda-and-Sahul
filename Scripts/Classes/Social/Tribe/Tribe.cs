using UnityEngine;

public class Tribe : SocialGroup
{
    [Header("Tribe Info")]
    public int tribeid;

    public virtual void TribeInfo()
    {
        Debug.Log($"TribeID: {tribeid}");
    }
}