using UnityEngine;

public class PickUp : MonoBehaviour
{
    //renderer
    private Mesh model;
    [SerializeField] private int lifeSpawn;

    public float Health { get; private set; }
    public int Bolts { get; private set; }
    public int Ammo { get; private set; }

    private void Awake()
    {
        Setup();
        Destroy(gameObject, lifeSpawn);
    }

    private void Setup()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
