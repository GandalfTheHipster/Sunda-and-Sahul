using UnityEngine;
using System.Collections.Generic;

public class GodPower : MonoBehaviour
{
    [Header("Player Info")]
    public int powerid;
    public string powername = "Power";
    public int damage;

    public virtual void DisplayInfo()
    {
        Debug.Log($"PowerID: {powerid}, Power Name: {powername}");
    }

    public virtual void DisplayPoints()
    {
        Debug.Log($"Power Name: {powername}, Damage: {damage}");
    }

    /// <summary>
    /// Adds a GodPower to the given list if it's not already present.
    /// </summary>
    public static void AddPower(List<GodPower> list, GodPower powerToAdd)
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

    /// <summary>
    /// Removes a GodPower from the given list if it exists.
    /// </summary>
    public static void DeletePower(List<GodPower> list, GodPower powerToRemove)
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