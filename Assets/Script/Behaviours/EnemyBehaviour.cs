using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Tank enemyTank;

    [Header("Objects")]
    [SerializeField] private GameObject turret;
    [SerializeField] private GameObject nozzle;

    [Header("Drops")]
    [SerializeField] private GameObject[] drops;
    [SerializeField] private GameObject sfxDeath;

    [Header("Components")]
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Damageable damageable;
    [SerializeField] private AudioSource audioSource;

    private EnemyState state;
    private GameObject target;
    private GameObject projectile;
    private bool hasStopped = true;
    private Coroutine stateMachine;
    private Coroutine moving;
    private Coroutine aiming;
    private Coroutine shooting;

    public Action<GameObject> onDeath;
    const float MIN_TO_ROTATE = 1f;

    public enum EnemyState
    {
        Idle,
        Move,
        Follow,
        Shot,
        Aim
    }

    public void ConfigureEnemy(TankConfig tankConfig)
    {
        enemyTank.Init(tankConfig);
        rb.mass = enemyTank.weight;
        SetConfigValue();
    }

    public void Init()
    {
        damageable = gameObject.GetComponent<Damageable>();
        damageable.OnDeath = Die;
        stateMachine = StartCoroutine(StateMachineCheck());
    }

    private IEnumerator StateMachineCheck()
    {
        while (true)
        {
            target = CastSphere();
            switch (state)
            {
                case EnemyState.Follow:
                    Follow(target);
                    Aim(target);
                    yield return new WaitForFixedUpdate();
                    break;
                case EnemyState.Shot:
                    Shot();
                    yield return new WaitForFixedUpdate();
                    break;
                case EnemyState.Idle:
                    StopCoroutines(moving, aiming, shooting);
                    moving = null;
                    aiming = null;
                    shooting = null;
                    yield return new WaitForSeconds(1);
                    break;
            }
        }
    }

    private void Shot()
    {
        if (shooting == null)
            shooting = StartCoroutine(ShotRoutine());
    }

    private void Follow(GameObject target)
    {
        if (moving == null)
            moving = StartCoroutine(FollowRoutine());
    }
    private void Aim(GameObject target)
    {
        if (aiming == null)
            aiming = StartCoroutine(AimRoutine());
    }
    private IEnumerator AimRoutine()
    {
        while (true)
        {
            if (target == null)
            {
                state = EnemyState.Idle;
                break;
            }

            turret.transform.LookAt(target.transform.position);
            if (Vector3.Distance(turret.transform.position, target.transform.position) <= enemyTank.range)
                state = EnemyState.Shot;
            else
            {
                state = EnemyState.Follow;
                break;
            }

            yield return new WaitForFixedUpdate();
        }
    }
    private IEnumerator ShotRoutine()
    {
        GameObject goProj = Instantiate(projectile);
        audioSource.Play();
        goProj.transform.position = nozzle.transform.position;
        goProj.transform.rotation = nozzle.transform.rotation;
        goProj.transform.parent = null;

        yield return new WaitForSeconds(enemyTank.fireRate);
        state = EnemyState.Follow;
        shooting = null;
    }
    private IEnumerator FollowRoutine()
    {
        navMeshAgent.speed = MIN_TO_ROTATE;
        navMeshAgent.SetDestination(target.transform.position);
        yield return new WaitForSeconds(enemyTank.speed);
        navMeshAgent.speed = enemyTank.speed;

        while (state == EnemyState.Follow)
        {
            yield return new WaitForSeconds(enemyTank.responseTime);
            navMeshAgent.ResetPath();
            navMeshAgent.SetDestination(target.transform.position);
        }
    }

    private void Die()
    {
        DropItens();
        DropSFX();
        onDeath.Invoke(gameObject);
        gameObject.SetActive(false);
        EventBus.killChangeEvent.Publish(new EventArgs(1));
    }

    private void DropItens()
    {
        int count = GetRandom(0, 3);
        for (int i = 0; i < count; i++)
        {
            Vector3 offset = new Vector3(GetRandom(-4f, 4f), GetRandom(3f, 4f), GetRandom(-4f, 4f));
            GameObject drop = Instantiate(drops[GetRandom(0, drops.Length)]);
            drop.transform.position = gameObject.transform.position + Vector3.up;
            drop.GetComponent<PickUp>().Init(offset);
        }
    }

    private void DropSFX()
    {
        GameObject dropSFX = Instantiate(sfxDeath);
        dropSFX.transform.position = gameObject.transform.position;
        dropSFX.transform.parent = null;
        Destroy(dropSFX, 2);
    }

    private int GetRandom(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    private float GetRandom(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }
    private GameObject CastSphere()
    {
        Collider[] collisions = Physics.OverlapSphere(gameObject.transform.position, enemyTank.spotRange);
        for (int i = 0; i < collisions.Length; i++)
        {
            if (collisions[i].tag == "Player")
            {
                hasStopped = false;
                if (state == EnemyState.Idle)
                    state = EnemyState.Follow;

                return collisions[i].gameObject;
            }
        }
        state = EnemyState.Idle;
        return null;
    }

    public void SetConfigValue()
    {
        damageable.SetInitialConfig(enemyTank.health);
        projectile = enemyTank.projectile;
        navMeshAgent.angularSpeed = enemyTank.maneuverability * 10;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gameObject.transform.position, enemyTank.spotRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, enemyTank.range);
    }

    private void StopCoroutines(params Coroutine[] corotines)
    {
        if (hasStopped)
            return;

        for (int i = 0; i < corotines.Length; i++)
        {
            if (corotines[i] != null)
            {
                StopCoroutine(corotines[i]);
            }
        }
        hasStopped = true;
    }
}