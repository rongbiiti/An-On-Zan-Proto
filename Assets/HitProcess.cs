using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class HitProcess : MonoBehaviour
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
            PlaydeathVoice();
            PlayHit();
            //other.gameObject.SetActive(false);
            Vector3 vec = new Vector3(other.transform.position.x, other.transform.position.y + 1f, other.transform.position.z);
            var b = Instantiate(blood, vec, Quaternion.identity) as GameObject;
            b.transform.SetParent(other.transform);
        } else if (other.CompareTag("Enemy")) {
            PlaydeathVoice();
            PlayHit();
            //other.gameObject.SetActive(false);
            Vector3 vec = new Vector3(other.transform.position.x, other.transform.position.y + 1f, other.transform.position.z);
            var b = Instantiate(blood, vec, Quaternion.identity) as GameObject;
            b.transform.SetParent(other.transform);
        }
    }
}
