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

    void Start()
    {
        selectable = GetComponent<Selectable>();
    }

    void Update()
    {
        if (eventSystem.currentSelectedGameObject != _inputField) return;

        if (Input.GetButtonUp("Dicide") || Input.GetButtonUp("Cancel")) {
            selectable.Select();
        }

    }
}
