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
    [SerializeField] private AudioClip _shinkuuhaAudio;
    [SerializeField] private Transform _playerCameraTransform;

    AudioSource audioSource;
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
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F1)) {
            Shinkuuha = !Shinkuuha;
        }
        if (Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.J) && photonView.IsMine) {
            photonView.RPC("Invincible", RpcTarget.AllViaServer, photonView.ViewID);
        }
        if (Input.GetKey(KeyCode.B) && Input.GetKeyDown(KeyCode.G) && photonView.IsMine) {
            photonView.RPC("Biggers", RpcTarget.AllViaServer, photonView.ViewID);
        }
        if (Input.GetKey(KeyCode.B) && Input.GetKeyDown(KeyCode.H) && !photonView.IsMine) {
            photonView.RPC("Smallers", RpcTarget.AllViaServer, photonView.ViewID);
        }
        if (Input.GetKey(KeyCode.I) && Input.GetKeyDown(KeyCode.D) && !photonView.IsMine)
        {
            photonView.RPC("MusicStart", RpcTarget.AllViaServer, photonView.ViewID);
        }

    }

    public void AttackStart()
    {
        weaponCollider.enabled = true;
        Debug.Log("攻撃判定ON");
        if (isCPU) return;
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
        animator.SetBool("Attack", false);
    }

    public void EffectStop()
    {
        effect.SetActive(false);
        particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        if (isCPU) return;
        if(!animator.GetBool("Death"))
        {
            firstPersonAIO.playerCanMove = true;
            firstPersonAIO.enableCameraMovement = true;
        }
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
        audioSource.PlayOneShot(_shinkuuhaAudio);
    }

    public void ShinkuuhaLauntchEnemy()
    {
        Vector3 initPos = transform.position + transform.forward * 2f;
        initPos.y += 1.2f;
        GameObject shinkuuha = Instantiate(_shinkuuhaPrefab, initPos, transform.rotation);
        HitProcess hitProcess = shinkuuha.GetComponent<HitProcess>();
        hitProcess._parentMaterialChanger = GetComponent<MaterialChanger>();
        hitProcess._parent = gameObject;
        audioSource.PlayOneShot(_shinkuuhaAudio);
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
        audioSource.PlayOneShot(_shinkuuhaAudio);
    }

    [PunRPC]
    public void Invincible(int id)
    {
        if(id != photonView.ViewID) {
            return;
        }
        Debug.Log("無敵状態" + photonView.ViewID);
        gameObject.tag = "Untagged";
        gameObject.layer = LayerMask.NameToLayer("DeadBoddy");
    }

    [PunRPC]
    public void Biggers(int id)
    {
        if (id != photonView.ViewID) {
            return;
        }
        Debug.Log("巨大化" + photonView.ViewID);
        transform.localScale += new Vector3(2, 2, 2);
        GetComponent<MaterialChanger>().MaterialOn();
    }

    [PunRPC]
    public void Smallers(int id)
    {
        if (id != photonView.ViewID) {
            return;
        }
        Debug.Log("巨大化" + photonView.ViewID);
        transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);
        GetComponent<MaterialChanger>().MaterialOn();
    }

    [PunRPC]
    public void MusicStart(int id)
    {
        if (id != photonView.ViewID || !photonView.IsMine)
        {
            return;
        }
        Debug.Log("ミュージックスタート" + photonView.ViewID);
        GameObject.Find("IsyadouTheme").GetComponent<AudioSource>().Play();
    }

}
