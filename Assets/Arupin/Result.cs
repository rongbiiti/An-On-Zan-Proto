using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    public GameObject result;
    public PauseManager _pause;
    [HideInInspector] public FirstPersonAIO _fpsAIO;
    [HideInInspector] public FPSMove _fpsMove;

    public void ResultEneble()
    {
        result.SetActive(true);
        _pause.isCanPause = false;
        _fpsAIO.enabled = false;
        _fpsMove.enabled = false;
    }
}
