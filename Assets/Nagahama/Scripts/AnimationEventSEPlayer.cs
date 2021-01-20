using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Audio;

public class AnimationEventSEPlayer : MonoBehaviourPunCallbacks
{
    [SerializeField] private AudioClip slashClip;
    [SerializeField] private float walkSpeed = 0.2f;
    [SerializeField] private float sprintSpeed = 2f;
    [SerializeField] private AudioSource audioSource;
    public List<AudioClip> footStepSounds = null;

    private float runSpeed = 5f;
    private Animator animator;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // 相手の近くにワープ
        /* if (Input.GetKey(KeyCode.Z) && Input.GetKeyDown(KeyCode.T) && gameObject.CompareTag("Player")) {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var p in players) {
                PhotonView photonView = p.GetPhotonView();
                Debug.Log(PhotonNetwork.NetworkingClient.UserId);
                if (!photonView.IsMine) {
                    Vector3 startPos = p.transform.position;
                    Vector3 targetPos = p.transform.forward;
                    transform.position = startPos + targetPos * 3f;
                    GameManager_Net.FindObjectOfType<GameManager_Net>().PlayerActive();
                    return;
                }
            }
        } */
    }

    public void PlayFootStep()
    {
        if (PhotonNetwork.InRoom) {
            photonView.RPC("FootStep", RpcTarget.AllViaServer);
        } else {
            FootStep();
        }
        
    }

    [PunRPC]
    public void FootStep()
    {
        int n = Random.Range(0, footStepSounds.Count);
        if (footStepSounds.Any() && footStepSounds[n] != null) {
            audioSource.PlayOneShot(footStepSounds[n]);
        }
            
    }

    public void PlayWalk(string eventName)
    {
        if (animator.GetFloat("Speed") < walkSpeed)
            audioSource.PlayOneShot(audioSource.clip);
    }

    public void PlayRun(string eventName)
    {
        if (animator.GetFloat("Speed") > runSpeed)
            audioSource.PlayOneShot(audioSource.clip);
    }

    public void PlaySprint(string eventName)
    {
        if (animator.GetFloat("Speed") > sprintSpeed)
            audioSource.PlayOneShot(audioSource.clip);
    }

    public void PlaySlash(string eventName)
    {
        Debug.Log("音再生");
        audioSource.PlayOneShot(slashClip);
    }    
}