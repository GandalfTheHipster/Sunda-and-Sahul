using UnityEngine;
using System.Collections.Generic;

public class Herbivore : Animal
{
    [Header("Animal Info")]
    public List<Creature> diet = new();

    public virtual void AddtoDiet()
    {
        Debug.Log($"{animalname} crows.");
    }

     public virtual void Remove()
    {
        Debug.Log($"{animalname} crows.");
    }
}