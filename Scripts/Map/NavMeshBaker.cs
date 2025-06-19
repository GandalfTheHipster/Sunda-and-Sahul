using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public static class NavMeshBaker
{
    public static void BakeNavMesh(
        MeshFilter meshFilter,
        Vector3 meshOrigin,
        float meshSize,
        float heightScale)
    {
        // 1) Load the 'Sapien' prefab to get its NavMeshAgent settings
        GameObject sapiPrefab = Resources.Load<GameObject>("Sapien");
        if (sapiPrefab == null)
        {
            Debug.LogError("[NavMeshBaker] Could not load 'Sapien' prefab from Resources.");
        }

        int   agentTypeID  = 0;
        float agentHeight  = 2f;   // default fallback
        float agentRadius  = 0.5f; // default fallback
        // float agentClimb = <your-default>; // you can expose this if needed

        if (sapiPrefab != null)
        {
            var agentComp = sapiPrefab.GetComponent<NavMeshAgent>();
            if (agentComp != null)
            {
                agentTypeID = agentComp.agentTypeID;
                agentHeight = agentComp.height;
                agentRadius = agentComp.radius;
                // we’re skipping stepOffset here because it's unavailable
            }
            else
            {
                Debug.LogWarning("[NavMeshBaker] 'Sapien' has no NavMeshAgent; using defaults.");
            }
        }

        // 2) Override the bake settings
        var settings = NavMesh.GetSettingsByID(agentTypeID);
        settings.agentHeight = agentHeight;
        settings.agentRadius = agentRadius;
        // settings.agentClimb  = agentClimb; // optionally set if you expose it

        // 3) Build the NavMesh from the runtime mesh
        var source = new NavMeshBuildSource
        {
            shape        = NavMeshBuildSourceShape.Mesh,
            sourceObject = meshFilter.mesh,
            transform    = meshFilter.transform.localToWorldMatrix,
            area         = 0
        };

        var sources = new List<NavMeshBuildSource> { source };
        var bounds  = new Bounds(
            meshOrigin + Vector3.up * (heightScale * 0.5f),
            new Vector3(meshSize, heightScale, meshSize)
        );

        NavMeshData data = NavMeshBuilder.BuildNavMeshData(
            settings,
            sources,
            bounds,
            meshOrigin,
            Quaternion.identity
        );
        NavMesh.AddNavMeshData(data);

        Debug.Log("[NavMeshBaker] Runtime NavMesh baked with Sapien dimensions.");
    }
}