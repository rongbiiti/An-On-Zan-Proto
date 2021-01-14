using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]

public class PlayerDeathProcess : MonoBehaviour
{
    Animator animator;
    NavMeshAgent meshAgent;
    Rigidbody rb;
    CapsuleCollider capsule;
    EnemyMove enemyMove;
    FirstPersonAIO firstPersonAIO;
    FPSMove fPSMove;
    PauseManager _pauseManager;
    [SerializeField] AudioSource _breathSource;
    [SerializeField] GameObject _katana;
    [SerializeField] GameObject _camera;
    [SerializeField] Transform[] meshs;
    [SerializeField] GameObject _bloodEffect;
    [SerializeField] public List<AudioClip> _deathVoiceClip = null;
    AudioSource rootAudioSource;
    public GameObject reflectionprobe;

    private BoxCollider[] weaponColliders;

    void Start()
    {
        animator = GetComponent<Animator>();
        meshAgent = GetComponent<NavMeshAgent>();
        enemyMove = GetComponent<EnemyMove>();
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        firstPersonAIO = GetComponent<FirstPersonAIO>();
        fPSMove = GetComponent<FPSMove>();
        rootAudioSource = GetComponent<AudioSource>();
        _pauseManager = GameObject.Find("Canvas").GetComponent<PauseManager>();
        reflectionprobe = GameObject.Find("Reflection Probe");
        weaponColliders = _katana.GetComponentsInChildren<BoxCollider>();

        MeshtoOne();
    }

    public void MeshtoOne()
    {
        foreach (Transform mesh in meshs) {
            mesh.localScale = Vector3.one;
        }
    }

    public void MeshtoZero()
    {
        foreach (Transform mesh in meshs) {
            mesh.localScale = Vector3.zero;
        }
    }

    // CPU用
    public void KillPlayer()
    {
        meshAgent.enabled = false;
        enemyMove.enabled = false;
        foreach (var wpcol in weaponColliders) {
            wpcol.enabled = false;
        }
        _breathSource.enabled = false;
        gameObject.tag = "Untagged";
        gameObject.layer = LayerMask.NameToLayer("DeadBoddy");
        InstantiateBloodEffect();
        ZanAnimation.flg = true;
        replay.PlayerDeathflg = true;
        _pauseManager.isCanPause = false;
        StartCoroutine("PlayDeathVoice");
        reflectionprobe.SetActive(true);
    }

    // プレイヤー用
    public void KillPlayer_Net()
    {
        MeshtoOne();
        firstPersonAIO.enabled = false;
        fPSMove.enabled = false;
        _breathSource.enabled = false;
        ZanAnimation.flg = true;
        replay.PlayerDeathflg = true;
        _pauseManager.isCanPause = false;
        gameObject.tag = "Untagged";
        gameObject.layer = LayerMask.NameToLayer("DeadBoddy");
        foreach (var wpcol in weaponColliders) {
            wpcol.enabled = false;
        }
        rb.velocity = Vector3.zero;
        InstantiateBloodEffect();
        StartCoroutine("PlayDeathVoice");
        reflectionprobe.SetActive(true);

        if (_camera.activeSelf) {
            _camera.GetComponent<ExecutionCamera>().StartCoroutine("Execution");
            GameObject.Find("Directional Light").GetComponent<Light>().intensity = 1;
        }
        
    }

    public void InstantiateBloodEffect()
    {
        //_bloodEffect.SetActive(true);
    }

    private IEnumerator PlayDeathVoice()
    {
        yield return new WaitForSeconds(0.17f);
        int n = Random.Range(0, _deathVoiceClip.Count);
        if (_deathVoiceClip.Any() && _deathVoiceClip[n] != null) {
            rootAudioSource.PlayOneShot(_deathVoiceClip[n]);
        }
        
    }
}