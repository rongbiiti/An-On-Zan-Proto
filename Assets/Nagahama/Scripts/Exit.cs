using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Exit : MonoBehaviourPunCallbacks
{
    public void GameExit()
    {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        if (PhotonNetwork.IsConnected) {
            PhotonNetwork.Disconnect();
        }
#elif UNITY_STANDALONE
                      UnityEngine.Application.Quit();
        if (PhotonNetwork.IsConnected) {
            PhotonNetwork.Disconnect();
        }
#endif
    }
}
