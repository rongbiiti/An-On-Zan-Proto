using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class EnemyMove : MonoBehaviour
{

    public Transform[] _points;                     // 巡回ポイント
    public GameObject _player;                      // プレイヤー
    public bool isCanShinkuuha = false;             // 真空波発動許可
    public float _sprintSpeed = 1.5f;               // ダッシュ速度
    public float _attackStartDistance = 2f;         // 攻撃を施行する距離
    public float _footStepSearchDistance = 4f;      // 足音反応距離
    public float _attackingSearchDistance = 9f;     // プレイヤーの攻撃動作反応距離


    private float startSpeed;                       // 初期移動速度
    private Vector3 lastPlayerPos;                  // 最後にプレイヤーの気配を探知した座標
    private AudioSource playerAudioSource;          // プレイヤーのAudioSource
    private int destPoint = 0;                      // 巡回ポイントを回す用変数
    private NavMeshAgent agent;                     // 敵のNavMeshAgentコンポーネント変数
    private Animator animator;                      // 敵のAnimatorコンポーネント変数
    private Rigidbody rb;                           // 敵のRigidbodyコンポーネント変数
    private float speed, speedSeeker;               // Moving speed.
    private int jumpBool;                           // Animator variable related to jumping.
    private int groundedBool;                       // Animator variable related to whether or not the player is on ground.
    private bool jump;                              // Boolean to determine whether or not the player started a jump.
    private bool isColliding;                       // Boolean to determine if the player has collided with an obstacle.
    private Animator playerAnimator;                // プレイヤーのAnimatorコンポーネント変数
    private bool isFindAttackingPlayer;             // プレイヤーの攻撃に反応したか
    private bool isFindFootStepingPlayer;           // プレイヤーの足音に反応したか
                                                    
    private void Awake()                            
    {                   
        // 自身のコンポーネント取得
        agent = GetComponent<NavMeshAgent>();       
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // 初期速度を記憶しておく
        startSpeed = agent.speed;

        // autoBraking を無効にすると、目標地点の間を継続的に移動します
        //(つまり、エージェントは目標地点に近づいても
        // 速度をおとしません)
        agent.autoBraking = false;

        // animatorのパラメーターに代入しておく
        animator.SetBool("Grounded", true);
    }

    void Start()
    {
        // ランダムな値を取得する
        int range = Random.Range(0, 2);

        // 0ならプレイヤー初期位置に真っ直ぐ向かう
        if (range == 0) {
            agent.destination = lastPlayerPos;
            Debug.Log("プレイヤーに向かうを引いた");

        // 1なら巡回ポイントをランダムで指定して向かう
        } else if (range == 1) {
            int n = Random.Range(0, _points.Length);
            agent.destination = _points[n].position;
            destPoint = (n + 1) % _points.Length;
            Debug.Log("巡回地点に向かうを引いた");
        // 2なら3秒その場で待機し、0か1を改めて抽選する。
        } else {
            StartCoroutine(nameof(DelayStart));
            Debug.Log("止まるを引いた");
        }
        
    }

    public void GetPlayerComponent()
    {
        // プレイヤーのコンポーネント等取得。他オブジェクトから呼ばれる。
        lastPlayerPos = _player.transform.position;
        playerAudioSource = _player.GetComponent<AudioSource>();
        playerAnimator = _player.GetComponent<Animator>();
    }

    void GotoNextPoint()
    {
        // 地点がなにも設定されていないときに返します
        if (_points.Length == 0)
            return;

        // エージェントが現在設定された目標地点に行くように設定します
        agent.destination = _points[destPoint].position;

        // 配列内の次の位置を目標地点に設定し、
        // 必要ならば出発地点にもどります
        destPoint = (destPoint + 1) % _points.Length;

        // プレイヤーの足音反応フラグを折る
        isFindFootStepingPlayer = false;

        // 初期速度代入
        agent.speed = startSpeed;
    }

    private void Update()
    {
        #region ShinkuuhaUnLock
        // 真空波解禁！！！！
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyUp(KeyCode.F2)) {
            isCanShinkuuha = !isCanShinkuuha;
        }
        #endregion
    }

    private void FixedUpdate()
    {
        // プレイヤーのAudioSourceが再生中で、敵とプレイヤーの現在位置が一定距離以下なら、プレイヤーの現在位置を進行目標地点に設定する。
        if (playerAudioSource.isPlaying && Vector3.Distance(transform.position, _player.transform.position) < _footStepSearchDistance) {

            // 音がしたプレイヤーの座標を記憶する。
            lastPlayerPos = _player.transform.position;

            // navmeshagentの目的地を音がした座標にする
            agent.destination = lastPlayerPos;

            // 足音反応フラグを立てる
            isFindFootStepingPlayer = true;

            // 開始時のくじで2を引いていたら、止めて音がした地点へ向かうようにする。
            StopCoroutine(nameof(DelayStart));

            // 行動できるように速度を戻す
            agent.speed = startSpeed;
            Debug.Log("プレイヤー発見：距離" + Vector3.Distance(transform.position, _player.transform.position));
        }        

        // プレイヤーが攻撃中で、自身とプレイヤーの現在地との距離が一定以下でかつ
        // プレイヤーの攻撃反応フラグが折れていたら
        if (playerAnimator.GetBool("Attack") && Vector3.Distance(transform.position, _player.transform.position) < _attackingSearchDistance && !isFindAttackingPlayer)
        {
            // 音がしたプレイヤーの座標を記憶する。
            lastPlayerPos = _player.transform.position;

            // navmeshagentの目的地を音がした座標にする
            agent.destination = lastPlayerPos;

            // 攻撃反応フラグを立てる
            isFindAttackingPlayer = true;

            // 開始時のくじで2を引いていたら、止めて音がした地点へ向かうようにする。
            StopCoroutine(nameof(DelayStart));

            // 行動できるように速度を戻す
            agent.speed = startSpeed;
            Debug.Log("攻撃中のプレイヤー発見：距離" + Vector3.Distance(transform.position, _player.transform.position));
        }
        else if (isFindAttackingPlayer)
        {
            // プレイヤーの攻撃反応フラグが立っていたら、
            // ダッシュ速度に切替えてプレイヤーに迫る
            SetSprintSpeed();
        }
        else
        {
            // プレイヤーの攻撃反応フラグが折れていたら、
            // 通常速度に切替える
            SetWalkSpeed();
        }

        #region Shinkuuha
        if (isCanShinkuuha && playerAudioSource.isPlaying && Vector3.Distance(transform.position, _player.transform.position) < 10f)
        {

            // 真空波関連
            lastPlayerPos = _player.transform.position;
            agent.destination = lastPlayerPos;
            Debug.Log("プレイヤー発見");
            if (!animator.GetBool(Animator.StringToHash("Attack")) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                animator.SetBool("Attack", true);
                StartCoroutine("EnemyShinkuuha");
                Debug.Log("CPUが真空波攻撃をした");
                GotoNextPoint();
            }
        }
        #endregion
        
        // 最初のくじで2を引いた場合、ここで処理を止めて停止動作を表現する。
        if (agent.speed < startSpeed) return;

        // エージェントが現目標地点に近づいてきたら、
        // 次の目標地点を選択します
        if (!agent.pathPending && agent.remainingDistance < _attackStartDistance) {

            // 足音か攻撃動作を探知したことがあり、
            //navmeshagentの目標地点と自信の位置との距離が一定以下でかつ
            //攻撃モーションを再生中でないとき
            if ((isFindFootStepingPlayer || isFindAttackingPlayer) && Vector3.Distance(transform.position, agent.destination) < _attackStartDistance && !animator.GetBool(Animator.StringToHash("Attack")) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
                // 攻撃する。
                animator.SetBool("Attack", true);
                Debug.Log("CPUが攻撃した：距離" + Vector3.Distance(transform.position, _player.transform.position));
                isFindAttackingPlayer = false;
                // 次の巡回地点を指定しておく
                GotoNextPoint();

            // 攻撃モーション中でなければ、次の巡回地点へ向かう
            } else if (!animator.GetBool(Animator.StringToHash("Attack")) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
                GotoNextPoint();
                Debug.Log("次のポイントへ");
            }
        }
        
    }

    private void SetWalkSpeed()
    {
        // 最初のくじで2を引いた場合、ここで処理を止めて停止動作を表現する。
        if (agent.speed < startSpeed) return;

        // 歩行用の速度を代入する。
        animator.SetFloat("Speed", 0.2f, 0.5f, Time.deltaTime);
    }

    private void SetSprintSpeed()
    {
        // 最初のくじで2を引いた場合、ここで処理を止めて停止動作を表現する。
        if (agent.speed < startSpeed) return;

        // ダッシュ用の速度を代入する。
        animator.SetFloat("Speed", _sprintSpeed, 0.2f, Time.deltaTime);
    }

    private void OnDisable()
    {
        // EnemyMoveコンポーネントが無効化されたとき、移動速度を0にして
        // navmeshagentも切っておく。
        animator.SetFloat("Speed", 0, 0, Time.deltaTime);
        agent.enabled = false;
    }

    #region ShinkuuhaLauntch
    public IEnumerator EnemyShinkuuha()
    {
        // 真空波発射
        yield return new WaitForSeconds(0.6f);
        GetComponent<AttackProcess>().ShinkuuhaLauntchEnemy();
    }
    #endregion

    private IEnumerator DelayStart()
    {
        // 最初のくじで2を引いた場合の処理。
        // まず移動速度を0にする
        agent.speed = 0f;
        // 3秒待つ
        yield return new WaitForSeconds(3f);
        // 移動速度をもとに戻す
        agent.speed = startSpeed;

        // くじを0か1で再抽選する
        int range = Random.Range(0, 1);

        // 0ならプレイヤー初期位置に真っ直ぐ向かう
        if (range == 0) {
            agent.destination = lastPlayerPos;
            Debug.Log("プレイヤーに向かうを引いた");

        }
        // 1なら巡回ポイントをランダムで指定して向かう
        else
        {
            int n = Random.Range(0, _points.Length);
            agent.destination = _points[n].position;
            destPoint = (n + 1) % _points.Length;
            Debug.Log("巡回地点に向かうを引いた");
        }
    }

    // UnityEditor上でのみ表示されるデバッグ用表示
    private void OnGUI()
    {
#if UNITY_EDITOR
        if (isFindAttackingPlayer)
        {
            GUI.Label(new Rect(400, 400, 100, 100), "プレイヤーの攻撃発見");
        }
        GUI.Label(new Rect(400, 300, 100, 50), "移動速度" + animator.GetFloat("Speed"));
#endif
    }
}