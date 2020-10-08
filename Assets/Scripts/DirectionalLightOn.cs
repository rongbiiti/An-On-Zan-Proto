using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalLightOn : MonoBehaviour
{

    public Light directionalLight;

    public void LightOn()
    {
        if(directionalLight.intensity <= 0.1f) {
            directionalLight.intensity = 1f;
        } else if ( 0.1f <= directionalLight.intensity) {
            directionalLight.intensity = 0f;
        }
    }
}
