using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnlineMatch : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene(1);
        Debug.Log("ok");
    }
}
