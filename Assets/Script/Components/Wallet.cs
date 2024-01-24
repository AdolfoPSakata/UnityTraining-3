using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    private int bolts;

    public void AddBolts(int recievedBolts)
    {
        bolts += recievedBolts;
    }
}
