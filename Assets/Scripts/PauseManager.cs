using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    private bool isPause = false;
    public bool isCanPause;

    void Update()
    {
        if(Input.GetButtonDown("Pause") && !isPause && isCanPause) {
            _pausePanel.SetActive(true);
            Pauser.Pause();
            isPause = true;
        }
    }

    public void UnPause()
    {
        _pausePanel.SetActive(false);
        Pauser.Resume();
        isPause = false;
    }
}
