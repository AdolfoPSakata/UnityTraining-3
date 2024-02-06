using UnityEngine;

public class Wallet : MonoBehaviour
{
    private float bolts;
    private void Awake()
    {
        EventBus.boltChangeEvent.Publish(new EventArgs(bolts));
    }
    public void AddBolts(float recievedBolts)
    {
        bolts += recievedBolts;
        EventBus.boltChangeEvent.Publish(new EventArgs(bolts));
    }
}
