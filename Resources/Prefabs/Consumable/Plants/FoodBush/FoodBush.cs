using UnityEngine;

public class FoodBush : ConsumablePlant
{
    [Header("Foodbush Plant Info")]
    public int nutrition;
    public int satiation;

    public override void DisplayInfo()
    {
        Debug.Log($"Food Name: {foodname}, Satiation: {satiation}, Rotten Percentage: {rottenpercentage}");
    }
}