using UnityEngine;
public abstract class PickUp : MonoBehaviour
{
    private Mesh model;
    protected Rigidbody rb;
    [SerializeField] protected int lifeSpawn;
    [SerializeField] protected float amount;
   
    private void Awake()
    {
        Destroy(gameObject, lifeSpawn);
    }

    public void Init(Vector3 direction)
    {
        TryGetComponent<Rigidbody>(out rb);
        rb.AddForce(direction, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            Destroy(gameObject);
    }

    public abstract float GetValue();
}
