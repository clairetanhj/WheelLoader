using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NPOI.SS.Formula.Functions;
using UnityEngine;
using WSMGameStudio.HeavyMachinery;

// [RequireComponent(typeof(WheelLoaderPlayerInput))]

public class MainController : MonoBehaviour
{
    public enum InputMode { MQTT, Manual }

    public WheelLoaderPlayerInput playerInputController;
    public InputMode inputMode;
    public MqttReceiver mqttReceiver;
    public List<BaseController> baseControllerList;
    

    private void Update()
    {
        switch (inputMode)
        {
            case InputMode.Manual:
                playerInputController.enablePlayerInput = true;
                mqttReceiver.Disable();
                break;
            case InputMode.MQTT:
                playerInputController.enablePlayerInput = false;
                mqttReceiver.Enable();
                break;
        }
    }
}
