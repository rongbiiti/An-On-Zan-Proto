using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 最初に選択状態にしたいボタンにつける
public class FirstSelectButton : MonoBehaviour
{
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
        GetComponent<Button>().OnSelect(null);
    }
}
