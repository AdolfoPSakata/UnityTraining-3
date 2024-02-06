using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Shoter : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject nozzle;
    [SerializeField] private GameObject turret;
    [SerializeField] private int ammo;

    [Header("omponents")]
    [SerializeField] private AudioSource audioSource;

    public UnityAction onShot;

    private void Awake()
    {
        EventBus.ammoChangeEvent.Publish(new EventArgs(ammo));
        onShot = Shot;
    }
    private void FixedUpdate()
    {
        RotateBarrel();
    }
    public void SetInitialConfig(GameObject projectile)
    {
        this.projectile = projectile;
    }
    public void Shot()
    {
        if (ammo > 0)
        {
            GameObject goProj = Instantiate(projectile);
            audioSource.Play();
            goProj.transform.position = nozzle.transform.position;
            goProj.transform.rotation = nozzle.transform.rotation;
            goProj.transform.parent = null;
            ammo --;
            EventBus.ammoChangeEvent.Publish(new EventArgs(ammo));
            
        }
    }

    public void AddAmmo(int recievedAmmo)
    {
        ammo += recievedAmmo;
        EventBus.ammoChangeEvent.Publish(new EventArgs(ammo));
    }

    public void RotateBarrel()
    {
        Vector3 finalPoint = Camera.main.ScreenPointToRay(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono).GetPoint(20);
        finalPoint = new Vector3(finalPoint.x, turret.transform.position.y, finalPoint.z);
        turret.transform.LookAt(finalPoint);
    }

}
