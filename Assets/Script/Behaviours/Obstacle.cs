using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField] private Damageable damageable;
    [SerializeField] private int collisionDamage = 1;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            damageable.TakeDamage(collisionDamage);
        }
    }
}
