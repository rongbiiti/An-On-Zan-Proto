using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class SimplePun : MonoBehaviourPunCallbacks
{

    [SerializeField] Vector3[] _startPosition;
    [SerializeField] string _playerPrefabName;

    bool playerCreatedFlg = false;
    GameObject myplayer;

    void Start()
    {
        ////旧バージョンでは引数必須でしたが、PUN2では不要です。
        // シーンの読み込みコールバックを登録.
        //SceneManager.sceneUnloaded += OnLoadedScene;
        //キャラクターを生成
        myplayer = PhotonNetwork.Instantiate(_playerPrefabName, _startPosition[PhotonNetwork.LocalPlayer.ActorNumber - 1], Quaternion.identity, 0);
        //自分だけが操作できるようにスクリプトを有効にする

        PhotonView photonview = myplayer.GetComponent<PhotonView>();
        if (photonview.IsMine && PhotonNetwork.IsConnected == true) {

            FirstPersonAIO firstPersonAIO = myplayer.transform.GetComponent<FirstPersonAIO>();
            firstPersonAIO.enabled = true;

            FPSMove fpsMove = myplayer.GetComponent<FPSMove>();
            fpsMove.enabled = true;

            myplayer.GetComponent<MaterialChanger>().enabled = false;
            Debug.Log("キャラ作成成功" + PhotonNetwork.LocalPlayer.ActorNumber);

        }

        GameManager_Net gameManager_Net = GameObject.Find("GameManager_Net").GetComponent<GameManager_Net>();
        gameManager_Net._player = myplayer;
        gameManager_Net.MatchStart();

        playerCreatedFlg = true;
    }

    void OnGUI()
    {
        //ログインの状態を画面上に出力
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }

    private void OnLoadedScene(Scene i_scene)
    {
        playerCreatedFlg = false;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        SceneManager.LoadScene(0);
    }

}