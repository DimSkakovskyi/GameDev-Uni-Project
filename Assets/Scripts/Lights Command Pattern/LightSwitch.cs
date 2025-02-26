using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch
{
    ICommand _onCommand;

    public LightSwitch(ICommand onCommand)
    {
        _onCommand = onCommand;
    }

    public void TogglePower()
    {
        _onCommand.Execute();
    }
}