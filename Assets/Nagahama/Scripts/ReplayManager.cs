using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayManager : MonoBehaviour
{
    private Queue<Vector3> oldPos = new Queue<Vector3>(420);
    private Queue<Quaternion> oldRot = new Queue<Quaternion>(420);
    private Queue<bool> oldAtkBool = new Queue<bool>(420);
    private Queue<bool> oldDeathBool = new Queue<bool>(420);
    private Queue<float> oldSpeed = new Queue<float>(420);
    private Queue<float> oldH = new Queue<float>(420);
    private Queue<float> oldV = new Queue<float>(420);

    private Queue<Vector3> oldPosMemo = new Queue<Vector3>(420);
    private Queue<Quaternion> oldRotMemo = new Queue<Quaternion>(420);
    private Queue<bool> oldAtkBoolMemo = new Queue<bool>(420);
    private Queue<bool> oldDeathBoolMemo = new Queue<bool>(420);
    private Queue<float> oldSpeedMemo = new Queue<float>(420);
    private Queue<float> oldHMemo = new Queue<float>(420);
    private Queue<float> oldVMemo = new Queue<float>(420);

    [HideInInspector] public bool isRunning;
    private bool isLaunched;
    public Transform target_P;
    public FirstPersonAIO firstPerson;
    public Animator animator;
    public bool _isCPU;
    public GameObject playerCamera;
    public GameObject _replayCamera;
    public PlayerDeathProcess _playerDeathProcess;
    private Rigidbody rb;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //ReplayStart();
            Debug.Log("^v^");
        }
    }

    public void ReplayStart()
    {
        isRunning = true;
        

        animator.SetBool("Replay", true);

        oldPos.Enqueue(target_P.position);
        oldRot.Enqueue(target_P.rotation);
        oldAtkBool.Enqueue(animator.GetBool("Attack"));
        oldDeathBool.Enqueue(animator.GetBool("Death"));
        oldSpeed.Enqueue(0);
        oldH.Enqueue(0);
        oldV.Enqueue(0);

        rb = target_P.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;

        if (oldPos.Count > 420)
        {
            AllDequeue();
        }

        if (!isLaunched) {
            oldPosMemo = new Queue<Vector3>(oldPos);
            oldRotMemo = new Queue<Quaternion>(oldRot);
            oldAtkBoolMemo = new Queue<bool>(oldAtkBool);
            oldDeathBoolMemo = new Queue<bool>(oldDeathBool);
            oldSpeedMemo = new Queue<float>(oldSpeed);
            oldHMemo = new Queue<float>(oldH);
            oldVMemo = new Queue<float>(oldV);

        } else {
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
            oldPos.Enqueue(target_P.position);
            oldRot.Enqueue(target_P.rotation);
            oldAtkBool.Enqueue(animator.GetBool("Attack"));
            oldDeathBool.Enqueue(animator.GetBool("Death"));
            oldSpeed.Enqueue(animator.GetFloat("Speed"));
            oldH.Enqueue(animator.GetFloat("H"));
            oldV.Enqueue(animator.GetFloat("V"));

            if (oldPos.Count > 420)
            {
                AllDequeue();
            }
        } else {
            target_P.position = oldPos.Dequeue();
            target_P.rotation = oldRot.Dequeue();
            animator.SetBool("Attack", oldAtkBool.Dequeue());
            animator.SetBool("Death", oldDeathBool.Dequeue());
            animator.SetFloat("Speed", oldSpeed.Dequeue());
            animator.SetFloat("H", oldH.Dequeue());
            animator.SetFloat("V", oldV.Dequeue());

            if (oldPos.Count == 0)
            {
                isRunning = false;
                //animator.SetBool("Replay", false);
                if (!_isCPU)
                {
                    //firstPerson.enabled = true;
                    //animator.SetBool("Aim", true);
                }
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

    private void OnGUI()
    {
        if (!isRunning) {
            if (!_isCPU){
                //GUI.Label(new Rect(500, 700, 500, 100), "記録中:" + oldPos.Count / 60 + "秒(" + oldPos.Count + "フレーム)");
                //GUI.Label(new Rect(500, 700, 500, 100), "" + animator.GetFloat("Speed"));
            } else {
                //GUI.Label(new Rect(500, 750, 500, 100), "CPU記録中:" + oldPos.Count / 60 + "秒(" + oldPos.Count + "フレーム)");
            }
            
        } else {
            if (!_isCPU) {
                //GUI.Label(new Rect(500, 700, 500, 100), "再生中残り:" + oldPos.Count / 60 + "秒(" + oldPos.Count + "フレーム)");

            } else {
                //GUI.Label(new Rect(500, 750, 500, 100), "CPU再生中残り:" + oldPos.Count / 60 + "秒(" + oldPos.Count + "フレーム)");
            }
        }
    }
}
