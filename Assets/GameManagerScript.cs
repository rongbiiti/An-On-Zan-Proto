using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManagerScript : MonoBehaviour
{

    public Light _directionalLight;
    public float _fadeTime = 2f;
    public GameObject _player;
    public GameObject _enemy;
    public GameObject _camera;
    public float zoomValue = 20f;

    private float time = 0f;
    private float firstCameraZ;

    void Start()
    {
        _player.GetComponent<MoveBehaviour>().enabled = false;
        _enemy.GetComponent<NavMeshAgent>().enabled = false;
        _enemy.GetComponent<EnemyMove>().enabled = false;

        _camera.GetComponent<ThirdPersonOrbitCamBasic>().enabled = false;
        firstCameraZ = _camera.transform.position.z;
        _camera.transform.localPosition = new Vector3(0, 8, 0);
        _camera.transform.localEulerAngles = new Vector3(89, 0, 0);
        _camera.transform.position += new Vector3(0, zoomValue, 0);
    }

    private void FixedUpdate()
    {
        if(time < _fadeTime) {
            time += Time.deltaTime;
            _directionalLight.intensity -= 1 / _fadeTime * Time.deltaTime;
            _camera.transform.position -= new Vector3(0, zoomValue / _fadeTime * Time.deltaTime, 0);
        } else {
            _player.GetComponent<MoveBehaviour>().enabled = true;
            _enemy.GetComponent<NavMeshAgent>().enabled = true;
            _enemy.GetComponent<EnemyMove>().enabled = true;
            _camera.GetComponent<ThirdPersonOrbitCamBasic>().enabled = true;
        }
    }
}
