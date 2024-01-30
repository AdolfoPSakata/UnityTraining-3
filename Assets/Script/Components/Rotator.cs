using System.Collections;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private bool isRotating;
    private float maneuverability;
    private Coroutine rotating;
    public void ChangePressedStatus(bool state)
    {
        isRotating = state;
    }
    public virtual void StartRotation(float interestPoint)
    {
        StartCoroutine(Rotate(interestPoint));
    }

    private IEnumerator Rotate(float direction)
    {
        ChangePressedStatus(true);
        while (isRotating)
        {
            yield return null;
            gameObject.transform.Rotate(Vector3.up, direction * maneuverability);
        }
    }
    public void SetInitialConfig(float maneuverability)
    {
        this.maneuverability = maneuverability;
    }
}
