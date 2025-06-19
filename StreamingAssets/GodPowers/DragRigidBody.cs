using UnityEngine;
using System.Collections;

/// <summary>
/// Click and drag Rigidbodies in the scene using physics.
/// </summary>
[RequireComponent(typeof(Camera))]
public class DragRigidbody : MonoBehaviour
{
    [Header("Spring Settings")]
    [Tooltip("Strength of the spring joint.")]
    public float spring = 50f;
    [Tooltip("Damping of the spring joint.")]
    public float damper = 5f;
    [Tooltip("Max distance the spring can stretch.")]
    public float maxDistance = 0.2f;

    [Header("Temporary Drag Overrides")]
    [Tooltip("Drag applied to the object while dragging.")]
    public float drag = 10f;
    [Tooltip("Angular drag applied while dragging.")]
    public float angularDrag = 5f;

    [Header("Raycast Settings")]
    [Tooltip("Which layers can be clicked and dragged.")]
    public LayerMask draggableLayers = Physics.DefaultRaycastLayers;
    [Tooltip("Max distance for the picking ray.")]
    public float maxRayDistance = 100f;

    // The spring joint we’ll create at runtime
    private SpringJoint springJoint;

    void Update()
    {
        // Start drag on left-mouse down
        if (Input.GetMouseButtonDown(0))
            TryStartDrag();
    }

    void TryStartDrag()
    {
        Camera cam = GetComponent<Camera>();
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, draggableLayers))
            return;

        // Only drag non-kinematic Rigidbodies
        if (hit.rigidbody == null || hit.rigidbody.isKinematic)
            return;

        // Lazy-create the spring joint object
        if (springJoint == null)
        {
            GameObject go = new GameObject("RigidbodyDragger");
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            springJoint = go.AddComponent<SpringJoint>();
        }

        // Position the joint at the hit point
        springJoint.transform.position = hit.point;
        springJoint.anchor = Vector3.zero;

        // Configure spring
        springJoint.spring = spring;
        springJoint.damper = damper;
        springJoint.maxDistance = maxDistance;

        // Connect to the hit body
        springJoint.connectedBody = hit.rigidbody;

        // Start the drag coroutine, passing the initial hit distance
        StartCoroutine(DragObject(hit.distance));
    }

    IEnumerator DragObject(float distance)
    {
        Rigidbody targetRb = springJoint.connectedBody;
        // Cache old drag values
        float oldDrag = targetRb.drag;
        float oldAngularDrag = targetRb.angularDrag;

        // Override for smoother dragging
        targetRb.drag = drag;
        targetRb.angularDrag = angularDrag;

        Camera cam = GetComponent<Camera>();
        while (Input.GetMouseButton(0))
        {
            // Move the joint’s anchor to follow the mouse
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            springJoint.transform.position = ray.GetPoint(distance);
            yield return null;
        }

        // On release, restore original drag and disconnect
        if (springJoint.connectedBody)
        {
            springJoint.connectedBody.drag = oldDrag;
            springJoint.connectedBody.angularDrag = oldAngularDrag;
            springJoint.connectedBody = null;
        }
    }
}