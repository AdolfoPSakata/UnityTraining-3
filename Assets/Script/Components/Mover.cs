using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
    private Rigidbody rb;
    private float speed;
    private bool isPressed;

    public UnityAction<float> onMove;
    public UnityAction<bool> onPressButton;

    private void Awake()
    {
        onMove = StartMoving;
        onPressButton = ChangePressedStatus;
        Setup();
    }
    private void Setup()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public void ChangePressedStatus(bool state)
    {
        isPressed = state;
    }

    public void StartMoving(float direction)
    {
        StartCoroutine(Move(direction));
    }
    private IEnumerator Move(float direction)
    {
        ChangePressedStatus(true);
        while (isPressed)
        {
            yield return null;
            rb.AddForce(gameObject.transform.forward * direction * speed , ForceMode.Impulse);
        }
    }

    public void FullStop()
    {
        rb.velocity = Vector3.zero;
    }

    public void SetInitialConfig(float weight, float speed)
    {
        rb.mass = weight;
        this.speed = speed;
    }
}
