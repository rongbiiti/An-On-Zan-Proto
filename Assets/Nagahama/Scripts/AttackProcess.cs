using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttackProcess : MonoBehaviourPunCallbacks
{
    [SerializeField] private BoxCollider weaponCollider;
    [SerializeField] private GameObject effect;
    [SerializeField] ParticleSystem particle;
    [SerializeField] private bool isCPU;
    [SerializeField] private bool Shinkuuha;
    [SerializeField] private GameObject _shinkuuhaPrefab;
    [SerializeField] private string _shinkuuhaPrefabName;
    [SerializeField] private Transform _playerCameraTransform;

    float preMouseSensitibity;
    float preStickRotateSpeed;
    float preMoveSpeed;
    float preSprintSpeed;
    FirstPersonAIO firstPersonAIO;
    private Animator animator;
    private bool isAttacking;

    public bool IsAttacking
    {
        get { return isAttacking; }
    }

    private void Start()
    {
        effect.SetActive(false);
        particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        animator = GetComponent<Animator>();
        firstPersonAIO = GetComponent<FirstPersonAIO>();
        if (isCPU) return;
        preMouseSensitibity = firstPersonAIO.mouseSensitivity;
        preStickRotateSpeed = firstPersonAIO.stickRotateSpeed;
        preMoveSpeed = firstPersonAIO.walkSpeed;
        preSprintSpeed = firstPersonAIO.sprintSpeed;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyUp(KeyCode.F1)) {
            Shinkuuha = !Shinkuuha;
        }
    }

    public void AttackStart()
    {
        weaponCollider.enabled = true;
        Debug.Log("攻撃判定ON");
        if (isCPU) return;
        //firstPersonAIO.mouseSensitivity = 0f;
        //firstPersonAIO.stickRotateSpeed = 0f;
        //firstPersonAIO.walkSpeed = 0f;
        //firstPersonAIO.sprintSpeed = 0f;
        firstPersonAIO.playerCanMove = false;
        firstPersonAIO.enableCameraMovement = false;
        isAttacking = true;
        if (Shinkuuha && GetComponent<FPSMove>().isShinkuuha) {
            if (PhotonNetwork.InRoom) {
                ShinkuuhaLauntch_net();
            } else {
                ShinkuuhaLauntch();
            }
        }
    }

    public void AttackEnd()
    {
        weaponCollider.enabled = false;
        Debug.Log("攻撃判定OFF");
        animator.SetBool("Attack", false);
        
    }

    public void EffectStart()
    {
        effect.SetActive(true);
        particle.Play(true);
    }

    public void EffectStop()
    {
        effect.SetActive(false);
        particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        if (isCPU) return;
        //firstPersonAIO.mouseSensitivity = preMouseSensitibity;
        //firstPersonAIO.stickRotateSpeed = preStickRotateSpeed;
        //firstPersonAIO.walkSpeed = preMoveSpeed;
        //firstPersonAIO.sprintSpeed = preSprintSpeed;
        firstPersonAIO.playerCanMove = true;
        firstPersonAIO.enableCameraMovement = true;
        isAttacking = false;
        if (Shinkuuha && GetComponent<FPSMove>().isShinkuuha) {
            GetComponent<FPSMove>().isShinkuuha = false;
        }
    }

    public void ShinkuuhaLauntch()
    {
        Vector3 initPos = _playerCameraTransform.position + _playerCameraTransform.forward * 2f;
        GameObject shinkuuha = Instantiate(_shinkuuhaPrefab, initPos, _playerCameraTransform.rotation);
        HitProcess hitProcess = shinkuuha.GetComponent<HitProcess>();
        hitProcess._camera = _playerCameraTransform.gameObject;
        hitProcess._parentMaterialChanger = GetComponent<MaterialChanger>();
        hitProcess._parent = gameObject;
    }

    public void ShinkuuhaLauntchEnemy()
    {
        Vector3 initPos = transform.position + transform.forward * 2f;
        initPos.y += 1.2f;
        GameObject shinkuuha = Instantiate(_shinkuuhaPrefab, initPos, transform.rotation);
        HitProcess hitProcess = shinkuuha.GetComponent<HitProcess>();
        hitProcess._parentMaterialChanger = GetComponent<MaterialChanger>();
        hitProcess._parent = gameObject;
    }

    [PunRPC]
    public void ShinkuuhaLauntch_net()
    {
        Vector3 initPos = _playerCameraTransform.position + _playerCameraTransform.forward * 2f;
        GameObject shinkuuha = PhotonNetwork.Instantiate(_shinkuuhaPrefabName, initPos, _playerCameraTransform.rotation, 0);
        HitProcess hitProcess = shinkuuha.GetComponent<HitProcess>();
        hitProcess._camera = _playerCameraTransform.gameObject;
        hitProcess._parentMaterialChanger = GetComponent<MaterialChanger>();
        hitProcess._parent = gameObject;
    }

}
