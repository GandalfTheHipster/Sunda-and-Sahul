using UnityEngine;
using System.Collections;

public class PawnsTribes : MonoBehaviour
{
    [Header("Components")]
    public TribeLoader tribeLoader;
    public PawnLoader pawnLoader;
    public TribeAssign tribeAssign;

    private IEnumerator Start()
    {
        Debug.Log("[PawnsTribes] Starting tribe-pawn-assignment process...");

        if (tribeLoader == null || pawnLoader == null || tribeAssign == null)
        {
            Debug.LogError("[PawnsTribes] Missing one or more assigned components in Inspector!");
            yield break;
        }

        // Load tribes
        yield return StartCoroutine(LoadTribes());

        // Force an extra frame to settle creation
        yield return new WaitForEndOfFrame();

        // Load pawns
        yield return StartCoroutine(LoadPawns());

        // Extra frame after pawn load to ensure registration
        yield return new WaitForEndOfFrame();

        // Assign members
        yield return StartCoroutine(AssignToTribes());

        // Final confirmation pause before print
        yield return new WaitForSecondsRealtime(0.1f);

        // Print final tribe/member info
        PrintAllTribes();

        Debug.Log("[PawnsTribes] Process complete.");
    }

    private IEnumerator LoadTribes()
    {
        Debug.Log("[PawnsTribes] Loading tribes...");
        yield return StartCoroutine(tribeLoader.LoadAllTribes());
        Debug.Log("[PawnsTribes] Tribe loading complete.");
    }

    private IEnumerator LoadPawns()
    {
        Debug.Log("[PawnsTribes] Loading pawns...");
        yield return StartCoroutine(pawnLoader.LoadAllPawns());
        Debug.Log("[PawnsTribes] Pawn loading complete.");
    }

    private IEnumerator AssignToTribes()
    {
        Debug.Log("[PawnsTribes] Assigning pawns to tribes...");
        tribeAssign.AssignAll();
        Debug.Log("[PawnsTribes] Tribe assignment complete.");
        yield return null;
    }

    private void PrintAllTribes()
    {
        Tribe[] allTribes = FindObjectsOfType<Tribe>();
        Debug.Log($"[PawnsTribes] Total tribes in scene: {allTribes.Length}");

        foreach (Tribe tribe in allTribes)
        {
            Debug.Log($"[PawnsTribes] Tribe ID {tribe.tribeid} (GroupID: {tribe.groupid}) has {tribe.members.Count} member(s):");
            tribe.ListEntities();
        }
    }
}