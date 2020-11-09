using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class DebugGUI : MonoBehaviour
{
    public bool _debug;

    private void OnGUI()
    {
        if (!_debug) return;

        if (GUI.Button(new Rect(10, 10, 100, 50), "部屋退出"))
        {
            //命令//
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
                Debug.Log("InRoomが呼ばれた");
            }
            else if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveLobby();
                Debug.Log("Inlobbyが呼ばれた");
            }
            else
            {
                Debug.Log("elseが呼ばれた");
                SceneReload();
            }
        }

        if (GUI.Button(new Rect(10, 70, 100, 50), "太陽光ON"))
        {
            Light directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();

            //命令//
            if (directionalLight.intensity <= 0.1f)
            {
                directionalLight.intensity = 1f;
            }
            else if (0.1f <= directionalLight.intensity)
            {
                directionalLight.intensity = 0f;
            }
        }

        if (GUI.Button(new Rect(10, 130, 100, 50), "ゲーム中断"))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            if (PhotonNetwork.IsConnected)
            {
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

    private void SceneReload()
    {
        SceneManager.LoadScene(0);
    }
}
