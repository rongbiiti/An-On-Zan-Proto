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

    void SetRagdoll(bool isEnabled, Vector3 direction)
    {
        foreach (Rigidbody rigidbody in ragdollRigidbodies) {
            rigidbody.isKinematic = !isEnabled;
            rigidbody.useGravity = !isEnabled;
            rigidbody.AddForce(direction * 16f, ForceMode.Impulse);
        }
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

    public void RagdollActive(Vector3 direction)
    {
        SetRagdoll(true, direction);
        animator.enabled = false;
        meshAgent.enabled = false;
        enemyMove.enabled = false;
        capsule.enabled = false;
        rb.isKinematic = true;
        swordCollider.enabled = false;
    }

    public void RagdollActive_Net(Vector3 direction)
    {
        MeshtoOne();
        firstPersonAIO.enabled = false;
        fPSMove.enabled = false;
        _breathSource.enabled = false;
        gameObject.tag = "Untagged";
        swordCollider.enabled = false;

        if (_camera.activeSelf) {
            _camera.GetComponent<ExecutionCamera>().StartCoroutine("Execution");
            GameObject.Find("Directional Light").GetComponent<Light>().intensity = 1;
        }
        
    }    
}