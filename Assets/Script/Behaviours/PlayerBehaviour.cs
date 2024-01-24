using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camera;
    [SerializeField] private GameObject tower;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject nozzle;
    [SerializeField] private GameObject target;

    [SerializeField] private Damageable damageable;
    [SerializeField] private Mover mover;
    [SerializeField] private Rotator rotator;
    [SerializeField] private Shoter shoter;
    [SerializeField] private Wallet wallet;
   
    Coroutine shooting;

    [SerializeField] private float health;
    [SerializeField] private float weight;
    [SerializeField] private float responseTime;
    [SerializeField] private float range;
    [SerializeField] private float spotRange;
    [SerializeField] private float fireRate;
    [SerializeField] private float speed;
    [SerializeField] private float maneuverability;
    [SerializeField] private int bolts;
    [SerializeField] private int ammo;

    public Action onDeath;
    public Action onShot;
    public UnityAction<float> onRotate;
    public UnityAction<float> onMove;
    public UnityAction<bool> onPressButtonMover;
    public UnityAction<bool> onPressButtonRotator;

    public void SetupInputs()
    {
        damageable.OnDeath = Die;
        onPressButtonMover += mover.ChangePressedStatus;
        onPressButtonRotator += rotator.ChangePressedStatus;
        onRotate += rotator.StartRotation;
        onMove += mover.StartMoving;
        onShot += Shot;
    }
    //move it
    public void ConfigurePlayer(TankConfig tankConfig)
    {
        health = tankConfig.wheel.health + tankConfig.body.health + tankConfig.turret.health;
        weight = tankConfig.wheel.weight + tankConfig.body.weight + tankConfig.cannon.weight + tankConfig.turret.weight;
        fireRate = tankConfig.cannon.fireRate;
        speed = tankConfig.wheel.speed;
        maneuverability = tankConfig.body.maneuverability;
        spotRange = tankConfig.turret.spotRange;

        SetConfigValue();
    }

    private IEnumerator ShotRoutine()
    {
        shoter.Shot();
        yield return new WaitForSeconds(fireRate);
        shooting = null;
    }

    public void Shot()
    {
        if (shooting == null && gameObject.activeSelf)
            shooting = StartCoroutine(ShotRoutine());
    }

    private void Die()
    {
        StopAllCoroutines();
        mover.FullStop();
        onDeath.Invoke();
        gameObject.SetActive(false);
    }

    private void PickUpItem()
    {
        shoter.AddAmmo(1);
        damageable.Heal(1);
        wallet.AddBolts(1);
    }

    public void SetConfigValue()
    {
        damageable.SetInitialConfig(health);
        mover.SetInitialConfig(weight, speed);
        rotator.SetInitialConfig(maneuverability);
        camera.m_Lens.OrthographicSize = spotRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        PickUpItem();
    }

    private void OnTriggerExit(Collider other)
    {
    }
}
