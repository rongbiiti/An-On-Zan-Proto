using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackProcess : MonoBehaviour
{

    [SerializeField]private BoxCollider weaponCollider;
    [SerializeField]private GameObject effect;
    [SerializeField] private bool isCPU;

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
        if (isCPU) return;
        preMouseSensitibity = firstPersonAIO.mouseSensitivity;
        preMoveSpeed = firstPersonAIO.walkSpeed;
        preSprintSpeed = firstPersonAIO.sprintSpeed;
    }

    public void AttackStart()
    {
        weaponCollider.enabled = true;
        Debug.Log("攻撃判定ON");
        effect.SetActive(true);
        if (isCPU) return;
        firstPersonAIO.mouseSensitivity = 0f;
        firstPersonAIO.walkSpeed = 0f;
        firstPersonAIO.sprintSpeed = 0f;
    }

    public void AttackEnd()
    {
        weaponCollider.enabled = false;
        Debug.Log("攻撃判定OFF");
        animator.SetBool("Attack", false);
    }

    public void EffectStop()
    {
        effect.SetActive(false);
        if (isCPU) return;
        firstPersonAIO.mouseSensitivity = preMouseSensitibity;
        firstPersonAIO.walkSpeed = preMoveSpeed;
        firstPersonAIO.sprintSpeed = preSprintSpeed;
    }
    
}
