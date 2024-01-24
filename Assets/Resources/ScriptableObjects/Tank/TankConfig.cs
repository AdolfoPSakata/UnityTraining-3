using UnityEngine;

[CreateAssetMenu(fileName = "TankConfig", menuName = "PartsConfig/Tank", order = 0)]
public class TankConfig : ScriptableObject
{
    public WheelConfig wheel;
    public BodyConfig body;
    public CannonConfig cannon;
    public TurretConfig turret;
}
