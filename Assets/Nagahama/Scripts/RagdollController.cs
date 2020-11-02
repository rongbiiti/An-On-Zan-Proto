using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]

public class RagdollController : MonoBehaviour
{
    Animator animator;
    NavMeshAgent meshAgent;
    Rigidbody[] ragdollRigidbodies;
    Collider[] colliders;
    Rigidbody rb;
    CapsuleCollider capsule;
    EnemyMove enemyMove;
    FirstPersonAIO firstPersonAIO;
    FPSMove fPSMove;
    [SerializeField] AudioSource _breathSource;
    [SerializeField] BoxCollider swordCollider;
    [SerializeField] GameObject _camera;
    [SerializeField] Transform[] meshs;
    [SerializeField] GameObject _bloodEffect;

    void Start()
    {
        animator = GetComponent<Animator>();
        meshAgent = GetComponent<NavMeshAgent>();
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        enemyMove = GetComponent<EnemyMove>();
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        firstPersonAIO = GetComponent<FirstPersonAIO>();
        fPSMove = GetComponent<FPSMove>();
        //SetRagdoll(false, Vector3.zero);
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
    public void RagdollActive()
    {
        meshAgent.enabled = false;
        enemyMove.enabled = false;
        swordCollider.enabled = false;
        _breathSource.enabled = false;
        gameObject.tag = "Untagged";
        gameObject.layer = LayerMask.NameToLayer("DeadBoddy");
        InstantiateBloodEffect();
    }

    // プレイヤー用
    public void RagdollActive_Net()
    {
        MeshtoOne();
        firstPersonAIO.enabled = false;
        fPSMove.enabled = false;
        _breathSource.enabled = false;
        gameObject.tag = "Untagged";
        gameObject.layer = LayerMask.NameToLayer("DeadBoddy");
        swordCollider.enabled = false;
        rb.velocity = Vector3.zero;
        InstantiateBloodEffect();

        if (_camera.activeSelf) {
            _camera.GetComponent<ExecutionCamera>().StartCoroutine("Execution");
            GameObject.Find("Directional Light").GetComponent<Light>().intensity = 1;
        }
        
    }

    public void InstantiateBloodEffect()
    {
        _bloodEffect.SetActive(true);
    }

    private void OnParticleCollision(GameObject other)
    {
        
    }
}