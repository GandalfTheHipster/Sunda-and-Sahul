using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(God))]
public class HumanGod : God
{
    /// <summary>
    /// The singleton instance of the local HumanGod.
    /// </summary>
    public static HumanGod Instance { get; private set; }

    /// <summary>
    /// Index of the currently selected ability in the abilities list.
    /// </summary>
    private int currentAbilityIndex = 0;

    void Awake()
    {
        // Ensure only one local human
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("[HumanGod] Multiple HumanGod instances detected. Destroying duplicate.");
            Destroy(this);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        // Only the singleton processes input
        if (Instance != this) return;

        // --- Ability Selection (optional) ---
        // Cycle forward: E, backward: Q
        if (Input.GetKeyDown(KeyCode.E))
        {
            CycleAbility(1);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            CycleAbility(-1);
        }

        // --- Ability Activation ---
        // You can map Fire1 (left click) to activate the selected power:
        if (Input.GetButtonDown("Fire1"))
        {
            ActivateCurrentAbility();
        }

        // Or bind number keys 1–n directly:
        for (int i = 0; i < abilities.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                abilities[i].Activate();
            }
        }
    }

    /// <summary>
    /// Moves the currentAbilityIndex by delta (wraps around).
    /// </summary>
    private void CycleAbility(int delta)
    {
        if (abilities == null || abilities.Count == 0) return;
        currentAbilityIndex = (currentAbilityIndex + delta + abilities.Count) % abilities.Count;
        Debug.Log($"[HumanGod] Selected ability: {abilities[currentAbilityIndex].powername}");
    }

    /// <summary>
    /// Activates whatever ability is currently selected.
    /// </summary>
    private void ActivateCurrentAbility()
    {
        if (abilities == null || abilities.Count == 0) return;
        abilities[currentAbilityIndex].Activate();
    }
}