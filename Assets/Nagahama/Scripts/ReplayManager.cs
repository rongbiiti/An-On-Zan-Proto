﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayManager : MonoBehaviour
{
    [HideInInspector] public Queue<Vector3> oldPos = new Queue<Vector3>(420);
    [HideInInspector] public Queue<Quaternion> oldRot = new Queue<Quaternion>(420);
    [HideInInspector] public Queue<bool> oldAtkBool = new Queue<bool>(420);
    [HideInInspector] public Queue<bool> oldDeathBool = new Queue<bool>(420);

    [HideInInspector] public bool isRunning;
    public Transform target_P;
    public FirstPersonAIO firstPerson;
    public Animator animator;
    public bool _isCPU;
    public GameObject playerCamera;
    public GameObject _replayCamera;
    public PlayerDeathProcess _playerDeathProcess;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("^v^");
        }
    }

    public void ReplayStart()
    {
        isRunning = true;
        if (!_isCPU)
        {
            firstPerson.enabled = false;
            _playerDeathProcess.MeshtoOne();
            playerCamera.SetActive(false);
            _replayCamera.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        if (!isRunning)
        {
            oldPos.Enqueue(target_P.position);
            oldRot.Enqueue(target_P.rotation);
            oldAtkBool.Enqueue(animator.GetBool("Attack"));
            oldDeathBool.Enqueue(animator.GetBool("Death"));
            Debug.Log("記録中");

            if(oldPos.Count > 420)
            {
                oldPos.Dequeue();
                oldRot.Dequeue();
                oldAtkBool.Dequeue();
                oldDeathBool.Dequeue();
            }
        }
        else 
        {
            target_P.position = oldPos.Dequeue();
            target_P.rotation = oldRot.Dequeue();
            animator.SetBool("Attack", oldAtkBool.Dequeue());
            animator.SetBool("Death", oldDeathBool.Dequeue());
            
            
            Debug.Log("再生中");
            if (oldPos.Count == 0)
            {
                isRunning = false;
                if (!_isCPU)
                {
                    //firstPerson.enabled = true;
                }
                    
                Debug.Log("再生終了");
            }
        }
        
    }

    private void OnGUI()
    {
        if (!isRunning)
        {
            if (!_isCPU)
            {
                GUI.Label(new Rect(500, 700, 500, 100), "記録中:" + oldPos.Count / 60 + "秒(" + oldPos.Count + "フレーム)");
            }
            else
            {
                GUI.Label(new Rect(500, 750, 500, 100), "CPU記録中:" + oldPos.Count / 60 + "秒(" + oldPos.Count + "フレーム)");

            }
        }
        else
        {
            if (!_isCPU)
            {
                GUI.Label(new Rect(500, 700, 500, 100), "再生中残り:" + oldPos.Count / 60 + "秒(" + oldPos.Count + "フレーム)");

            }
            else
            {
                GUI.Label(new Rect(500, 750, 500, 100), "CPU再生中残り:" + oldPos.Count / 60 + "秒(" + oldPos.Count + "フレーム)");

            }
        }
    }

}