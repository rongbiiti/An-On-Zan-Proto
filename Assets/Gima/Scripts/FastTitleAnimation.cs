using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastTitleAnimation : MonoBehaviour
{
    private GameObject TitleAnimation2;
    private GameObject TitleAnimation3;

    private Image Img;
    private GameObject image_object;
    float alfa = 1.0f;
    private bool flg = false;

    private float mCur;
    private float mLength;

    void Start()
    {
        TitleAnimation2 = GameObject.Find("FastTitleAnimationImage1");
        TitleAnimation3 = GameObject.Find("FastTitleAnimationImage3");

        image_object = GameObject.Find("TitleAnimationImage");
        Img = image_object.GetComponent<Image>();

        AnimatorStateInfo infAnim = TitleAnimation2.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        mLength = infAnim.length;
        flg = false;
        mCur = 0;
    }

    void FixedUpdate()
    {
        mCur += Time.deltaTime;
        if (mCur > mLength)
        {
            TitleAnimation2.GetComponent<Animator>().SetBool("FastTitleAnimationKey1", true);
            TitleAnimation3.GetComponent<Animator>().SetBool("FastTitleAnimationKey2", true);
            Img.color = new Color(1, 1, 1, alfa);
            flg = true;
        }
        if (flg)
        {
            alfa -= 0.01f;
            Img.color = new Color(1, 1, 1, alfa);
        }
    }
}
