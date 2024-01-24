using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretConfig", menuName = "PartsConfig/Turret", order = 4)]
public class TurretConfig : ScriptableObject
{
    public Mesh mesh;
    public float health;
    public float weight;
    public float responseTime;
    public float spotRange;
}
