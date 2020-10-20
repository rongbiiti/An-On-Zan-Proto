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
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 10000;
        PhotonNetwork.KeepAliveInBackground = 10;
    }

    void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    void OnGUI()
    {
        //ログインの状態を画面上に出力
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 10000;
        if (string.IsNullOrEmpty(_playerNameInputField.text)) {
            PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
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
        _errorText.text = "Room Creation Failed: " + message;
        Debug.LogError("Room Creation Failed: " + message);
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
            PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
            return;
        } else {
            PhotonNetwork.NickName = _playerNameInputField.text;
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        if (!PhotonNetwork.ReconnectAndRejoin()) {
            PhotonNetwork.LeaveRoom();
        }
    }

}
