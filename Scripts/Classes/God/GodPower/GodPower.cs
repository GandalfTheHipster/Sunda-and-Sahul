using UnityEngine;

public class GodPower : MonoBehaviour
{
    [Header("Power Info")]
    public int powerid;
    public string powername = "Power";
    public int damage;

    /// <summary>
    /// Called when the player uses this power.
    /// Override in derived classes to implement the actual effect.
    /// </summary>
    public virtual void Activate()
    {
        Debug.Log($"[GodPower] Activated power: {powername}");
    }

    public virtual void DisplayInfo()
    {
        Debug.Log($"PowerID: {powerid}, Power Name: {powername}");
    }

    public virtual void DisplayPoints()
    {
        Debug.Log($"Power Name: {powername}, Damage: {damage}");
    }

    public static void AddPower(System.Collections.Generic.List<GodPower> list, GodPower powerToAdd)
    {
        if (powerToAdd == null)
        {
            Debug.LogWarning("[GodPower] Cannot add null power.");
            return;
        }

        if (!list.Contains(powerToAdd))
        {
            list.Add(powerToAdd);
            Debug.Log($"[GodPower] Added power: {powerToAdd.powername}");
        }
        else
        {
            Debug.LogWarning($"[GodPower] Power already in list: {powerToAdd.powername}");
        }
    }

    public static void DeletePower(System.Collections.Generic.List<GodPower> list, GodPower powerToRemove)
    {
        if (powerToRemove == null)
        {
            Debug.LogWarning("[GodPower] Cannot remove null power.");
            return;
        }

        if (list.Contains(powerToRemove))
        {
            list.Remove(powerToRemove);
            Debug.Log($"[GodPower] Removed power: {powerToRemove.powername}");
        }
        else
        {
            Debug.LogWarning($"[GodPower] Power not found in list: {powerToRemove.powername}");
        }
    }
}