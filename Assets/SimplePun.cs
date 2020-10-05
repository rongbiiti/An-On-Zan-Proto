using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;

public class SimplePun : MonoBehaviourPunCallbacks
{

    [SerializeField] private byte maxPlayer = 2;

    RoomOptions roomOptions = new RoomOptions
    {
        MaxPlayers = 2
    };

    // Use this for initialization
    void Start()
    {
        //旧バージョンでは引数必須でしたが、PUN2では不要です。
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("start");
    }

    void OnGUI()
    {
        //ログインの状態を画面上に出力
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }

    //ルームに入室前に呼び出される
    public override void OnConnectedToMaster()
    {
        // "room"という名前のルームに参加する（ルームが無ければ作成してから参加する）
        roomOptions.MaxPlayers = maxPlayer;
        PhotonNetwork.JoinOrCreateRoom("An-On-Zan", roomOptions, TypedLobby.Default);
        Debug.Log("OnConnectedToMaster");
    }

    //ルームに入室後に呼び出される
    public override void OnJoinedRoom()
    {
        //キャラクターを生成
        GameObject myplayer = PhotonNetwork.Instantiate("Player_1_Net_Test", new Vector3(0, 5.507f, -5), Quaternion.identity, 0);
        //自分だけが操作できるようにスクリプトを有効にする
        
        PhotonView photonview = myplayer.GetComponent<PhotonView>();
        if (photonview.IsMine && PhotonNetwork.IsConnected == true) {
            MoveBehaviour playerScript = myplayer.GetComponent<MoveBehaviour>();
            playerScript.enabled = true;

            myplayer.transform.GetChild(0).gameObject.SetActive(true);

            ThirdPersonOrbitCamBasic camera = myplayer.transform.GetChild(0).GetComponent<ThirdPersonOrbitCamBasic>();
            camera.enabled = true;
        }

        Debug.Log("キャラ作成成功");

        Room myroom = PhotonNetwork.CurrentRoom;　//myroom変数にPhotonnetworkの部屋の現在状況を入れる。
        Photon.Realtime.Player player = PhotonNetwork.LocalPlayer;　//playerをphotonnetworkのローカルプレイヤーとする
        Debug.Log("ルーム名:" + myroom.Name);
        Debug.Log("PlayerNo" + player.ActorNumber);
        Debug.Log("プレイヤーID" + player.UserId);

        //この部分はニックネームを決めるためのもので、入力は不要です。
        if (player.ActorNumber == 1) {
            player.NickName = "わたしは1です";
        }

        Debug.Log("プレイヤー名" + player.NickName);
        Debug.Log("ルームマスター" + player.IsMasterClient); //ルームマスターならTrur。最初に部屋を作成した場合は、基本的にルームマスターなはず。
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {

        Debug.Log("入室失敗");
        //ルームを作成する。
        PhotonNetwork.CreateRoom(null, roomOptions); //JoinOrCreateroomと同じ引数が使用可能。nullはルーム名を作成したくない場合roomNameを勝手に割り当てる。
    }
}