using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] float health;
    public void TakeDamage(float value)
    {
        health -= value;
        CheckDeath(health);
    }

    public void CheckDeath(float health)
    {
        if (health <= 0)
            Die();
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
