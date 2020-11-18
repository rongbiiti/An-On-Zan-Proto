using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCancel : MonoBehaviour
{
    [SerializeField] private Selectable _selectable;

    public void FocusButton()
    {
        _selectable.Select();
    }
}
