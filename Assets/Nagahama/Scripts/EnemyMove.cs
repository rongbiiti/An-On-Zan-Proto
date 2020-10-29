using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class EnemyMove : MonoBehaviour
{

    public Transform[] points;
    public GameObject player;
    private Vector3 playerTransform;
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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // autoBraking を無効にすると、目標地点の間を継続的に移動します
        //(つまり、エージェントは目標地点に近づいても
        // 速度をおとしません)
        agent.autoBraking = false;

        animator.SetBool("Grounded", true);

        GotoNextPoint();
    }

    public void GetPlayerComponent()
    {
        playerTransform = player.transform.position;
        playerAudioSource = player.GetComponent<AudioSource>();
    }

    void GotoNextPoint()
    {
        // 地点がなにも設定されていないときに返します
        if (points.Length == 0)
            return;

        // エージェントが現在設定された目標地点に行くように設定します
        agent.destination = points[destPoint].position;

        // 配列内の次の位置を目標地点に設定し、
        // 必要ならば出発地点にもどります
        destPoint = (destPoint + 1) % points.Length;
    }

    void Update()
    {
        if (playerAudioSource.isPlaying && Vector3.Distance(transform.position, playerTransform) < 4f) {
            
            playerTransform = player.transform.position;
            agent.destination = playerTransform;
            Debug.Log("プレイヤー発見");
        }
        // エージェントが現目標地点に近づいてきたら、
        // 次の目標地点を選択します
        if (!agent.pathPending && agent.remainingDistance < 2f) {
            if(Vector3.Distance(transform.position, playerTransform) < 2f) {
                animator.SetBool("Attack", true);
                Debug.Log("CPUが攻撃した");
                GotoNextPoint();
            } else {
                GotoNextPoint();
                Debug.Log("次のポイントへ");
            }
        }
            
    }

    private void FixedUpdate()
    {
        Vector2 dir = new Vector2(rb.worldCenterOfMass.x, rb.worldCenterOfMass.z);
        speed = Vector2.ClampMagnitude(dir, 1f).magnitude;
        speedSeeker = Mathf.Clamp(speedSeeker, 0.3f, 0.8f);
        speed *= speedSeeker;
        animator.SetFloat("Speed", speed, 0.2f, Time.deltaTime);
    }
}