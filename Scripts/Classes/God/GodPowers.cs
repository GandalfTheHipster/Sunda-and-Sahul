using UnityEngine;
using System.Collections.Generic;

public class GodPowers : MonoBehaviour
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
}