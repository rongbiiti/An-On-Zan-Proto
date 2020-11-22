using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ForReMatch : MonoBehaviour
{
    private void Start()
    {
        RoomManager.Instance.isCreatead = false;
        if (!PhotonNetwork.IsMasterClient)//マスタークライアントでなければ下記の処理は行えない。
            return;
        StartCoroutine(nameof(LoadScene));
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(2f);
        RoomManager.Instance.isCreatead = false;
        PhotonNetwork.LoadLevel(2);
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
    }
}
