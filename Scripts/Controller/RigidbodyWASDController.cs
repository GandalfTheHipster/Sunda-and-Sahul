using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyWASDController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(horizontal, 0f, vertical).normalized;
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
    }
}