using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CannonConfig", menuName = "PartsConfig/Cannon", order = 3)]
public class CannonConfig : ScriptableObject
{
    public Mesh mesh;
    public float weight;
    public float range;
    public float fireRate;
}