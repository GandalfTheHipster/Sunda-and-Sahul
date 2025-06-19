using UnityEngine;

public class gp_fireball : GodPower
{
    [Header("Fireball Settings")]
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public float projectileSpeed = 20f;
    public float cooldown = 1f;
    private float _nextFireTime;

    public override void Activate()
    {
        if (Time.time < _nextFireTime)
            return;

        _nextFireTime = Time.time + cooldown;
        ShootFireball();
    }

    void ShootFireball()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("[gp_fireball] No projectilePrefab assigned!");
            return;
        }

        Vector3 origin    = spawnPoint != null ? spawnPoint.position : transform.position;
        Vector3 direction = Camera.main ? Camera.main.transform.forward : transform.forward;

        var ball = Instantiate(projectilePrefab, origin, Quaternion.LookRotation(direction));
        if (ball.TryGetComponent<Rigidbody>(out var rb))
            rb.velocity = direction * projectileSpeed;
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Debug.Log($"Cooldown: {cooldown}s, Speed: {projectileSpeed}");
    }
}