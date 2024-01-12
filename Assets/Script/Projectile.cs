using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    private float velocity;
    private void Start()
    {
        Setup();
        StartMoving();
    }
    private void Setup()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    public void StartMoving()
    {
        rb.velocity = gameObject.transform.forward * 50f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
