using UnityEngine;

public class Sapien : Human
{
    public override void Speak()
    {
        Debug.Log($"{firstname} gurgles in a freakish tone.");
    }

    public void Hunt()
    {
        Debug.Log($"{firstname} is out hunting.");
    }
}
