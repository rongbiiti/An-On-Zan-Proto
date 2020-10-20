using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class SimplePun : MonoBehaviourPunCallbacks
{

    [SerializeField] Vector3[] _startPosition;
    [SerializeField] string _playerPrefabName;
    [SerializeField] GameObject _panel;
    [SerializeField] GameObject _errorPanel;
    [SerializeField] TMP_Text _text;

    bool playerCreatedFlg = false;
    GameObject myplayer;
    float timeOutWait = 10f;
    bool timeOutWaitFlg;

    void Start()
    {
        Debug.Log(PhotonNetwork.NetworkingClient.UserId);
        // シーンの読み込みコールバックを登録.
        //SceneManager.sceneUnloaded += OnLoadedScene;
        //キャラクターを生成
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var p in players) {
            PhotonView photonView = p.GetPhotonView();
            Debug.Log(PhotonNetwork.NetworkingClient.UserId);
            if (photonView.Controller.UserId == PhotonNetwork.NetworkingClient.UserId) {
                Debug.Log("自分が操作していたキャラを見つけた");
                myplayer = p;
                FirstPersonAIO firstPersonAIO = myplayer.GetComponent<FirstPersonAIO>();
                firstPersonAIO.enabled = true;

                FPSMove fpsMove = myplayer.GetComponent<FPSMove>();
                fpsMove.enabled = true;

                myplayer.GetComponent<MaterialChanger>().enabled = false;
                GameManager_Net gameManager_Net = GameObject.Find("GameManager_Net").GetComponent<GameManager_Net>();
                gameManager_Net._player = p;
                gameManager_Net.PlayerActive();
                playerCreatedFlg = true;
                return;
            }
        }
        Debug.Log(players.Length);
        if (players.Length >= 2) {
            playerCreatedFlg = true;
        }
        if (playerCreatedFlg == false) {
            myplayer = PhotonNetwork.Instantiate(_playerPrefabName, _startPosition[PhotonNetwork.LocalPlayer.ActorNumber - 1], Quaternion.identity, 0);
        }

        //自分だけが操作できるようにスクリプトを有効にする
        PhotonView photonview = myplayer.GetComponent<PhotonView>();
        if (photonview.IsMine && PhotonNetwork.IsConnected == true) {

            FirstPersonAIO firstPersonAIO = myplayer.GetComponent<FirstPersonAIO>();
            firstPersonAIO.enabled = true;

            FPSMove fpsMove = myplayer.GetComponent<FPSMove>();
            fpsMove.enabled = true;

            myplayer.GetComponent<MaterialChanger>().enabled = false;
            Debug.Log("キャラ作成成功" + PhotonNetwork.LocalPlayer.ActorNumber);

        }

        if (playerCreatedFlg == false) {
            GameManager_Net gameManager_Net = GameObject.Find("GameManager_Net").GetComponent<GameManager_Net>();
            gameManager_Net._player = myplayer;
            gameManager_Net.MatchStart();
        }
        playerCreatedFlg = true;
    }

    private void FixedUpdate()
    {
        if (timeOutWaitFlg) {
            timeOutWait -= Time.deltaTime;
            _text.text = timeOutWait.ToString("f1");
            if (timeOutWait < 0f) {
                StartCoroutine("ErrorLeaveRoom");
            }
        }
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
        PhotonNetwork.CurrentRoom.IsOpen = true;
        timeOutWaitFlg = true;
        _panel.SetActive(true);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        timeOutWait = 10f;
        timeOutWaitFlg = false;
        _panel.SetActive(false);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        if (!PhotonNetwork.ReconnectAndRejoin()) {
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("再入室？");
        
    }

    public override void OnConnected()
    {
        base.OnConnected();
    }

    IEnumerator ErrorLeaveRoom()
    {
        _panel.SetActive(false);
        _errorPanel.SetActive(true);
        timeOutWaitFlg = false;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        yield return new WaitForSeconds(3f);
        PhotonNetwork.LeaveRoom();
    }
}