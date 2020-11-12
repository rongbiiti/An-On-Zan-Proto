using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputFieldDeFocus : MonoBehaviour
{
    private Selectable selectable;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject _inputField;
    private float preAxis;

    void Start()
    {
        selectable = GetComponent<Selectable>();
    }

    void Update()
    {
        if (eventSystem.currentSelectedGameObject != _inputField) return;

        float nowAxis = Input.GetAxis("DirPadV");

        if (Input.GetButtonUp("Dicide") || Input.GetButtonUp("Cancel") ||
            ( nowAxis < 0 && preAxis == 0.0f)) {
            selectable.Select();
        }

        preAxis = nowAxis;
    }
}
