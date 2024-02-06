using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Config")]
    [SerializeField] private float velocity;
    [SerializeField] private float damage;
    [SerializeField] private GameObject explosion;

    const float LIFESPAWN = 5f;
    private void Start()
    {
        Setup();
        StartMoving();
        Destroy(gameObject, LIFESPAWN);
    }

    private void Setup()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    private void StartMoving()
    {
        rb.velocity = gameObject.transform.forward * velocity;
    }

    private void Explode()
    {
        GameObject vfx = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
        vfx.transform.parent = null;
        Destroy(vfx.gameObject, 2);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<Damageable>() != null)
        {
            collision.gameObject.GetComponentInParent<Damageable>().TakeDamage(damage);
        }
        Explode();
    }
}
