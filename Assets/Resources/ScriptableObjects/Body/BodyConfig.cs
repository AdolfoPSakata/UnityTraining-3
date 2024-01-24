using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BodyConfig", menuName = "PartsConfig/Body", order = 2)]
public class BodyConfig : ScriptableObject
{
    public Mesh mesh;
    public float health;
    public float weight;
    public float maneuverability;
}

