using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
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
        }
        isPause = false;
    }
}
