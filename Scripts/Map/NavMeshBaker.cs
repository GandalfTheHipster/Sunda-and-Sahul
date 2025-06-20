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
        // Default/fallback agent settings
        int   agentTypeID  = 0;    // Use default humanoid
        float agentHeight  = 2f;
        float agentRadius  = 0.5f;

        // Get default NavMesh settings for agentTypeID 0
        var settings = NavMesh.GetSettingsByID(agentTypeID);
        settings.agentHeight = agentHeight;
        settings.agentRadius = agentRadius;

        // Build the NavMesh from the runtime mesh
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

        Debug.Log("[NavMeshBaker] Runtime NavMesh baked using default agent dimensions.");
    }
}