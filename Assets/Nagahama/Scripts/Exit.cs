using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Exit : MonoBehaviourPunCallbacks
{
    public void GameExit()
    {
        if(PhotonNetwork.IsConnected || PhotonNetwork.NetworkClientState == ClientState.Joined) {
            Debug.Log("trueの方");
            PhotonNetwork.Disconnect();
            StartCoroutine(nameof(Quit));
            
        } else {
            Debug.Log("終了します");
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
            Debug.Log("処理中");
        }

        Debug.Log("終了します");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
        #endif
    }
}
