using UnityEngine;

public class Consumable : MonoBehaviour
{
    [Header("Consumable Info")]
    public string foodname;
    public int food_category;

    public virtual void DisplayInfo()
    {
        Debug.Log($"Food Name: {foodname}, Food Category: {food_category}");
    }

    public void Delete()
    {
        Debug.Log($"{foodname} DELETE THIS THING");
    }
}