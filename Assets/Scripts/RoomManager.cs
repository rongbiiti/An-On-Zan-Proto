using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;
    public bool isCreatead;

    void Awake()
    {
        if (Instance) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
        Debug.Log("RoomManagerCreated");
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 1 && PhotonNetwork.InRoom) // We're in the game scene
        {
            if (!isCreatead) {
                GameObject.Find("NetworkStarter").GetComponent<SimplePun>().CreatePlayer();
                Debug.Log("プレイヤー作成");
            } else {
                GameObject.Find("NetworkStarter").GetComponent<SimplePun>().StartCoroutine("SearchPlayer");
                Debug.Log("プレイヤー検索");
            }
            
        }
    }
}