using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBGM : MonoBehaviour
{
    [SerializeField] private AudioClip _killBGMClip;
    private AudioSource audioSource;
    private bool playFlg;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayKillBGM()
    {
        if (!playFlg) {
            playFlg = true;
            audioSource.PlayOneShot(_killBGMClip);
        }
    }
}
