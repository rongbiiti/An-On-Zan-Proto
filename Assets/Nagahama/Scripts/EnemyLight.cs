using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLight : MonoBehaviour
{
    [SerializeField] Light _light;
    [SerializeField] private float _lightFadeOutTime = 1.67f;
    private float startLightIntensity;
    private float time;
    public bool startFlg;

    void Start()
    {
        startLightIntensity = _light.intensity;
    }

    private void FixedUpdate()
    {
        if (time < _lightFadeOutTime && startFlg)
        {
            time += Time.deltaTime;
            
            if(_lightFadeOutTime <= time)
            {
                _light.intensity = 0;
                startFlg = false;
            }
        }
    }
}
