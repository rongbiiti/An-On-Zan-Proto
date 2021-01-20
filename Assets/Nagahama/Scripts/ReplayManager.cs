using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class ReplayManager : MonoBehaviourPunCallbacks
{
    private Queue<Vector3> oldPos = new Queue<Vector3>(420);            // Positionの配列
    private Queue<Quaternion> oldRot = new Queue<Quaternion>(420);      // Rotationの配列
    private Queue<bool> oldAtkBool = new Queue<bool>(420);              // AnimatorのAttackフラグの配列
    private Queue<bool> oldDeathBool = new Queue<bool>(420);            // AnimatorのDeathフラグの配列
    private Queue<float> oldSpeed = new Queue<float>(420);              // AnimatorのSpeedの値の配列
    private Queue<float> oldH = new Queue<float>(420);                  // AnimatorのHの値の配列
    private Queue<float> oldV = new Queue<float>(420);                  // AnimatorのVの値の配列

    private Queue<Vector3> oldPosMemo = new Queue<Vector3>(420);        // もう一度再生する用に保存している
    private Queue<Quaternion> oldRotMemo = new Queue<Quaternion>(420);
    private Queue<bool> oldAtkBoolMemo = new Queue<bool>(420);
    private Queue<bool> oldDeathBoolMemo = new Queue<bool>(420);
    private Queue<float> oldSpeedMemo = new Queue<float>(420);
    private Queue<float> oldHMemo = new Queue<float>(420);
    private Queue<float> oldVMemo = new Queue<float>(420);

    [HideInInspector] public bool isRunning;        // 他スクリプトから制御する用。trueにするとリプレイ開始
    private bool isLaunched;                        // 一度でも再生したか
    public Transform target_P;                      // 値を注入するオブジェクト
    public FirstPersonAIO firstPerson;              // プレイヤー操作受付スクリプト
    public Animator animator;                       // Animator
    public bool _isCPU;                             // ターゲットがCPUか
    public GameObject playerCamera;                 // プレイヤーのカメラ
    public GameObject _replayCamera;                // リプレイ用のカメラ
    public PlayerDeathProcess _playerDeathProcess;  // プレイヤーの死亡スクリプト
    private Rigidbody rb;                           // RigidBody

    public void ReplayStart()
    {
        isRunning = true;           // 再生開始

        animator.SetBool("Replay", true);   // Animatorのリプレイ用のフラグオン。遷移条件に関係する

        oldPos.Enqueue(target_P.position);  // キューに直前の座標を入れる
        oldRot.Enqueue(target_P.rotation);  // 直前の回転
        oldAtkBool.Enqueue(animator.GetBool("Attack")); // 直前の攻撃フラグ
        oldDeathBool.Enqueue(animator.GetBool("Death"));// 直前の死亡フラグ
        oldSpeed.Enqueue(0);                // 停止させるためにスピードを0にする
        oldH.Enqueue(0);
        oldV.Enqueue(0);

        rb = target_P.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;     // 停止させるために運動量0にする

        if (oldPos.Count > 420)
        {
            AllDequeue();       // キューの最大値を超えてたらデキューする
        }

        if (!isLaunched) {
            oldPosMemo = new Queue<Vector3>(oldPos);            // 初回再生時なら配列を別のものにコピーする
            oldRotMemo = new Queue<Quaternion>(oldRot);
            oldAtkBoolMemo = new Queue<bool>(oldAtkBool);
            oldDeathBoolMemo = new Queue<bool>(oldDeathBool);
            oldSpeedMemo = new Queue<float>(oldSpeed);
            oldHMemo = new Queue<float>(oldH);
            oldVMemo = new Queue<float>(oldV);

            if (PhotonNetwork.IsConnected) {
                // リプレイ用に同期を停止させる
                PhotonTransformView photonTransformView = target_P.gameObject.GetComponent<PhotonTransformView>();
                photonTransformView.m_SynchronizePosition = false;
                photonTransformView.m_SynchronizeRotation = false;

                PhotonAnimatorView photonAnimatorView = target_P.gameObject.GetComponent<PhotonAnimatorView>();
                photonAnimatorView.enabled = false;
            }

        } else {
            // もう一度再生する用
            // キューをクリアして保存していた配列の値をまたコピーする
            oldPos.Clear();
            oldRot.Clear();
            oldAtkBool.Clear();
            oldDeathBool.Clear();
            oldSpeed.Clear();
            oldH.Clear();
            oldV.Clear();

            oldPos = new Queue<Vector3>(oldPosMemo);
            oldRot = new Queue<Quaternion>(oldRotMemo);
            oldAtkBool = new Queue<bool>(oldAtkBoolMemo);
            oldDeathBool = new Queue<bool>(oldDeathBoolMemo);
            oldSpeed = new Queue<float>(oldSpeedMemo);
            oldH = new Queue<float>(oldHMemo);
            oldV = new Queue<float>(oldVMemo);
        }        

        if (!_isCPU)
        {
            firstPerson.enabled = false;
            _playerDeathProcess.MeshtoOne();
            playerCamera.SetActive(false);
            _replayCamera.SetActive(true);
        }

        isLaunched = true;
    }

    private void FixedUpdate()
    {
        if (!isRunning) {
            // 試合中は値を記録する
            oldPos.Enqueue(target_P.position);
            oldRot.Enqueue(target_P.rotation);
            oldAtkBool.Enqueue(animator.GetBool("Attack"));
            oldDeathBool.Enqueue(animator.GetBool("Death"));
            oldSpeed.Enqueue(animator.GetFloat("Speed"));
            oldH.Enqueue(animator.GetFloat("H"));
            oldV.Enqueue(animator.GetFloat("V"));

            if (oldPos.Count > 420)
            {
                // 最大値を超えたらデキューする
                AllDequeue();
            }

        } else {
            // リプレイ中は記録していた値をターゲットに注入する
            target_P.position = oldPos.Dequeue();
            target_P.rotation = oldRot.Dequeue();
            animator.SetBool("Attack", oldAtkBool.Dequeue());
            animator.SetBool("Death", oldDeathBool.Dequeue());
            animator.SetFloat("Speed", oldSpeed.Dequeue());
            animator.SetFloat("H", oldH.Dequeue());
            animator.SetFloat("V", oldV.Dequeue());

            if (oldPos.Count == 0)
            {
                // キューがすべてなくなったらフラグをおる
                isRunning = false;
                Debug.Log("再生終了");
            }
        }
    }

    private void AllDequeue()
    {
        oldPos.Dequeue();
        oldRot.Dequeue();
        oldAtkBool.Dequeue();
        oldDeathBool.Dequeue();
        oldSpeed.Dequeue();
        oldH.Dequeue();
        oldV.Dequeue();
    }
}
