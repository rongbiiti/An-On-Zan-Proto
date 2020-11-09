using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class EnemyMove : MonoBehaviour
{

    public Transform[] _points;
    public GameObject _player;
    public bool isCanShinkuuha = false;
    public float _sprintSpeed = 1.5f;

    private float startSpeed;
    private Vector3 lastPlayerPos;
    private AudioSource playerAudioSource;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private Animator animator;
    private Rigidbody rb;
    private float speed, speedSeeker;               // Moving speed.
    private int jumpBool;                           // Animator variable related to jumping.
    private int groundedBool;                       // Animator variable related to whether or not the player is on ground.
    private bool jump;                              // Boolean to determine whether or not the player started a jump.
    private bool isColliding;                       // Boolean to determine if the player has collided with an obstacle.
    private Animator playerAnimator;
    private bool isFindAttackingPlayer;
    private bool isFindFootStepingPlayer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        startSpeed = agent.speed;

        // autoBraking を無効にすると、目標地点の間を継続的に移動します
        //(つまり、エージェントは目標地点に近づいても
        // 速度をおとしません)
        agent.autoBraking = false;

        animator.SetBool("Grounded", true);
    }

    void Start()
    {
        float range = Random.Range(0f, 100f);
        if (range <= 33f) {
            agent.destination = lastPlayerPos;
            Debug.Log("プレイヤーに向かうを引いた");
        } else if (range > 33f && range <= 66f) {
            int n = Random.Range(0, _points.Length);
            agent.destination = _points[n].position;
            destPoint = (n + 1) % _points.Length;
            Debug.Log("巡回地点に向かうを引いた");
        } else {
            StartCoroutine(nameof(DelayStart));
            Debug.Log("止まるを引いた");
        }
        
    }

    public void GetPlayerComponent()
    {
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

        isFindFootStepingPlayer = false;

        agent.speed = startSpeed;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyUp(KeyCode.F2)) {
            isCanShinkuuha = !isCanShinkuuha;
        }
    }

    private void FixedUpdate()
    {
        // プレイヤーのAudioSourceが再生中で、敵とプレイヤーの現在位置が一定距離以下なら、プレイヤーの現在位置を進行目標地点に設定する。
        if (playerAudioSource.isPlaying && Vector3.Distance(transform.position, _player.transform.position) < 4f) {

            lastPlayerPos = _player.transform.position;
            agent.destination = lastPlayerPos;
            isFindFootStepingPlayer = true;
            StopCoroutine(nameof(DelayStart));
            agent.speed = startSpeed;
            Debug.Log("プレイヤー発見：距離" + Vector3.Distance(transform.position, _player.transform.position));
        }

        if (isCanShinkuuha && playerAudioSource.isPlaying && Vector3.Distance(transform.position, _player.transform.position) < 10f) {

            lastPlayerPos = _player.transform.position;
            agent.destination = lastPlayerPos;
            Debug.Log("プレイヤー発見");
            if (!animator.GetBool(Animator.StringToHash("Attack")) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
                animator.SetBool("Attack", true);
                StartCoroutine("EnemyShinkuuha");
                Debug.Log("CPUが真空波攻撃をした");
                GotoNextPoint();
            }
        }

        // エージェントが現目標地点に近づいてきたら、
        // 次の目標地点を選択します
        if (!agent.pathPending && agent.remainingDistance < 2f) {
            if ((isFindFootStepingPlayer || isFindAttackingPlayer) && Vector3.Distance(transform.position, agent.destination) < 2f && !animator.GetBool(Animator.StringToHash("Attack")) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
                animator.SetBool("Attack", true);
                Debug.Log("CPUが攻撃した：距離" + Vector3.Distance(transform.position, _player.transform.position));
                isFindAttackingPlayer = false;
                GotoNextPoint();
            } else {
                GotoNextPoint();
                Debug.Log("次のポイントへ");
            }
        }

        if (playerAnimator.GetBool("Attack") && Vector3.Distance(transform.position, _player.transform.position) < 7f) {
            lastPlayerPos = _player.transform.position;
            agent.destination = lastPlayerPos;
            isFindAttackingPlayer = true;
            Debug.Log("攻撃中のプレイヤー発見：距離" + Vector3.Distance(transform.position, _player.transform.position));
            agent.speed = _sprintSpeed;
            StopCoroutine(nameof(DelayStart));
        } 
        else if (isFindAttackingPlayer) {
            SetSprintSpeed();
        }
        else {
            SetWalkSpeed();
        }
        
    }

    private void SetWalkSpeed()
    {
        Vector2 dir = new Vector2(rb.worldCenterOfMass.x, rb.worldCenterOfMass.z);
        speed = Vector2.ClampMagnitude(dir, 1f).magnitude;
        speedSeeker = Mathf.Clamp(speedSeeker, 0.3f, 0.8f);
        speed *= speedSeeker;
        animator.SetFloat("Speed", speed, 0.2f, Time.deltaTime);
    }

    private void SetSprintSpeed()
    {
        Vector2 dir = new Vector2(rb.worldCenterOfMass.x, rb.worldCenterOfMass.z);
        speed = Vector2.ClampMagnitude(dir, 1f).magnitude;
        speedSeeker = Mathf.Clamp(speedSeeker, 0.3f, 2.1f);
        speed *= speedSeeker;
        animator.SetFloat("Speed", speed, 2.1f, Time.deltaTime);
    }

    private void OnDisable()
    {
        animator.SetFloat("Speed", 0, 0, Time.deltaTime);
        agent.enabled = false;
    }

    public IEnumerator EnemyShinkuuha()
    {
        yield return new WaitForSeconds(0.6f);
        GetComponent<AttackProcess>().ShinkuuhaLauntchEnemy();
    }

    private IEnumerator DelayStart()
    {
        agent.speed = 0f;
        yield return new WaitForSeconds(3f);
        agent.speed = startSpeed;
        float range = Random.Range(0f, 50);
        if (range <= 25f) {
            agent.destination = lastPlayerPos;
            Debug.Log("プレイヤーに向かうを引いた");
        } else {
            int n = Random.Range(0, _points.Length);
            agent.destination = _points[n].position;
            destPoint = (n + 1) % _points.Length;
            Debug.Log("巡回地点に向かうを引いた");
        }
    }
}