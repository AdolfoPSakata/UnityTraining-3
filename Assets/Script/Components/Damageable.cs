using System;
using UnityEngine;
using UnityEngine.UI;

public class Damageable : MonoBehaviour
{
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
            UpdateBar(healthBar, health);
        CheckDeath(health);
    }

    public void Heal(float value)
    {
        health += value;
        if (healthBar != null)
            UpdateBar(healthBar, health);
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

    private void UpdateBar(Image bar, float value)
    {
        bar.fillAmount = value / initialHealth;
    }

    public void SetInitialConfig(float health)
    {
        initialHealth = health;
        this.health = health;
        UpdateBar(healthBar, health);
    }
}
