using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.AI;
using Photon.Pun;

public class HitProcess : MonoBehaviourPunCallbacks
{

    [SerializeField] private AudioClip deathVoiceClip;

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

    public void PlaydeathVoice()
    {
        audioSource.PlayOneShot(deathVoiceClip);
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
        PhotonView.Find(viewID).GetComponent<RagdollController>().RagdollActive_Net();
        PhotonView.Find(viewID).GetComponent<MaterialChanger>().MaterialOn();
        PhotonView.Find(viewID).GetComponent<Animator>().SetBool("Death", true);
        PhotonView.Find(viewID).GetComponent<AttackProcess>().AttackEnd();

        PlayHit();
        StartCoroutine("DeathVoice");
    }

    // オフライン用
    private void PlayerDeath(Collider col, bool isTargetCPU)
    {
        // 自分の姿を明かす
        _parentMaterialChanger.MaterialOn();

        if (!isTargetCPU) {
            col.GetComponent<RagdollController>().RagdollActive_Net();
        } else {
            col.GetComponent<RagdollController>().RagdollActive();
        }

        col.GetComponent<MaterialChanger>().MaterialOn();
        col.GetComponent<AttackProcess>().AttackEnd();
        col.GetComponent<Animator>().SetBool("Death", true);

        PlayHit();
        StartCoroutine("DeathVoice");
    }

    private IEnumerator DeathVoice()
    {
        if (_camera != null && _camera.activeSelf) {
            _camera.GetComponent<ExecutionCamera>().StartCoroutine("Execution");
        }

        yield return new WaitForSeconds(0.17f);
        PlaydeathVoice();
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("真空波がなにかにヒットした");
        if (other.CompareTag("Player")) {
            Debug.Log("真空波がプレイヤーにヒット");
            if (other == _parent) return;
            if (PhotonNetwork.InRoom) {
                int viewID = other.GetComponent<PhotonView>().ViewID;
                photonView.RPC("PlayerDeath", RpcTarget.All, viewID);
            } else {
                PlayerDeath(other.GetComponent<Collider>(), false);
            }

            GameObject.Find("Directional Light").GetComponent<Light>().intensity = 1;

        } else if (other.CompareTag("Enemy")) {
            Debug.Log("真空波がCPUにヒット");
            PlayerDeath(other.GetComponent<Collider>(), true);
            PlayHit();
            GameObject.Find("Directional Light").GetComponent<Light>().intensity = 1;
        }
    }
}
