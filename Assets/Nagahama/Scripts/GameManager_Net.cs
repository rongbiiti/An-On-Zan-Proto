using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager_Net : MonoBehaviour
{
    public Light _directionalLight; // シーン上の太陽光
    public float _fpsCameraEnableWaitTime = 2f;    // FPSカメラがONになるまでの時間
    public float _fpsCameraLightTime = 4f;
    public GameObject _player;      // 自分が操作権を持つキャラ
    public GameObject _enemy;
    public GameObject _camera;      // シーン上のメインカメラ
    public GameObject _fasttext;
    public GameObject[] _candle;    // シーン上のろうそく
    public float zoomValue = 20f;   // どのくらいカメラが落ちていくか
    public Result _result;

    private NavMeshAgent navMesh;
    private EnemyMove enemyMove;
    private PauseManager pauseManager;
    private float time = 0f;
    private bool startedFlg = false;
    private Animator anim;

    public bool _isCPUMatch;

    private void Start()
    {
        if(_isCPUMatch) {
            navMesh = _enemy.GetComponent<NavMeshAgent>();
            enemyMove = _enemy.GetComponent<EnemyMove>();
            navMesh.enabled = false;
            enemyMove.enabled = false;
            //_fasttext = GameObject.Find("Image");
            
        }
        pauseManager = GameObject.Find("Canvas").GetComponent<PauseManager>();
        pauseManager.isCanPause = false;
        anim = _fasttext.GetComponent<Animator>();
    }

    // 試合開始時に別スクリプトから呼ばれる
    public void MatchStart()
    {
        _result._fpsAIO = _player.GetComponent<FirstPersonAIO>();
        _result._fpsMove = _player.GetComponent<FPSMove>();
        _player.GetComponent<FirstPersonAIO>().enabled = false;
        _player.GetComponent<FPSMove>().enabled = false;
        _directionalLight.intensity = 1;
        startedFlg = true;
        _player.transform.GetChild(3).gameObject.SetActive(false);  // 呼吸音を出すスピーカーON
    }

    private void FixedUpdate()
    {
        if (time < _fpsCameraEnableWaitTime && startedFlg)
        {
            time += Time.deltaTime;

            // カメラが落ちていく
            _camera.transform.position -= new Vector3(0, zoomValue / _fpsCameraEnableWaitTime * Time.deltaTime, 0);
            if(_fpsCameraEnableWaitTime <= time)
            {
                _player.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);   // FPSカメラON
                anim.SetBool("CameraTrigger",true);
                
                _camera.GetComponent<AudioListener>().enabled = false;
                if (_isCPUMatch)
                {
                    _enemy.GetComponent<EnemyLight>().startFlg = true;
                }
            }
            
        }
        else if (time < _fpsCameraLightTime && startedFlg)
        {
            time += Time.deltaTime;
            _directionalLight.intensity -= 1 / _fpsCameraEnableWaitTime * Time.deltaTime;

            for (int i = 0; i<4; i++) { 
                //ろうそくの火を消す
                _candle[i].GetComponent<Light>().intensity -= 1.37f / _fpsCameraEnableWaitTime * Time.deltaTime;
            }

            // カメラOFF
            //_camera.SetActive(false);
            if (_fpsCameraLightTime <= time)
            {
                PlayerActive();
                if (_isCPUMatch)
                {
                    EnemyActive();
                    
                }
                pauseManager.isCanPause = true;
            }
           
        }
    }

    // キャラを操作可能にしたいときに呼ばれる
    public void PlayerActive()
    {
        _player.GetComponent<FirstPersonAIO>().enabled = true;
        _player.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);   // FPSカメラON
        _player.GetComponent<FPSMove>().enabled = true;
        _player.GetComponent<PlayerDeathProcess>().MeshtoZero();     // FPS用に足と頭を縮ませる
        StartCoroutine(nameof(DelayAudioEnable));
        Vector3 lookPos = Vector3.zero;
        lookPos.y = _player.transform.position.y;
        _player.transform.LookAt(lookPos);
    }

    public void EnemyActive()
    {
        navMesh.enabled = true;
        enemyMove.enabled = true;
        enemyMove._player = _player;
        enemyMove.GetPlayerComponent();
        StartCoroutine(nameof(DelayAudioEnable_CPU));
    }

    private IEnumerator DelayAudioEnable()
    {
        yield return new WaitForSeconds(0.3f);
        _player.GetComponent<AudioSource>().volume = 1;
    }

    private IEnumerator DelayAudioEnable_CPU()
    {
        yield return new WaitForSeconds(0.3f);
        _enemy.GetComponent<AudioSource>().volume = 1;
    }

}