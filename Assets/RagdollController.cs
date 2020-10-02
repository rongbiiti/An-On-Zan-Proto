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
    Rigidbody rb;
    CapsuleCollider capsule;
    EnemyMove enemyMove;

    void Start()
    {
        animator = GetComponent<Animator>();
        meshAgent = GetComponent<NavMeshAgent>();
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        enemyMove = GetComponent<EnemyMove>();
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        SetRagdoll(false, Vector3.zero);
    }

    void SetRagdoll(bool isEnabled, Vector3 direction)
    {
        foreach (Rigidbody rigidbody in ragdollRigidbodies) {
            rigidbody.isKinematic = !isEnabled;
            rigidbody.AddForce(direction * 5f, ForceMode.Impulse);
        }
    }

    public void RadollActive(Vector3 direction)
    {
        SetRagdoll(true, direction);
        animator.enabled = false;
        meshAgent.enabled = false;
        enemyMove.enabled = false;
        rb.AddForce(direction * 100f, ForceMode.Impulse);
        capsule.enabled = false;
        
    }
}