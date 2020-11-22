using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
// 1.UIシステムを使うときに必要なライブラリ
using UnityEngine.UI;
// 2.Scene関係の処理を行うときに必要なライブラリ
using UnityEngine.SceneManagement;

public class Online_Restart : MonoBehaviourPunCallbacks
{
    public void SceneReload()
    {
        if (!PhotonNetwork.IsMasterClient)//マスタークライアントでなければ下記の処理は行えない。
            return;
        RoomManager.Instance.isCreatead = false;
        PhotonNetwork.LoadLevel(4);
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

}