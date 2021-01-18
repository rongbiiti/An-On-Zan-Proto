using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class SetComponentForReplay : MonoBehaviourPunCallbacks
{
    private ReplayManager replayManager;

    private void OnEnable()
    {
        // ステージ中に配置されたReplayManagerを見つけて変数に自分のコンポーネントなどを入れてセットアップする
        if (photonView.IsMine || SceneManager.GetActiveScene().buildIndex == 3)
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
