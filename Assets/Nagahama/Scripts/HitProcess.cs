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

    public GameObject _camera;
    public GameObject _parent;
    public MaterialChanger _parentMaterialChanger;

    private AudioSource audioSource;
    private bool hitFlg = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayHit()
    {
        audioSource.PlayOneShot(hitClip);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            Debug.Log("プレイヤーにヒット");

            if (PhotonNetwork.InRoom) {
                int viewID = other.GetComponent<PhotonView>().ViewID;
                photonView.RPC("PlayerDeath", RpcTarget.All, viewID);
            } else {
                PlayerDeath(other, false);
                transform.root.GetComponent<EnemyMove>().enabled = false;
            }

            GameObject.Find("Directional Light").GetComponent<Light>().intensity = 1;

        } else if (other.CompareTag("Enemy")) {
            Debug.Log("CPUにヒット");
            PlayerDeath(other, true);
            PlayHit();
            GameObject.Find("Directional Light").GetComponent<Light>().intensity = 1;
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

        // 自分の姿を明かす
        _parentMaterialChanger.MaterialOn();
    }

    // オフライン用
    private void PlayerDeath(Collider col, bool isTargetCPU)
    {
        
        if (!isTargetCPU) {
            col.GetComponent<PlayerDeathProcess>().KillPlayer_Net();
        } else {
            col.GetComponent<PlayerDeathProcess>().KillPlayer();            
        }

        col.GetComponent<MaterialChanger>().MaterialOn();
        col.GetComponent<AttackProcess>().AttackEnd();
        col.GetComponent<Animator>().SetBool("Death", true);

        PlayHit();
        StartCoroutine("DeathVoice");

        // 自分の姿を明かす
        _parentMaterialChanger.MaterialOn();
    }

    private IEnumerator DeathVoice()
    {
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
            GameObject.Find("Directional Light").GetComponent<Light>().intensity = 1;

            if (PhotonNetwork.InRoom) {

                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                foreach (var p in players) {
                    PhotonView photonView = p.GetPhotonView();
                    if (!photonView.IsMine) {
                        p.GetComponent<MaterialChanger>().MaterialOn();
                    }
                }

                int viewID = other.GetComponent<PhotonView>().ViewID;
                photonView.RPC("PlayerDeath", RpcTarget.All, viewID);

            } else {
                PlayerDeath(other.GetComponent<Collider>(), false);
                _parent.GetComponent<EnemyMove>().enabled = false;
            }

        } else if (other.CompareTag("Enemy")) {
            Debug.Log("真空波がCPUにヒット");
            PlayerDeath(other.GetComponent<Collider>(), true);
            PlayHit();
            GameObject.Find("Directional Light").GetComponent<Light>().intensity = 1;
        }
    }
}
