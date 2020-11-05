using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZanAnimation : MonoBehaviour
{
    private GameObject zan;
    private GameObject zan1;
    private GameObject zan2;

    private Animator _ani;
    private Animation ani;
    private float mCur;
    private float mLength;
    public static bool flg = false;

    void Start()
    {
        zan = GameObject.Find("Zan");
        zan1 = GameObject.Find("Zan1");
        zan2 = GameObject.Find("Zan2");

        ani = zan.GetComponent<Animation>();
        _ani = zan.GetComponent<Animator>();

        AnimatorStateInfo infAnim = _ani.GetCurrentAnimatorStateInfo(0);
        mLength = infAnim.length;
    }

    void FixedUpdate()
    {
        if (flg)
        {
            _ani.SetBool("key", true);
            mCur = 0;
            zan2.GetComponent<Animator>().SetBool("ZanKey2", false);
            zan1.GetComponent<Animator>().SetBool("ZanKey1", false);
            flg = false;
        }

        if (_ani.GetBool("key"))
        {
            mCur += Time.deltaTime;
            if (mCur > mLength)
            { 
                _ani.SetBool("key", false);
                zan2.GetComponent<Animator>().SetBool("ZanKey2", true);
                zan1.GetComponent<Animator>().SetBool("ZanKey1", true);
            }
        }
    }
}
