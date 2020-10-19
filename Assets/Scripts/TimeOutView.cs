using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class TimeOutView : MonoBehaviourPunCallbacks
{
    [SerializeField] float _timeOutSecond = 10f;
    TMP_Text _text;
    float lag;

    void Start()
    {
        if (!photonView.IsMine) {
            _text = GameObject.Find("TimeOutCountDownText").GetComponent<TMP_Text>();
        }
    }

    public void FixedUpdate()
    {
        if (!photonView.IsMine) {
            _text.text = lag.ToString();
        }
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
    //}
}
