using UnityEngine;

public class Tank : MonoBehaviour, ITank
{

    public MeshFilter bodyMesh;
    public MeshFilter cannonMesh;
    public MeshFilter turretMesh;
    public MeshFilter wheelMesh;

    [HideInInspector] public GameObject projectile;
    [HideInInspector] public float health;
    [HideInInspector] public float weight;
    [HideInInspector] public float responseTime;
    [HideInInspector] public float range;
    [HideInInspector] public float fireRate;
    [HideInInspector] public float speed;
    [HideInInspector] public float maneuverability;
    [HideInInspector] public float spotRange;

    [HideInInspector] public int bolts;
    [HideInInspector] public int ammo;

    public void Init(TankConfig tankConfig)
    {
        UpdateTankConfigs(tankConfig);
        UpdateTankMeshs(tankConfig);
    }

    public void UpdateTankConfigs(TankConfig tankConfig)
    {
        health = tankConfig.wheel.health + tankConfig.body.health + tankConfig.turret.health;
        weight = tankConfig.wheel.weight + tankConfig.body.weight + tankConfig.cannon.weight + tankConfig.turret.weight;
        spotRange = tankConfig.turret.spotRange;
        fireRate = tankConfig.cannon.fireRate;
        speed = tankConfig.wheel.speed;
        maneuverability = tankConfig.body.maneuverability;
        projectile = tankConfig.cannon.projectil;
#nullable enable
        responseTime = tankConfig.turret.responseTime;
        range = tankConfig.cannon.range;
#nullable disable
    }

    public void UpdateTankMeshs(TankConfig tankConfig)
    {
        bodyMesh.mesh = tankConfig.body.mesh;
        cannonMesh.mesh = tankConfig.cannon.mesh;
        turretMesh.mesh = tankConfig.turret.mesh;
        wheelMesh.mesh = tankConfig.wheel.mesh;
    }
}
