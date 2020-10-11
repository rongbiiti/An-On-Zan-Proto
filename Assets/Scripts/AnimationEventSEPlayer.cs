using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Audio;

public class AnimationEventSEPlayer : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private AudioClip audioClip;

    [SerializeField]
    private AudioClip slashClip;

    [SerializeField]
    private AudioMixerGroup audioMixerGroup;

    [SerializeField]
    private float walkSpeed = 0.2f;

    [SerializeField]
    private float runSpeed = 0.8f;

    [SerializeField]
    private float sprintSpeed = 2f;

    [SerializeField]
    private AudioSource audioSource;

    private Animator animator;
    private float speed;
    public List<AudioClip> footStepSounds = null;


    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    
    public void PlayFootStep()
    {
        photonView.RPC("FootStep", RpcTarget.All);
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
        audioSource.PlayOneShot(slashClip);
    }

    private AudioSource CreateAudioSource()
    {
        var audioGameObject = new GameObject();
        audioGameObject.name = "AnimationEventSEPlayer";
        audioGameObject.transform.SetParent(gameObject.transform);

        var audioSource = audioGameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.outputAudioMixerGroup = audioMixerGroup;

        return audioSource;
    }
}