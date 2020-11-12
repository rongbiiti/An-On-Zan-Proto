using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 1.UIシステムを使うときに必要なライブラリ
using UnityEngine.UI;
// 2.Scene関係の処理を行うときに必要なライブラリ
using UnityEngine.SceneManagement;

public class CPU_Restart : MonoBehaviour
{
    public void SceneReload()
    {
        SceneManager.LoadScene(3);
    }

}
