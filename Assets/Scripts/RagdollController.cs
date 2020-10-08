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
    MoveBehaviour moveBehaviour;
    [SerializeField] BoxCollider swordCollider;

    void Start()
    {
        animator = GetComponent<Animator>();
        meshAgent = GetComponent<NavMeshAgent>();
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        enemyMove = GetComponent<EnemyMove>();
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        moveBehaviour = GetComponent<MoveBehaviour>();
        SetRagdoll(false, Vector3.zero);
    }

    void SetRagdoll(bool isEnabled, Vector3 direction)
    {
        foreach (Rigidbody rigidbody in ragdollRigidbodies) {
            rigidbody.isKinematic = !isEnabled;
            rigidbody.AddForce(direction * 16f, ForceMode.Impulse);
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
        SetRagdoll(true, direction);
        animator.enabled = false;
        moveBehaviour.enabled = false;
        rb.isKinematic = true;
        foreach (Collider col in colliders) {
            col.enabled = true;
        }
        capsule.enabled = false;
        swordCollider.enabled = false;

        if (transform.GetChild(0).gameObject.activeSelf) {
            transform.GetChild(0).GetComponent<ExecutionCamera>().StartCoroutine("Execution");
            GameObject.Find("Directional Light").GetComponent<Light>().intensity = 1;
        }
        
    }    
}