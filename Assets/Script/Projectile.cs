using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float velocity;
    [SerializeField] private float damage;
    [SerializeField] private GameObject explosion;

    private void Start()
    {
        Setup();
        StartMoving();
        Destroy(gameObject, 5f);
    }
    private void Setup()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    public void StartMoving()
    {
        //rb.AddForce(gameObject.transform.forward * velocity, ForceMode.Impulse);
        rb.velocity = gameObject.transform.forward * velocity;
    }

    public void Explode()
    {
        GameObject vfx = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
        vfx.transform.parent = null;
        Destroy(vfx.gameObject, 2);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Damage Taken: {damage}, {collision.gameObject.name}");
        if (collision.gameObject.GetComponentInParent<Damageable>() != null)
        {
            collision.gameObject.GetComponentInParent<Damageable>().TakeDamage(damage);
        }
        Explode();
    }
}
