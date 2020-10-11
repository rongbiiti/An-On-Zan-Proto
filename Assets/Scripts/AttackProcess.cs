using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackProcess : MonoBehaviour
{

    [SerializeField]private BoxCollider weaponCollider;
    [SerializeField]private GameObject effect;

    float preMouseSensitibity;
    float preMoveSpeed;
    float preSprintSpeed;
    FirstPersonAIO firstPersonAIO;
    private Animator animator;

    private void Start()
    {
        effect.SetActive(false);
        animator = GetComponent<Animator>();
        firstPersonAIO = GetComponent<FirstPersonAIO>();
        preMouseSensitibity = firstPersonAIO.mouseSensitivity;
        preMoveSpeed = firstPersonAIO.walkSpeed;
        preSprintSpeed = firstPersonAIO.sprintSpeed;
    }

    void AttackStart()
    {
        weaponCollider.enabled = true;
        Debug.Log("攻撃判定ON");
        effect.SetActive(true);
        firstPersonAIO.mouseSensitivity = 0f;
        firstPersonAIO.walkSpeed = 0f;
        firstPersonAIO.sprintSpeed = 0f;
    }

    void AttackEnd()
    {
        weaponCollider.enabled = false;
        Debug.Log("攻撃判定OFF");
        animator.SetBool("Attack", false);
    }

    void EffectStop()
    {
        effect.SetActive(false);
        firstPersonAIO.mouseSensitivity = preMouseSensitibity;
        firstPersonAIO.walkSpeed = preMoveSpeed;
        firstPersonAIO.sprintSpeed = preSprintSpeed;
    }
    
}
