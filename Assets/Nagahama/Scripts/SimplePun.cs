using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class SimplePun : MonoBehaviourPunCallbacks
{
    [SerializeField] Vector3[] _startPosition;
    [SerializeField] Vector3[] _startRotation;
    [SerializeField] string _playerPrefabName;
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] GameObject _panel;
    [SerializeField] GameObject _errorPanel;
    [SerializeField] TMP_Text _text;
    
    GameObject myplayer;
    float timeOutWait = 10f;
    bool timeOutWaitFlg;
    bool playerCreatedFlg = false;

    public void CreatePlayer()
    {
        Debug.Log(PhotonNetwork.NetworkingClient.UserId);
        // シーンの読み込みコールバックを登録.
        //SceneManager.sceneUnloaded += OnLoadedScene;
        //キャラクターを生成
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log("ルーム人数" + players.Length);
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
                PhotonNetwork.AutomaticallySyncScene = true;
                return;
            }
        }
        
        if (players.Length >= 2) {
            playerCreatedFlg = true;
            RoomManager.Instance.isCreatead = true;
        }
        if (playerCreatedFlg == false) {
            myplayer = PhotonNetwork.Instantiate(_playerPrefabName, _startPosition[PhotonNetwork.LocalPlayer.ActorNumber - 1], Quaternion.Euler(_startRotation[PhotonNetwork.LocalPlayer.ActorNumber - 1]), 0);
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
            StartCoroutine(nameof(MatchStart_Net));
        }
        playerCreatedFlg = true;
        RoomManager.Instance.isCreatead = true;
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
        RoomManager.Instance.isCreatead = false;
        Debug.Log("退出した");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PhotonNetwork.CurrentRoom.IsOpen = true;
        TimeOutCountDownStart();
        Debug.Log("他のプレイヤーが退室した");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        TimeOutCountDownReset();
        Debug.Log("他のプレイヤーが入室した");
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
                if (!PhotonNetwork.ReconnectAndRejoin()) {
                    Debug.Log("再入室に失敗したのでタイトルへ戻る");
                    PhotonNetwork.LeaveRoom();
                    RoomManager.Instance.isCreatead = false;
                }
                break;
            case DisconnectCause.OperationNotAllowedInCurrentState: // OnOperationResponseです。このクライアントでは (現在) 利用できない操作 (通常は許可されていません)。op Authenticate に対してのみ追跡されます。
            case DisconnectCause.CustomAuthenticationFailed:    // OnOperationResponseを使用しています。無効なクライアント値またはCloud Dashboardでカスタム認証を設定してPhoton Cloudで認証します。
            case DisconnectCause.DisconnectByClientLogic:   // サーバがこのクライアントをルームのロジック内から切断しました(C#のコード)。
            case DisconnectCause.InvalidAuthentication: // OnOperationResponse. 無効な AppId で Photon Cloud で認証しています。サブスクリプションを更新するか、Exit Gamesに連絡してください。
            case DisconnectCause.ExceptionOnConnect:    // OnStatusChanged: サーバが利用できないか、アドレスが間違っています。ポートが提供され、サーバが起動していることを確認してください。
            case DisconnectCause.MaxCcuReached: // OnOperationResponse。CCU Burstを使用せずにPhoton Cloudサブスクリプションを使用している場合、認証に（一時的に）失敗しました。サブスクリプションを更新してください。
            case DisconnectCause.InvalidRegion: // OnOperationResponse。アプリのPhoton Cloudサブスクリプションがいくつかの（他の）リージョンにロックされている場合に認証します。サブスクリプションまたはマスターサーバーのアドレスを更新してください。
            case DisconnectCause.None:  // エラーは追跡されませんでした。
                PhotonNetwork.LeaveRoom();
                RoomManager.Instance.isCreatead = false;
                break;
            default:
                PhotonNetwork.LeaveRoom();
                RoomManager.Instance.isCreatead = false;
                break;
        }
        Debug.Log("再入室を試みた");
        
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        TimeOutCountDownReset();
        Debug.Log("ルームに参加した");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log("ルームに参加ルーム人数" + players.Length);
        StartCoroutine("SearchPlayer");
    }

    public override void OnConnected()
    {
        base.OnConnected();
        TimeOutCountDownReset();
        Debug.Log("接続した"); GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log("接続したルーム人数" + players.Length);
    }

    IEnumerator ErrorLeaveRoom()
    {
        _panel.SetActive(false);
        _errorPanel.SetActive(true);
        timeOutWaitFlg = false;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        yield return new WaitForSeconds(3f);
        PhotonNetwork.LeaveRoom();
        RoomManager.Instance.isCreatead = false;
    }

    public IEnumerator SearchPlayer()
    {
        while(true) {
            
            yield return new WaitForSeconds(0.1f);
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if(players.Length >= 2) {
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
                        break;
                    }
                }
                break;
            }

        }
    }

    private void TimeOutCountDownStart()
    {
        timeOutWaitFlg = true;
        _panel.SetActive(true);
    }

    private void TimeOutCountDownReset()
    {
        timeOutWait = 10f;
        timeOutWaitFlg = false;
        _panel.SetActive(false);
    }

    public void CreatePlayerInCPUMatch()
    {
        myplayer = Instantiate(_playerPrefab, _startPosition[0], Quaternion.identity);
        FirstPersonAIO firstPersonAIO = myplayer.GetComponent<FirstPersonAIO>();
        firstPersonAIO.enabled = true;

        FPSMove fpsMove = myplayer.GetComponent<FPSMove>();
        fpsMove.enabled = true;

        myplayer.GetComponent<MaterialChanger>().enabled = false;

        GameManager_Net gameManager_Net = GameObject.Find("GameManager_Net").GetComponent<GameManager_Net>();
        gameManager_Net._player = myplayer;
        gameManager_Net.MatchStart();
    }

    private IEnumerator MatchStart_Net()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        Pauser.Pause();
        Time.timeScale = 0f;

        while (players.Length >= 2) {

            yield return new WaitForFixedUpdate();
            players = GameObject.FindGameObjectsWithTag("Player");
        }

        Pauser.Resume();
        Time.timeScale = 1f;

        GameManager_Net gameManager_Net = GameObject.Find("GameManager_Net").GetComponent<GameManager_Net>();
        gameManager_Net._player = myplayer;
        gameManager_Net.MatchStart();
    }
}