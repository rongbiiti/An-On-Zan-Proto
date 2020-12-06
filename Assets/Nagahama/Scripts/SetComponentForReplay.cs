using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SetComponentForReplay : MonoBehaviourPunCallbacks
{
    private ReplayManager replayManager;
    private void OnEnable()
    {
        if (photonView.IsMine)
        {
            replayManager = GameObject.Find("ReplayManager").GetComponent<ReplayManager>();
            replayManager.enabled = true;
            replayManager.target_P = transform;
            replayManager.firstPerson = GetComponent<FirstPersonAIO>();
            replayManager.animator = GetComponent<Animator>();
            replayManager.playerCamera = transform.GetChild(0).GetChild(0).gameObject;
            replayManager._playerDeathProcess = GetComponent<PlayerDeathProcess>();
        }
        else
        {
            replayManager = GameObject.Find("ReplayManager_Oppnent").GetComponent<ReplayManager>();
            replayManager.enabled = true;
            replayManager.target_P = transform;
            replayManager.animator = GetComponent<Animator>();
            replayManager._playerDeathProcess = GetComponent<PlayerDeathProcess>();
        }
    }
}
