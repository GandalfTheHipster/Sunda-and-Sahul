using UnityEngine;

public class Neanderthal : Human
{
    public override void Speak()
    {
        Debug.Log($"{firstname} grunts in a deep tone.");
    }

    public void Hunt()
    {
        Debug.Log($"{firstname} is out hunting.");
    }
}