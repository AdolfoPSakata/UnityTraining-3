using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Shoter : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject nozzle;
    [SerializeField] private GameObject tower;
    public UnityAction onShot;
    private int ammo;

    private void Awake()
    {
        ammo = 10;
        onShot = Shot;
    }
    private void FixedUpdate()
    {
        RotateBarrel();
    }
    public void Shot()
    {
        if (ammo > 0)
        {
            GameObject goProj = Instantiate(projectile);
            goProj.transform.position = nozzle.transform.position;
            goProj.transform.rotation = nozzle.transform.rotation;
            goProj.transform.parent = null;
        }
    }

    public void AddAmmo(int recievedAmmo)
    {
        ammo += recievedAmmo;
    }

    public void RotateBarrel()
    {
        Vector3 finalPoint = Camera.main.ScreenPointToRay(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono).GetPoint(20);
        finalPoint = new Vector3(finalPoint.x, tower.transform.position.y, finalPoint.z);
        tower.transform.LookAt(finalPoint);
    }

    private void UpdateAmmoAmount(TMP_Text ammo, int value)
    {
        ammo.text = value.ToString();
    }
}
