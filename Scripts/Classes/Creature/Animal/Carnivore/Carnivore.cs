using UnityEngine;
using System.Collections.Generic;

public class Carnivore : Animal
{
    [Header("Animal Info")]
    public List<Creature> diet = new();

    // You can later override or extend this
    public virtual void AddtoDiet()
    {
        Debug.Log($"{animalname} crows.");
    }

     public virtual void Remove()
    {
        Debug.Log($"{animalname} crows.");
    }
}