using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] TMP_InputField _roomNameInputField;
    [SerializeField] TMP_InputField _playerNameInputField;
    [SerializeField] TMP_Text _errorText;
    [SerializeField] TMP_Text _roomNameText;
    [SerializeField] Transform _roomListContent;
    [SerializeField] GameObject _roomListItemPrefab;
    [SerializeField] Transform _playerListContent;
    [SerializeField] GameObject _playerListItemPrefab;
    [SerializeField] GameObject _startGameButton;

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 10000;
        PhotonNetwork.KeepAliveInBackground = 10;
    }

    void Start()
    {
        //Connect();
    }

    void OnGUI()
    {
        //ログインの状態を画面上に出力
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }

    public void Connect()
    {
        Debug.Log("Connecting to Master");

        PhotonNetwork.ConnectUsingSettings();

        if (PhotonNetwork.InLobby) {
            MenuManager.Instance.OpenMenu("title");

        } else if (PhotonNetwork.IsConnected) {
            Debug.Log("Connected to Master");
            PhotonNetwork.JoinLobby();
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 10000;

            if (string.IsNullOrEmpty(_playerNameInputField.text)) {
                PhotonNetwork.NickName = "侍 " + Random.Range(0, 1000).ToString("0000");
                return;
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 10000;
        if (string.IsNullOrEmpty(_playerNameInputField.text)) {
            PhotonNetwork.NickName = "侍 " + Random.Range(0, 1000).ToString("0000");
            return;
        }
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined Lobby");        
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(_roomNameInputField.text)) {
            _roomNameInputField.text = "死合場 " + Random.Range(0, 1000).ToString("0000");
            return;
        }

        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.PlayerTtl = 10000;

        PhotonNetwork.CreateRoom(_roomNameInputField.text, roomOptions);

        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom()
    {        
        MenuManager.Instance.OpenMenu("room");
        _roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in _playerListContent) {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++) {
            Instantiate(_playerListItemPrefab, _playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        _startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        _startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        _errorText.text = "部屋の作成に失敗しました: " + message;
        Debug.LogError("部屋の作成に失敗しました: " + message);
        MenuManager.Instance.OpenMenu("error");
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    public void JoinRoom(RoomInfo info)
    {
        
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in _roomListContent) {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++) {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(_roomListItemPrefab, _roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers) {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }
        Instantiate(_playerListItemPrefab, _playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public void ChangePlayerNickName()
    {
        if (string.IsNullOrEmpty(_playerNameInputField.text)) {
            PhotonNetwork.NickName = "侍 " + Random.Range(0, 1000).ToString("0000");
            return;
        } else {
            PhotonNetwork.NickName = _playerNameInputField.text;
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        switch (cause) {
            case DisconnectCause.Exception:     // 何らかの内部例外によりソケットコードが失敗しました。これは、ローカルで接続しようとしてもサーバが利用できない場合に発生する可能性があります。疑問がある場合は、Exit Gamesに連絡してください。Exit Gamesに連絡してください。
            case DisconnectCause.ServerTimeout: // タイミングアウト（クライアントからの確認応答がない）のため、サーバーがこのクライアントを切断しました。
            case DisconnectCause.ClientTimeout: // このクライアントは、サーバの応答が期限内に受信されていないことを検出しました。
            case DisconnectCause.DisconnectByServerLogic:   // サーバがこのクライアントをルームのロジック内から切断しました(C#のコード)。
            case DisconnectCause.AuthenticationTicketExpired:   // 認証チケットは、別の認証サービスコールをしなくても、Photon Cloudサーバーへのアクセスを提供する必要があります。しかし、チケットの有効期限が切れています。
            case DisconnectCause.DisconnectByServerReasonUnknown:   // サーバは不明な理由でこのクライアントを切断しました。
                PhotonNetwork.ConnectUsingSettings();
                break;
            case DisconnectCause.OperationNotAllowedInCurrentState: // OnOperationResponseです。このクライアントでは (現在) 利用できない操作 (通常は許可されていません)。op Authenticate に対してのみ追跡されます。
            case DisconnectCause.CustomAuthenticationFailed:    // OnOperationResponseを使用しています。無効なクライアント値またはCloud Dashboardでカスタム認証を設定してPhoton Cloudで認証します。
            case DisconnectCause.DisconnectByClientLogic:   // サーバがこのクライアントをルームのロジック内から切断しました(C#のコード)。
            case DisconnectCause.InvalidAuthentication: // OnOperationResponse. 無効な AppId で Photon Cloud で認証しています。サブスクリプションを更新するか、Exit Gamesに連絡してください。
            case DisconnectCause.ExceptionOnConnect:    // OnStatusChanged: サーバが利用できないか、アドレスが間違っています。ポートが提供され、サーバが起動していることを確認してください。
            case DisconnectCause.MaxCcuReached: // OnOperationResponse。CCU Burstを使用せずにPhoton Cloudサブスクリプションを使用している場合、認証に（一時的に）失敗しました。サブスクリプションを更新してください。
            case DisconnectCause.InvalidRegion: // OnOperationResponse。アプリのPhoton Cloudサブスクリプションがいくつかの（他の）リージョンにロックされている場合に認証します。サブスクリプションまたはマスターサーバーのアドレスを更新してください。
            case DisconnectCause.None:  // エラーは追跡されませんでした。
                
                break;
            default:
                
                break;
        }
    }

}
