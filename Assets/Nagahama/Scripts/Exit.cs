using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Exit : MonoBehaviourPunCallbacks
{
    public void GameExit()
    {
        if(PhotonNetwork.IsConnected) {
            PhotonNetwork.Disconnect();
            StartCoroutine(nameof(Quit));
        } else {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
#endif
        }

    }

    private IEnumerator Quit()
    {
        while (PhotonNetwork.NetworkClientState != ClientState.Disconnected) {
            yield return new WaitForFixedUpdate();
        }

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
        #endif
    }
}
