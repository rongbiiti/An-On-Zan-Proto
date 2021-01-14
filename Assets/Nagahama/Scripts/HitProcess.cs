using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.AI;
using Photon.Pun;

public class HitProcess : MonoBehaviourPunCallbacks
{

    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioMixerGroup audioMixerGroup;
    [SerializeField] private GameObject blood;

    [HideInInspector] public GameObject _camera;
    [HideInInspector] public GameObject _parent;
    [HideInInspector] public MaterialChanger _parentMaterialChanger;

    private AudioSource audioSource;
    private Light directionalLight;
    private KillBGM killBGMComponent;
    private bool isHit;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
        killBGMComponent = GameObject.Find("Directional Light").GetComponent<KillBGM>();
    }

    public void PlayHit()
    {
        audioSource.PlayOneShot(hitClip);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isHit) {
            Debug.Log("プレイヤーにヒット");
            isHit = true;
            if (PhotonNetwork.InRoom) {
                int viewID = other.GetComponent<PhotonView>().ViewID;
                photonView.RPC("PlayerDeath", RpcTarget.AllViaServer, viewID);
                RoomManager.Instance.isCreatead = false;
            } else {
                PlayerDeath(other, false);
                transform.root.GetComponent<EnemyMove>().enabled = false;
            }

            directionalLight.intensity = 1;
            killBGMComponent.PlayKillBGM();
            transform.root.gameObject.tag = "Untagged";
            transform.root.gameObject.layer = LayerMask.NameToLayer("DeadBoddy");

        } else if (other.CompareTag("Enemy") && !isHit) {

            Debug.Log("CPUにヒット");
            isHit = true;
            PlayerDeath(other, true);
            PlayHit();
            directionalLight.intensity = 1;
            killBGMComponent.PlayKillBGM();
            transform.root.gameObject.tag = "Untagged";
            transform.root.gameObject.layer = LayerMask.NameToLayer("DeadBoddy");

        }
    }

    // オンライン用
    [PunRPC]
    private void PlayerDeath(int viewID)
    {
        PhotonView.Find(viewID).GetComponent<PlayerDeathProcess>().KillPlayer_Net();
        PhotonView.Find(viewID).GetComponent<MaterialChanger>().MaterialOn();
        PhotonView.Find(viewID).GetComponent<Animator>().SetBool("Death", true);
        PhotonView.Find(viewID).GetComponent<AttackProcess>().AttackEnd();

        PlayHit();
        StartCoroutine("DeathVoice");

        MaterialChanger[] materialChangers = FindObjectsOfType<MaterialChanger>();
        foreach(var mc in materialChangers)
        {
            mc.MaterialOn();
        }

        if(!gameObject.CompareTag("Sword")) {
            Debug.Log("真空波破棄");
            Destroy(gameObject);
        }
        
    }

    // オフライン用
    private void PlayerDeath(Collider col, bool isTargetCPU)
    {
        col.GetComponent<Animator>().SetBool("Death", true);
        if (!isTargetCPU) {
            col.GetComponent<PlayerDeathProcess>().KillPlayer_Net();
        } else {
            col.GetComponent<PlayerDeathProcess>().KillPlayer();            
        }

        col.GetComponent<MaterialChanger>().MaterialOn();
        col.GetComponent<AttackProcess>().AttackEnd();

        PlayHit();
        StartCoroutine("DeathVoice");

        // 自分の姿を明かす
        _parentMaterialChanger.MaterialOn();
    }

    private IEnumerator DeathVoice()
    {
        directionalLight.intensity = 1;
        killBGMComponent.PlayKillBGM();
        if (_camera != null && _camera.activeSelf) {
            _camera.GetComponent<ExecutionCamera>().StartCoroutine("Execution");
        }

        yield return new WaitForSeconds(0.17f);
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("真空波がなにかにヒットした");

        if (other.CompareTag("Player")) {
            Debug.Log("真空波がプレイヤーにヒット");
            directionalLight.intensity = 1;
            killBGMComponent.PlayKillBGM();

            if (PhotonNetwork.InRoom) {

                int viewID = other.GetComponent<PhotonView>().ViewID;
                photonView.RPC("PlayerDeath", RpcTarget.All, viewID);
                RoomManager.Instance.isCreatead = false;

            } else {
                PlayerDeath(other.GetComponent<Collider>(), false);
                _parent.GetComponent<EnemyMove>().enabled = false;
            }

        } else if (other.CompareTag("Enemy")) {
            Debug.Log("真空波がCPUにヒット");
            PlayerDeath(other.GetComponent<Collider>(), true);
            PlayHit();
            directionalLight.intensity = 1;
            killBGMComponent.PlayKillBGM();
        }
    }
}
