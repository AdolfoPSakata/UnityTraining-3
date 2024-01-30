using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    [SerializeField] private GameObject parent;
    [SerializeField] private Vector3 offset;
    void Update()
    {
        obj.transform.rotation = Camera.main.transform.rotation;
        obj.transform.position = parent.transform.position + offset;
    }
}
