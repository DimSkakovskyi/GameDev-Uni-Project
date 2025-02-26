using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UserInput : MonoBehaviour
{
    public Light2D _lightRef;
    LightSwitch _lightswitch;
    public bool isCollidingWithLight = false;
    void Start()
    {
        _lightRef = GetComponent<Light2D>();
        ICommand turnOnCommand = new TogglePowerCommand(_lightRef);
        _lightswitch = new LightSwitch(turnOnCommand);
    }

    void Update()
    {
        // Require both collision and "E" press
        if (isCollidingWithLight && Input.GetKeyDown(KeyCode.E))
        {
            _lightswitch.TogglePower();
        }
    }

    // Detect when player enters the trigger zone
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure collider belongs to the player
        {
            isCollidingWithLight = true;
        }
    }

    // Detect when player exits the trigger zone
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isCollidingWithLight = false;
        }
    }
}