using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

//CONCRETE COMMAND
public class TogglePowerCommand : ICommand
{
    Light2D lightRef;
    ParticleSystem _lightEffect;

    //constructor to store Light2D reference
    public TogglePowerCommand(Light2D light, ParticleSystem lightEffect)
    {
        lightRef = light;
        _lightEffect = lightEffect;
    }
    public void Execute()
    {
        if (lightRef.intensity == 0) {
            lightRef.intensity = 1;
            UpdateParticleEffect();
        }
        else
        {
            lightRef.intensity = 0;
            UpdateParticleEffect();
        }
    }

    private void UpdateParticleEffect()
    {
        if (_lightEffect == null)
        {
            Debug.LogError("ParticleSystem is missing!");
            return;
        }

        if (lightRef.intensity == 1)
        {
            if (!_lightEffect.isPlaying)
                _lightEffect.Play(); // Start effect
        }
        else
        {
            if (_lightEffect.isPlaying)
                _lightEffect.Stop(); // Stop effect
        }
    }
}
