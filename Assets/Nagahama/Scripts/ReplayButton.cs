using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplayButton : MonoBehaviour
{
    [SerializeField] private ReplayManager[] _replayManagers;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    private void FixedUpdate()
    {
        if (_replayManagers[0].isRunning) {
            button.interactable = false;
        } else {
            button.interactable = true;
        }
    }

    public void ReplayStart()
    {
        foreach(var rm in _replayManagers) {
            rm.ReplayStart();
        }
    }
}
