using UnityEngine;

public class Erectus : Human
{
    public override void Speak()
    {
        Debug.Log($"{firstname} gurgles in a freakish tone.");
    }

    public void SecretErectusSuperpower()
    {
        Debug.Log($"{firstname} fires lightning bolts out of their hands.");
    }

    public void Hunt()
    {
        Debug.Log($"{firstname} is out hunting.");
    }
}