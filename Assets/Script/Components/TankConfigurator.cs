using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankConfigurator : ITankConfig
{
    public TankConfigurator(TankConfig tankConfig,ref float health, ref float weight, 
        ref float spotRange, ref float fireRate, ref float speed, ref float maneuverability)
    {
        UpdatePlayerConfig(tankConfig);
        health = this.health;
        weight = this.weight;
        spotRange = this.spotRange;
        fireRate = this.fireRate;
        speed = this.speed;
        maneuverability = this.maneuverability;
    }

    [SerializeField] private float health;
    [SerializeField] private float weight;
    [SerializeField] private float spotRange;
    [SerializeField] private float fireRate;
    [SerializeField] private float speed;
    [SerializeField] private float maneuverability;

    [SerializeField] private MeshFilter bodyMesh;
    [SerializeField] private MeshFilter cannonMesh;
    [SerializeField] private MeshFilter turretMesh;
    [SerializeField] private MeshFilter wheelMesh;

    public void UpdatePlayerConfig(TankConfig tankConfig)
    {
        health = tankConfig.wheel.health + tankConfig.body.health + tankConfig.turret.health;
        weight = tankConfig.wheel.weight + tankConfig.body.weight + tankConfig.cannon.weight + tankConfig.turret.weight;
        fireRate = tankConfig.cannon.fireRate;
        speed = tankConfig.wheel.speed;
        maneuverability = tankConfig.body.maneuverability;
        spotRange = tankConfig.turret.spotRange;
    }
}
