using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnlineMatch : MonoBehaviour
{
    public void OnClick()
    {
        FadeManager.Instance.LoadScene(1, 2);
        Debug.Log("ok");
    }
}
