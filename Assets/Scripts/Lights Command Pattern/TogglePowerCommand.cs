using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

//CONCRETE COMMAND
public class TogglePowerCommand : ICommand
{
    Light2D lightRef;

    //constructor to store Light2D reference
    public TogglePowerCommand(Light2D light)
    {
        lightRef = light;
    }
    public void Execute()
    {
        if (lightRef.intensity == 0) {
            lightRef.intensity = 1;
        }
        else
        {
            lightRef.intensity = 0;
        }
    }
}
