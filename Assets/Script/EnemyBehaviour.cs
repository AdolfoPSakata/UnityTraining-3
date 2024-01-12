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
    Coroutine moving;
    Coroutine aiming;
    Coroutine shooting;
    private EnemyState state;

    [SerializeField] private float responseTime;
    [SerializeField] private float range;
    [SerializeField] private float fireRate;
    [SerializeField] private float speed;
    [SerializeField] private float maneuverability;

    const float MIN_TO_ROTATE = 0.1f;
    public enum EnemyState
    {
        Idle,
        Move,
        Follow,
        Patrol,
        Cover,
        Shot,
        Aim
    }

    private void SetTarget(Collider collider)
    {
        if (collider.tag == "Player" && moving == null)
        {
            state = EnemyState.Follow;
            target = collider.gameObject;
        }
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case EnemyState.Follow:
                Aim(target);
                Follow(target);
                break;
            case EnemyState.Shot:
                Shot();
                break;
        }

        Debug.Log(state);
    }

    private IEnumerator ShotRoutine()
    {
        while (true)
        {
            GameObject goProj = Instantiate(projectile);
            goProj.transform.position = nozzle.transform.position;
            goProj.transform.rotation = nozzle.transform.rotation;
            goProj.transform.parent = null;
            yield return new WaitForSeconds(fireRate);
        }
    }

    public void Shot()
    {
        if (shooting == null)
            shooting = StartCoroutine(ShotRoutine());
    }

    private void Follow(GameObject target)
    {
        if (moving == null)
            moving = StartCoroutine(FollowRoutine(target));
    }
    private void Aim(GameObject target)
    {
        if (aiming == null)
            aiming = StartCoroutine(AimRoutine(target));
    }
    private IEnumerator AimRoutine(GameObject target)
    {
        while (state == EnemyState.Follow)
        {
            yield return new WaitForFixedUpdate();
            if (target == null)
                break;
            tower.transform.LookAt(target.transform.position);
            if (Vector3.Distance(tower.transform.position, target.transform.position) <= range)
            {
                Shot();
            }
        }
    }

    private IEnumerator FollowRoutine(GameObject target)
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

    }
    private void OnTriggerEnter(Collider other)
    {
        SetTarget(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            state = EnemyState.Idle;
        }

        moving = null;
        aiming = null;
        shooting = null;
    }
}