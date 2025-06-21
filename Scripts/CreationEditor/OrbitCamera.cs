using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Distance")]
    public float distance = 5f;
    public float zoomSpeed = 2f;
    public float minDistance = 2f;
    public float maxDistance = 20f;

    [Header("Orbit")]
    public float orbitSpeed = 5f;
    private float yaw = 0f;
    private float pitch = 20f;
    public float minPitch = -20f;
    public float maxPitch = 80f;

    [Header("Pan")]
    public float panSpeed = 0.5f;

    void Start()
    {
        if (target == null)
        {
            // If no target is set, find any object tagged as "Player"
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null) target = obj.transform;
        }

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        HandleInput();
        UpdateCameraPosition();
    }

    void HandleInput()
    {
        // Orbit
        if (Input.GetMouseButton(0))
        {
            yaw += Input.GetAxis("Mouse X") * orbitSpeed;
            pitch -= Input.GetAxis("Mouse Y") * orbitSpeed;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        // Pan
        if (Input.GetMouseButton(2) || Input.GetMouseButton(1))
        {
            Vector3 right = transform.right;
            Vector3 up = transform.up;

            Vector3 pan = -right * Input.GetAxis("Mouse X") * panSpeed
                          - up * Input.GetAxis("Mouse Y") * panSpeed;

            target.position += pan;
        }

        // Zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }

    void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);

        transform.position = target.position + offset;
        transform.LookAt(target);
    }
}