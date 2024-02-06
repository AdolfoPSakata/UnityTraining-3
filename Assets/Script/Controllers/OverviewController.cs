using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverviewController : MonoBehaviour
{
    [Header("Body Parts")]
    [SerializeField] private ScriptableObject[] bodyConfigs;
    [SerializeField] private ScriptableObject[] cannonConfigs;
    [SerializeField] private ScriptableObject[] turretConfigs;
    [SerializeField] private ScriptableObject[] wheelConfigs;
    [SerializeField] private TankConfig playerTank;

    [Header("Mesh Filters")]
    [SerializeField] private MeshFilter bodyMesh;
    [SerializeField] private MeshFilter cannonMesh;
    [SerializeField] private MeshFilter turretMesh;
    [SerializeField] private MeshFilter wheelMesh;

    [Header("UI Texts")]
    [SerializeField] private TMP_Text healthValue;
    [SerializeField] private TMP_Text weightValue;
    [SerializeField] private TMP_Text rangeValue;
    [SerializeField] private TMP_Text fireValue;
    [SerializeField] private TMP_Text speedValue;
    [SerializeField] private TMP_Text maneuvValue;

    [Header("UI Buttons")]
    [SerializeField] private Button bodyR;
    [SerializeField] private Button bodyL;
    [SerializeField] private Button turretR;
    [SerializeField] private Button turretL;
    [SerializeField] private Button cannonR;
    [SerializeField] private Button cannonL;
    [SerializeField] private Button wheelR;
    [SerializeField] private Button wheelL;

    private int bodyIndex = 0;
    private int cannonIndex = 0;
    private int turretIndex = 0;
    private int wheelIndex = 0;
    private void Start()
    {
        bodyIndex = 0;
        cannonIndex = 0;
        turretIndex = 0;
        wheelIndex = 0;

        UpdatePlayerConfig(playerTank);

        bodyR.onClick.AddListener(delegate { AddPage(bodyConfigs, ref bodyIndex); });
        bodyL.onClick.AddListener(delegate { SubTractPage(bodyConfigs, ref bodyIndex); });
        cannonR.onClick.AddListener(delegate { AddPage(cannonConfigs, ref cannonIndex); });
        cannonL.onClick.AddListener(delegate { SubTractPage(cannonConfigs, ref cannonIndex); });
        turretR.onClick.AddListener(delegate { AddPage(turretConfigs, ref turretIndex); });
        turretL.onClick.AddListener(delegate { SubTractPage(turretConfigs, ref turretIndex); });
        wheelR.onClick.AddListener(delegate { AddPage(wheelConfigs, ref wheelIndex); });
        wheelL.onClick.AddListener(delegate { SubTractPage(wheelConfigs, ref wheelIndex); });
    }

    private void AddPage(ScriptableObject[] parts, ref int index)
    {
        index++;

        if (index >= parts.Length)
        {
            index = 0;
        }
        SetParts(parts[index]);
    }

    private void SubTractPage(ScriptableObject[] parts, ref int index)
    {
        index--;
       
        if (index < 0)
        {
            index = parts.Length - 1;
        }
        SetParts(parts[index]);
    }

    private void SetParts(ScriptableObject part)
    {
        switch (part)
        {
            case BodyConfig:
                playerTank.body = (BodyConfig)part;
                break;
            case WheelConfig:
                playerTank.wheel = (WheelConfig)part;
                break;
            case CannonConfig:
                playerTank.cannon = (CannonConfig)part;
                break;
            case TurretConfig:
                playerTank.turret = (TurretConfig)part;
                break;
            default:
                break;
        }
        UpdatePlayerConfig(playerTank);
    }
    private void UpdatePlayerConfig(TankConfig tankConfig)
    {
        bodyMesh.mesh = tankConfig.body.mesh;
        cannonMesh.mesh = tankConfig.cannon.mesh;
        turretMesh.mesh = tankConfig.turret.mesh;
        wheelMesh.mesh = tankConfig.wheel.mesh;
         
        healthValue.text = (tankConfig.wheel.health + tankConfig.body.health + tankConfig.turret.health).ToString();
        weightValue.text = (tankConfig.wheel.weight + tankConfig.body.weight + tankConfig.cannon.weight + tankConfig.turret.weight).ToString();
        fireValue.text = (tankConfig.cannon.fireRate).ToString();
        speedValue.text = (tankConfig.wheel.speed).ToString();
        maneuvValue.text = (tankConfig.body.maneuverability).ToString();
    }
}
