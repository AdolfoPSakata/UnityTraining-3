using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float angle = 0.1f;
    void Update()
    {
        gameObject.transform.Rotate(Vector3.up, angle, Space.Self);
    }
}
