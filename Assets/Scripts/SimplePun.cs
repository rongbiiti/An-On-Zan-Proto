using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class SimplePun : MonoBehaviourPunCallbacks
{

    //[SerializeField] private byte _maxPlayer = 2;
    [SerializeField] private Vector3[] _startPosition;
    private bool playerCreatedFlg = false;
    GameObject myplayer;

    //RoomOptions roomOptions = new RoomOptions
    //{
    //    MaxPlayers = 2
    //};

    //public static SimplePun Instance;

    //void Awake()
    //{
    //    if (Instance) {
    //        Destroy(gameObject);
    //        return;
    //    }
    //    DontDestroyOnLoad(gameObject);
    //    Instance = this;
    //}

    void Start()
    {
        ////旧バージョンでは引数必須でしたが、PUN2では不要です。
        //PhotonNetwork.ConnectUsingSettings();
        //Debug.Log("ConnectUsingSettings");
        // シーンの読み込みコールバックを登録.
        SceneManager.sceneLoaded += OnLoadedScene;
        //キャラクターを生成
        myplayer = PhotonNetwork.Instantiate("Player_1_Net_Test", _startPosition[PhotonNetwork.LocalPlayer.ActorNumber - 1], Quaternion.identity, 0);
        //自分だけが操作できるようにスクリプトを有効にする

        PhotonView photonview = myplayer.GetComponent<PhotonView>();
        if (photonview.IsMine && PhotonNetwork.IsConnected == true) {
            MoveBehaviour playerScript = myplayer.GetComponent<MoveBehaviour>();
            playerScript.enabled = true;

            myplayer.transform.GetChild(0).gameObject.SetActive(true);

            ThirdPersonOrbitCamBasic camera = myplayer.transform.GetChild(0).GetComponent<ThirdPersonOrbitCamBasic>();
            camera.enabled = true;

            myplayer.GetComponent<MaterialChanger>().MaterialOn();
        }

        Debug.Log("キャラ作成成功" + PhotonNetwork.CurrentRoom.PlayerCount);

        GameManager_Net gameManager_Net = GameObject.Find("GameManager_Net").GetComponent<GameManager_Net>();
        gameManager_Net._player = myplayer;
        gameManager_Net._camera = myplayer.transform.GetChild(0).gameObject;
        gameManager_Net.MatchStart();

        playerCreatedFlg = true;
    }

    void OnGUI()
    {
        //ログインの状態を画面上に出力
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }

    ////ルームに入室前に呼び出される
    //public override void OnConnectedToMaster()
    //{
    //    // 適当なルームに参加する
    //    PhotonNetwork.JoinRandomRoom();
    //    Debug.Log("OnConnectedToMaster");
    //}

    ////ルームに入室後に呼び出される
    //public override void OnJoinedRoom()
    //{
    //    Room myroom = PhotonNetwork.CurrentRoom;　//myroom変数にPhotonnetworkの部屋の現在状況を入れる。
    //    Photon.Realtime.Player player = PhotonNetwork.LocalPlayer;　//playerをphotonnetworkのローカルプレイヤーとする
    //    Debug.Log("ルーム名:" + myroom.Name);
    //    Debug.Log("PlayerNo:" + player.ActorNumber);
    //    Debug.Log("プレイヤーID:" + player.UserId);

    //    //この部分はニックネームを決めるためのもので、入力は不要です。
    //    if (player.ActorNumber == 1) {
    //        //player.NickName = "わたしは1です";
    //    }

    //    Debug.Log("プレイヤー名:" + player.NickName);
    //    Debug.Log("ルームマスター:" + player.IsMasterClient); //ルームマスターならTrur。最初に部屋を作成した場合は、基本的にルームマスターなはず。
    //}

    //// ルーム入室失敗時に呼び出される。
    //public override void OnJoinRandomFailed(short returnCode, string message)
    //{
    //    // ルーム入室に失敗したら、ルームを作成する。
    //    Debug.Log("入室失敗");
    //    //ルームを作成する。
    //    roomOptions.MaxPlayers = _maxPlayer;
    //    PhotonNetwork.CreateRoom(null, roomOptions); //JoinOrCreateroomと同じ引数が使用可能。nullはルーム名を作成したくない場合roomNameを勝手に割り当てる。
    //}

    //private void Update()
    //{
    //    if (PhotonNetwork.InRoom && !playerCreatedFlg) {
    //        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers) {
    //            //キャラクターを生成
    //            myplayer = PhotonNetwork.Instantiate("Player_1_Net_Test", _startPosition[PhotonNetwork.LocalPlayer.ActorNumber - 1], Quaternion.identity, 0);
    //            //自分だけが操作できるようにスクリプトを有効にする

    //            PhotonView photonview = myplayer.GetComponent<PhotonView>();
    //            if (photonview.IsMine && PhotonNetwork.IsConnected == true) {
    //                MoveBehaviour playerScript = myplayer.GetComponent<MoveBehaviour>();
    //                playerScript.enabled = true;

    //                myplayer.transform.GetChild(0).gameObject.SetActive(true);

    //                ThirdPersonOrbitCamBasic camera = myplayer.transform.GetChild(0).GetComponent<ThirdPersonOrbitCamBasic>();
    //                camera.enabled = true;

    //                myplayer.GetComponent<MaterialChanger>().MaterialOn();
    //            }

    //            Debug.Log("キャラ作成成功" + PhotonNetwork.CurrentRoom.PlayerCount);

    //            GameManager_Net gameManager_Net = GameObject.Find("GameManager_Net").GetComponent<GameManager_Net>();
    //            gameManager_Net._player = myplayer;
    //            gameManager_Net._camera = myplayer.transform.GetChild(0).gameObject;
    //            gameManager_Net.MatchStart();

    //            playerCreatedFlg = true;
    //        }
    //    }
    //}

    private void OnLoadedScene(Scene i_scene, LoadSceneMode i_mode)
    {
        playerCreatedFlg = false;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        //PhotonNetwork.JoinRandomRoom();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        SceneManager.LoadScene(0);
    }

}