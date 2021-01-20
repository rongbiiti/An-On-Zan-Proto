using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMove : MonoBehaviour
{
    private Animator animator;
    private FirstPersonAIO fpsController;
    private AttackProcess attackProcess;

    public bool isShinkuuha;

    void Start()
    {
        animator = GetComponent<Animator>();
        fpsController = GetComponent<FirstPersonAIO>();
        attackProcess = GetComponent<AttackProcess>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Attack") && !animator.GetBool(Animator.StringToHash("Attack")) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !attackProcess.IsAttackInterval) {
            animator.SetBool("Attack", true);
            StartCoroutine("AttackBoolControll");
        }

        /* if (Input.GetButtonDown("Shinkuuha") && !animator.GetBool(Animator.StringToHash("Attack")) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !attackProcess.IsAttackInterval) {
            animator.SetBool("Attack", true);
            StartCoroutine("AttackBoolControll");
            isShinkuuha = true;
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.J)) {
            fpsController.canJump = !fpsController.canJump;
        } */

    }

    private IEnumerator AttackBoolControll()
    {
        yield return new WaitForSeconds(0.9f);
        animator.SetBool("Attack", false);
    }
}
