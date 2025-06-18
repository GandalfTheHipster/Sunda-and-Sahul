using UnityEngine;

public class ConsumablePlant : Consumable
{
    [Header("Consumable Plant Info")]
    public int rottenpercentage;

    public override void DisplayInfo()
    {
        Debug.Log($"Food Name: {foodname}, Rotten Percentage: {rottenpercentage}");
    }
}