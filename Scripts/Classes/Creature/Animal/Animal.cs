using UnityEngine;

public class Animal : Creature
{
    [Header("Animal Info")]
    public string favouritesong;
    public string surname;
    public string animalname;
    

    // You can later override or extend this
    public virtual void Crow()
    {
        Debug.Log($"{animalname} crows.");
    }
    
    // Example for later updating
    public void HaveBirthday()
    {
        age++;
        Debug.Log($"{animalname} just turned {age}!");
    }
}