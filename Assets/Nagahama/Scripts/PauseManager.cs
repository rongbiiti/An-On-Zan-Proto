using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private AudioListener audioListener;
    private bool isPause = false;
    public bool isCanPause;
    public bool isCPUMatch = true;

    void Update()
    {
        if(Input.GetButtonDown("Pause") && !isPause && isCanPause) {
            _pausePanel.SetActive(true);
            
            if (isCPUMatch)
            {
                Pauser.Pause();
                audioListener.enabled = true;
            }
            isPause = true;
        }
    }

    public void UnPause()
    {
        _pausePanel.SetActive(false);
        
        if (isCPUMatch)
        {
            Pauser.Resume();
            audioListener.enabled = false;
        }
        isPause = false;
    }
}
