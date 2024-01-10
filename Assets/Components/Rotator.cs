using System.Collections;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private bool isRotating;
    private Coroutine rotating;
    protected void ChangePressedStatus(bool state)
    {
        isRotating = state;
    }

    protected virtual void StartRotation(float interestPoint)
    {
        StartCoroutine(Rotate(interestPoint));
    }

    protected IEnumerator Rotate(float direction)
    {
        ChangePressedStatus(true);
        while (isRotating)
        {
            yield return null;
            gameObject.transform.Rotate(Vector3.up, direction);
        }
    }

    protected float DirectionToPOI(Transform pointOfInterest)
    {
        return Vector3.Angle(gameObject.transform.position, pointOfInterest.position);
    }
}
