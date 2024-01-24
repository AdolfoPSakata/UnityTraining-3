using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private GameObject tower;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject nozzle;
    [SerializeField] private GameObject target;

    Coroutine stateMachine;
    Coroutine moving;
    Coroutine aiming;
    Coroutine shooting;
    private EnemyState state;

    [SerializeField] private float health;
    [SerializeField] private float weight;
    [SerializeField] private float responseTime;
    [SerializeField] private float range;
    [SerializeField] private float fireRate;
    [SerializeField] private float speed;
    [SerializeField] private float maneuverability;
    [SerializeField] private float spotRange;

    [SerializeField] private Rigidbody rb;

    private bool hasStopped = true;
    [SerializeField] private Damageable damageable;

    public Action<GameObject> onDeath;
    const float MIN_TO_ROTATE = 0.1f;
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
        health = tankConfig.wheel.health + tankConfig.body.health + tankConfig.turret.health;
        weight = tankConfig.wheel.weight + tankConfig.body.weight + tankConfig.cannon.weight + tankConfig.turret.weight;
        responseTime = tankConfig.turret.responseTime;
        spotRange = tankConfig.turret.spotRange;
        range = tankConfig.cannon.range;
        fireRate = tankConfig.cannon.fireRate;
        speed = tankConfig.wheel.speed;
        maneuverability = tankConfig.body.maneuverability;
        
        rb.mass = weight;
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
           // Debug.LogWarning($"<color=#00FF00>{state}</color>");
        }
    }

    private void Shot()
    {
        //Debug.LogWarning($"<color=black>{shooting == null}</color>");
        if (shooting == null)
            shooting = StartCoroutine(ShotRoutine());
    }

    private void Follow(GameObject target)
    {
        //Debug.LogWarning($"<color=black>{moving == null}</color>");
        if (moving == null)
            moving = StartCoroutine(FollowRoutine());
    }
    private void Aim(GameObject target)
    {
        //Debug.LogWarning($"<color=black>{aiming == null}</color>");
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
                
            tower.transform.LookAt(target.transform.position);
            if (Vector3.Distance(tower.transform.position, target.transform.position) <= range)
                state = EnemyState.Shot;
            else
            {
                state = EnemyState.Follow;
                break;
            }

            yield return new WaitForFixedUpdate();
        }
        //Debug.LogWarning($"<color=red> AimRoutine END </color>");
    }
    private IEnumerator ShotRoutine()
    {
        GameObject goProj = Instantiate(projectile);
        goProj.transform.position = nozzle.transform.position;
        goProj.transform.rotation = nozzle.transform.rotation;
        goProj.transform.parent = null;
        yield return new WaitForSeconds(fireRate);
        state = EnemyState.Follow;
        shooting = null;
       // Debug.LogWarning($"<color=red> ShotRoutine END </color>");
    }
    private IEnumerator FollowRoutine()
    {
        navMeshAgent.speed = MIN_TO_ROTATE;
        navMeshAgent.SetDestination(target.transform.position);
        yield return new WaitForSeconds(speed);
        navMeshAgent.speed = speed;

        while (state == EnemyState.Follow)
        {
            yield return new WaitForSeconds(responseTime);
            navMeshAgent.ResetPath();
            navMeshAgent.SetDestination(target.transform.position);
        }
        //Debug.LogWarning($"<color=red> FollowRoutine END </color>");
    }

    private void Die()
    {
        onDeath.Invoke(gameObject);
        gameObject.SetActive(false);
    }

    private GameObject CastSphere()
    {
        Collider[] collisions = Physics.OverlapSphere(gameObject.transform.position, spotRange);
        for (int i = 0; i < collisions.Length; i++)
        {
            if (collisions[i].tag == "Player")
            {
                hasStopped = false;
                if (state == EnemyState.Idle)
                    state = EnemyState.Follow;

                //Debug.LogWarning($"<color=purple>{hasStopped}</color>");
                return collisions[i].gameObject;
            }
        }
        //Debug.LogWarning($"<color=purple>{hasStopped}</color>");
        state = EnemyState.Idle;
        return null;
    }

    public void SetConfigValue()
    {
        damageable.SetInitialConfig(health);
        navMeshAgent.angularSpeed = maneuverability * 10;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gameObject.transform.position, spotRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, range);
    }

    private void StopCoroutines(params Coroutine[] corotines)
    {
       // Debug.LogWarning($"<color=white>{hasStopped}</color>");
        if (hasStopped)
            return;

        for (int i = 0; i < corotines.Length; i++)
        {
            if (corotines[i] != null)
            {
                //Debug.LogWarning($"<color=gray> {corotines[i]} END </color>");
                StopCoroutine(corotines[i]);
            }
        }
        hasStopped = true;
        //Debug.LogWarning($"<color=white>{hasStopped}</color>");
    }
}