using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITank
{
    public void UpdateTankConfigs(TankConfig tankConfig);
    public void UpdateTankMeshs(TankConfig tankConfig);
}
