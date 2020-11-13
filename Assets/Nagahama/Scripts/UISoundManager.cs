using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    protected static UISoundManager instance;

    public static UISoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (UISoundManager)FindObjectOfType(typeof(UISoundManager));

                if (instance == null)
                {
                    Debug.LogError("UISoundManager Instance Error");
                }
            }

            return instance;
        }
    }

    [SerializeField] private AudioClip _se_Decide;
    [SerializeField] private AudioClip _se_Cancel;
    [SerializeField] private AudioClip _se_Select;

    private AudioSource audioSource;

    void Awake()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("UISoundManager");
        if (obj.Length > 1)
        {
            // 既に存在しているなら削除
            Destroy(gameObject);
        }
        else
        {
            // 音管理はシーン遷移では破棄させない
            DontDestroyOnLoad(gameObject);
        }

        

    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayDecideSE()
    {
        audioSource.PlayOneShot(_se_Decide);
    }

    public void PlayCancelSE()
    {
        audioSource.PlayOneShot(_se_Cancel);
    }

    public void PlaySelectSE()
    {
        audioSource.PlayOneShot(_se_Select);
    }
}