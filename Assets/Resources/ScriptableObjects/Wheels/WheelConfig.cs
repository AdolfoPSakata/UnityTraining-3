using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WheelConfig", menuName = "PartsConfig/Wheel", order = 1)]
public class WheelConfig : ScriptableObject
{
    public Mesh mesh;
    public float health;
    public float weight;
    public float speed;
}
