using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class Restart : MonoBehaviour
{
    public void SceneReload()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        if (PhotonNetwork.InRoom) {
            PhotonNetwork.LeaveRoom();
            Debug.Log("InRoomが呼ばれた");
        } else if (PhotonNetwork.InLobby) {
            PhotonNetwork.LeaveLobby();
            Debug.Log("Inlobbyが呼ばれた");
        } else{
            Debug.Log("elseが呼ばれた");
            SceneReload();
        }
        
    }
}
