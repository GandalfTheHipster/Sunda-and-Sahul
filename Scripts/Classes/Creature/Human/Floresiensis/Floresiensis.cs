using UnityEngine;

public class Floresiensis : Human
{
    public override void Speak()
    {
        Debug.Log($"{firstname} gurgles in a freakish tone.");
    }

    public void Breakfast()
    {
        Debug.Log($"{firstname} has a wonderful third breakfast.");
    }

    public void Hunt()
    {
        Debug.Log($"{firstname} is out hunting.");
    }
}