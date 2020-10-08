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
            Debug.Log("プレイヤーにヒット1");
            int viewID = other.GetComponent<PhotonView>().ViewID;
            photonView.RPC("PlayerDeath", RpcTarget.All, viewID);

        } else if (other.CompareTag("Enemy")) {
            PlaydeathVoice();
            PlayHit();
            other.GetComponent<RagdollController>().RagdollActive(transform.root.forward);

        } else if (other.gameObject.layer == LayerMask.NameToLayer("EnemyRagdoll") && !hitFlg) {
            Vector3 vec = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
            var b = Instantiate(blood, vec, Quaternion.identity) as GameObject;
            b.transform.SetParent(other.transform);
            b.transform.LookAt(transform.root.position);

            hitFlg = false;
        }
    }

    [PunRPC]
    private void PlayerDeath(int viewID)
    {
        Vector3 vec = new Vector3(PhotonView.Find(viewID).transform.position.x, PhotonView.Find(viewID).transform.position.y + 1.5f, PhotonView.Find(viewID).transform.position.z);
        var b = Instantiate(blood, vec, Quaternion.identity) as GameObject;
        b.transform.SetParent(PhotonView.Find(viewID).transform.GetChild(2).GetChild(0).GetChild(2));
        b.transform.LookAt(transform.root.position);

        PhotonView.Find(viewID).GetComponent<RagdollController>().RagdollActive_Net(transform.root.forward);
        PhotonView.Find(viewID).GetComponent<MaterialChanger>().MaterialOn();
        PlayHit();
        StartCoroutine("DeathVoice");
    }

    private IEnumerator DeathVoice()
    {
        if (transform.root.GetChild(0).gameObject.activeSelf) {
            transform.root.GetChild(0).GetComponent<ExecutionCamera>().StartCoroutine("Execution");
        }
       
        yield return new WaitForSeconds(0.2f);
        PlaydeathVoice();
    }
}
