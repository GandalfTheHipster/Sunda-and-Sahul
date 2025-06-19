using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Base speed for camera movement via WASD or arrow keys.")]
    public float moveSpeed = 10f;
    [Tooltip("Multiplier applied to speeds when holding Shift.")]
    public float fastMultiplier = 2f;

    void Update()
    {
        HandleKeyboardMovement();
    }

    /// <summary>
    /// Handles keyboard movement (WASD / Arrow Keys) and vertical controls.
    /// Shift multiplies movement speed.
    /// </summary>
    void HandleKeyboardMovement()
    {
        // Determine speed multiplier
        bool isFast = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float currentSpeed = moveSpeed * (isFast ? fastMultiplier : 1f);

        // Horizontal movement
        float h = Input.GetAxis("Horizontal"); // A/D, Left/Right
        float v = Input.GetAxis("Vertical");   // W/S, Up/Down
        Vector3 move = (transform.right * h + transform.forward * v);
        move.y = 0f; // keep strictly horizontal
        transform.position += move * currentSpeed * Time.deltaTime;

        // Vertical movement: Space = up, Ctrl = down
        if (Input.GetKey(KeyCode.Space))
            transform.position += Vector3.up * currentSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            transform.position += Vector3.down * currentSpeed * Time.deltaTime;
    }
}