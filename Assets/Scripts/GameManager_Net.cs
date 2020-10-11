using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager_Net : MonoBehaviour
{

    public Light _directionalLight;
    public float _fadeTime = 2f;
    public GameObject _player;
    public GameObject _camera;
    public float zoomValue = 20f;

    private float time = 0f;

    private bool startedFlg = false;

    public void MatchStart()
    {

        _player.GetComponent<FirstPersonAIO>().enabled = false;
        _player.GetComponent<FPSMove>().enabled = false;

        startedFlg = true;
    }

    private void FixedUpdate()
    {
        if (time < _fadeTime && startedFlg) {
            time += Time.deltaTime;
            _directionalLight.intensity -= 1 / _fadeTime * Time.deltaTime;
            _camera.transform.position -= new Vector3(0, zoomValue / _fadeTime * Time.deltaTime, 0);
            if (_fadeTime <= time) {
                _camera.SetActive(false);
                _player.GetComponent<FirstPersonAIO>().enabled = true;
                _player.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                _player.GetComponent<FPSMove>().enabled = true;
                _player.transform.GetChild(3).gameObject.SetActive(false);
                _player.GetComponent<RagdollController>().MeshtoZero();
            }
        }
    }
}
