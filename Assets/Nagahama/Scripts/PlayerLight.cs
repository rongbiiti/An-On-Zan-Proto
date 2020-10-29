using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    Light _light;
    public float _fadeTime = 2f;
    private float preIntensity;
    private float time = 0f;

    void Start()
    {
        _light = GetComponent<Light>();
        preIntensity = _light.intensity;
        _light.intensity += 2;
    }    

    private void FixedUpdate()
    {
        if (time < _fadeTime) {
            time += Time.deltaTime;
            _light.intensity -= preIntensity / _fadeTime * Time.deltaTime;
            if (_fadeTime <= time) {
                _light.enabled = false;
            }
        }
    }
}
