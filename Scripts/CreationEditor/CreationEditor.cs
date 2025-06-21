using UnityEngine;

public class CreationEditor : MonoBehaviour
{
    [Header("Editor Info")]
    public int entityid

    // What sort of stuff do we want to edit? I want people to be able to create custom characters in game
    // Maybe Animals one day?
    // This is a box of lego, a playbox of toys
    // The editor will be SUPER hard to create. Ngl. Just see how The Sapling is struggling.

    public virtual void DisplayInfo()
    {
        Debug.Log($"Creature EntityID: {entityid}, Age: {age}");
    }

    public virtual void Export()
    {
        Debug.Log($"{entityid} dies.");
    }

    void Update()
    {

    }
}