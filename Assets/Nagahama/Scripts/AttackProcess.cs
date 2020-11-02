using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackProcess : MonoBehaviour
{

    [SerializeField]private BoxCollider weaponCollider;
    [SerializeField]private GameObject effect;
    [SerializeField] private bool isCPU;
    [SerializeField] private bool Shinkuuha;
    [SerializeField] private GameObject _shinkuuhaPrefab;

    float preMouseSensitibity;
    float preStickRotateSpeed;
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
        preStickRotateSpeed = firstPersonAIO.stickRotateSpeed;
        preMoveSpeed = firstPersonAIO.walkSpeed;
        preSprintSpeed = firstPersonAIO.sprintSpeed;
    }

    public void AttackStart()
    {
        weaponCollider.enabled = true;
        Debug.Log("攻撃判定ON");
        effect.SetActive(true);
        if (isCPU) return;
        //firstPersonAIO.mouseSensitivity = 0f;
        //firstPersonAIO.stickRotateSpeed = 0f;
        //firstPersonAIO.walkSpeed = 0f;
        //firstPersonAIO.sprintSpeed = 0f;
        firstPersonAIO.playerCanMove = false;
        firstPersonAIO.enableCameraMovement = false;
        if (Shinkuuha && GetComponent<FPSMove>().isShinkuuha) {
            Vector3 initPos = transform.position + transform.forward * 2f;
            GameObject shinkuuha = Instantiate(_shinkuuhaPrefab, initPos, transform.rotation);
            HitProcess hitProcess = shinkuuha.GetComponent<HitProcess>();
            hitProcess._camera = transform.GetChild(0).GetChild(0).gameObject;
            hitProcess._parentMaterialChanger = GetComponent<MaterialChanger>();
            hitProcess._parent = gameObject;
        }
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
        //firstPersonAIO.mouseSensitivity = preMouseSensitibity;
        //firstPersonAIO.stickRotateSpeed = preStickRotateSpeed;
        //firstPersonAIO.walkSpeed = preMoveSpeed;
        //firstPersonAIO.sprintSpeed = preSprintSpeed;
        firstPersonAIO.playerCanMove = true;
        firstPersonAIO.enableCameraMovement = true;
        if (Shinkuuha && GetComponent<FPSMove>().isShinkuuha) {
            GetComponent<FPSMove>().isShinkuuha = false;
        }
    }
    
}
