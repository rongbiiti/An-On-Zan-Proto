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
    [SerializeField] private float _attackIntervalResetTime = 2.21f;

    AudioSource audioSource;
    FirstPersonAIO firstPersonAIO;
    private Animator animator;
    private bool isAttacking;
    private bool isAttackInterval;
    private float attackIntervalTime;

    public bool IsAttacking
    {
        get { return isAttacking; }
    }

    public bool IsAttackInterval
    {
        get { return isAttackInterval; }
    }

    private void Awake()
    {
        effect.SetActive(false);
        particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (isCPU) return;

        firstPersonAIO = GetComponent<FirstPersonAIO>();
        audioSource = GetComponent<AudioSource>();
    }

    /* private void Update()
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

    } */

    private void FixedUpdate()
    {
        if(0 < attackIntervalTime) {
            attackIntervalTime -= Time.deltaTime;
            if(attackIntervalTime <= 0) {
                isAttackInterval = false;
            }
        }
    }

    public void EffectStart()
    {
        effect.SetActive(true);
        particle.Play(true);
        if (isCPU) return;
        firstPersonAIO.playerCanMove = false;

        attackIntervalTime += _attackIntervalResetTime;
        isAttackInterval = true;
    }

    public void AttackStart()
    {
<<<<<<< HEAD
        foreach(var wpcol in weaponColliders) {
            wpcol.enabled = true;
        }
=======
        weaponCollider.enabled = true;
>>>>>>> parent of 5201b18 (刀のコライダーを正確にした。キャラクターのコライダーを小さくした。これによりファイナル判定になりづらくなった。試合開始時光が消えたとき、相手の色が一瞬で消えるのではなく1秒かけてJOJOに消えるように変更した。ネット対戦時のデバッグがまだ)
        animator.SetBool("Attack", false);
        Debug.Log("攻撃判定ON");

        if (isCPU) return;

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
<<<<<<< HEAD
        foreach (var wpcol in weaponColliders) {
            wpcol.enabled = false;
        }
=======
        weaponCollider.enabled = false;
>>>>>>> parent of 5201b18 (刀のコライダーを正確にした。キャラクターのコライダーを小さくした。これによりファイナル判定になりづらくなった。試合開始時光が消えたとき、相手の色が一瞬で消えるのではなく1秒かけてJOJOに消えるように変更した。ネット対戦時のデバッグがまだ)
        Debug.Log("攻撃判定OFF");
        animator.SetBool("Attack", false);
        isAttacking = false;
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
<<<<<<< HEAD

        if (_Shinkuuha && GetComponent<FPSMove>().isShinkuuha) {
=======
        if (Shinkuuha && GetComponent<FPSMove>().isShinkuuha) {
>>>>>>> parent of 5201b18 (刀のコライダーを正確にした。キャラクターのコライダーを小さくした。これによりファイナル判定になりづらくなった。試合開始時光が消えたとき、相手の色が一瞬で消えるのではなく1秒かけてJOJOに消えるように変更した。ネット対戦時のデバッグがまだ)
            GetComponent<FPSMove>().isShinkuuha = false;
        }
    }

    #region Cheats

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

    #endregion

}