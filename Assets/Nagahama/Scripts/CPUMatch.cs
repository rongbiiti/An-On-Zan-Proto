using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class CPUMatch : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene(2);
        PhotonNetwork.OfflineMode = true;
        Debug.Log("ok");
    }

    
}
