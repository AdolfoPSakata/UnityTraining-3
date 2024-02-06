using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Tank playerTank;

    [Header("Objects")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject turret;
    [SerializeField] private GameObject nozzle;

    [Header("Components")]
    [SerializeField] private Damageable damageable;
    [SerializeField] private Mover mover;
    [SerializeField] private Rotator rotator;
    [SerializeField] private Shoter shoter;
    [SerializeField] private Wallet wallet;

    [Header("Drops")]
    [SerializeField] private GameObject sfxDeath;

    public Action onDeath;
    public Action onShot;
    public UnityAction<float> onRotate;
    public UnityAction<float> onMove;
    public UnityAction<bool> onPressButtonMover;
    public UnityAction<bool> onPressButtonRotator;

    private Coroutine shooting;
    private GameObject target;
    private GameObject projectile;

    public void SetupInputs()
    {
        damageable.OnDeath = Die;
        onPressButtonMover += mover.ChangePressedStatus;
        onPressButtonRotator += rotator.ChangePressedStatus;
        onRotate += rotator.StartRotation;
        onMove += mover.StartMoving;
        onShot += Shot;
    }

    public void ConfigurePlayer(TankConfig tankConfig)
    {
        playerTank.Init(tankConfig);
        SetConfigValue();
    }

    private IEnumerator ShotRoutine()
    {
        shoter.Shot();
        EventBus.startCooldownEvent.Publish(new EventArgs(playerTank.fireRate));
        yield return new WaitForSeconds(playerTank.fireRate);
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
        DropSFX();
        mover.FullStop();
        onDeath.Invoke();
        gameObject.SetActive(false);
    }

    private void DropSFX()
    {
        GameObject dropSFX = Instantiate(sfxDeath);
        dropSFX.transform.position = gameObject.transform.position;
        dropSFX.transform.parent = null;
        Destroy(dropSFX, 2);
    }
    public void SetConfigValue()
    {
        damageable.SetInitialConfig(playerTank.health);
        mover.SetInitialConfig(playerTank.weight, playerTank.speed);
        rotator.SetInitialConfig(playerTank.maneuverability);
        shoter.SetInitialConfig(playerTank.projectile);
        virtualCamera.m_Lens.OrthographicSize = playerTank.spotRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PickUp pickUp))
        {
            Debug.Log(other.gameObject.name);
            switch (pickUp)
            {
                case Bolt:
                    wallet.AddBolts(pickUp.GetValue());
                    break;
                case Ammo:
                    shoter.AddAmmo((int)pickUp.GetValue());
                    break;
                case MedKit:
                    damageable.Heal(pickUp.GetValue());
                    break;
                default:
                    break;
            }
        }
    }
}
