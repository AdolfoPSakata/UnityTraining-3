using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputMap inputMap;

    private void Start()
    {
       //inputMap = new InputMap();
        //inputMap.Gameloop.Fire.Enable();
    }
    public void Init()
    {
        inputMap.Gameloop.Fire.Enable();
    }
    //inputSystem.ScreenInput.UI.Disable();
}
