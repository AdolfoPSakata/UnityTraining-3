using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
    private Rigidbody rb;
    public UnityAction<float> onMove;
    public UnityAction<bool> onPressButton;
    private bool isPressed;

    private void Start()
    {
        onMove = StartMoving;
        onPressButton = ChangePressedStatus;
        Setup();
    }
    private void Setup()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void ChangePressedStatus(bool state)
    {
        isPressed = state;
    }

    public void StartMoving(float direction)
    {
        StartCoroutine(Move(direction));
    }

    public IEnumerator Move(float direction)
    {
        ChangePressedStatus(true);
        while (isPressed)
        {
            yield return null;
            rb.AddForce(gameObject.transform.forward * direction * 15f, ForceMode.Force);
        }
    }
}
