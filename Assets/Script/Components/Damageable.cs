using System;
using UnityEngine;
using UnityEngine.UI;
public class Damageable : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float initialHealth;
    [SerializeField] private float health;
    [SerializeField] private Image healthBar;
    [SerializeField] private Canvas canvas;

    public Action OnDeath;
    private void Awake()
    {
        initialHealth = health;
        if (healthBar != null)
            canvas.worldCamera = Camera.main;
    }

    public void TakeDamage(float value)
    {
        health -= value;
        if (healthBar != null)
            EventBus.healthChangeEvent.Publish(new EventArgs(UpdateBar(health)));
        CheckDeath(health);
    }

    public void Heal(float value)
    {
        health += value;
        if (healthBar != null)
            EventBus.healthChangeEvent.Publish(new EventArgs(UpdateBar(health)));
    }

    public void CheckDeath(float health)
    {
        if (health <= 0)
            Die();
    }

    public void Die()
    {
        if (OnDeath != null)
            OnDeath.Invoke();
        else
           gameObject.SetActive(false);
    }

    private float UpdateBar(float value)
    {
        return value / initialHealth;
    }

    public void SetInitialConfig(float health)
    {
        initialHealth = health;
        this.health = health;
        EventBus.healthChangeEvent.Publish(new EventArgs(UpdateBar(health)));
    }
}
