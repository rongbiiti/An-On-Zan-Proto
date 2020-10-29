using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLight : MonoBehaviour
{
    [SerializeField] Light _light;
    [SerializeField] private float _lightFadeOutTime = 2f;
    private float startLightIntensity;
    private float time;
    public bool startFlg;

    // Start is called before the first frame update
    void Start()
    {
        startLightIntensity = _light.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (time < _lightFadeOutTime && startFlg)
        {
            time += Time.deltaTime;
            _light.intensity -= startLightIntensity / _lightFadeOutTime * Time.deltaTime;
            if(_lightFadeOutTime <= time)
            {
                startFlg = false;
            }
        }
    }
}
