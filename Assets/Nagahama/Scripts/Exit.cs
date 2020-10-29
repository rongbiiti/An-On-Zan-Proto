using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Exit : MonoBehaviour
{
    public void GameExit()
    {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
                      UnityEngine.Application.Quit();
#endif
    }
}
