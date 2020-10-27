using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.AI;
using Photon.Pun;

public class HitProcess : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private AudioClip deathVoiceClip;

    [SerializeField]
    private AudioClip hitClip;

    [SerializeField]
    private AudioMixerGroup audioMixerGroup;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private GameObject blood;

    [SerializeField] GameObject _camera;

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
        Vector3 vec = new Vector3(PhotonView.Find(viewID).transform.position.x + 0.25f, PhotonView.Find(viewID).transform.position.y + 0.35f, PhotonView.Find(viewID).transform.position.z + 0.8f);
        GameObject effect = Instantiate(blood, vec, Quaternion.identity) as GameObject;
        effect.transform.SetParent(PhotonView.Find(viewID).transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2));
        effect.transform.LookAt(transform.root.position);

        PhotonView.Find(viewID).GetComponent<RagdollController>().RagdollActive_Net(transform.root.forward);
        PhotonView.Find(viewID).GetComponent<MaterialChanger>().MaterialOn();
        PhotonView.Find(viewID).GetComponent<Animator>().SetBool("Death", true);
        PhotonView.Find(viewID).GetComponent<AttackProcess>().AttackEnd();
        PlayHit();
        StartCoroutine("DeathVoice");
    }

    // オフライン用
    private void PlayerDeath(Collider col, bool isTargetCPU)
    {
        GameObject effect;
        if (!isTargetCPU) {
            Vector3 vec = new Vector3(col.transform.position.x + 0.25f, col.transform.position.y + 0.35f, col.transform.position.z + 0.8f);
            effect = Instantiate(blood, vec, Quaternion.identity) as GameObject;
            effect.transform.SetParent(col.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2));
            col.GetComponent<RagdollController>().RagdollActive_Net(transform.root.forward);
        } else {
            Vector3 vec = new Vector3(col.transform.position.x, col.transform.position.y + 1.35f, col.transform.position.z);
            effect = Instantiate(blood, vec, Quaternion.identity) as GameObject;
            effect.transform.SetParent(col.transform.GetChild(1).GetChild(0).GetChild(2));
            col.GetComponent<RagdollController>().RagdollActive(transform.root.forward);
        }

        effect.transform.LookAt(transform.root.position);

        col.GetComponent<MaterialChanger>().MaterialOn();
        transform.root.GetComponent<MaterialChanger>().MaterialOn();
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
}
