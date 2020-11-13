using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIEventSound : EventTrigger
{
    public override void OnSubmit(BaseEventData eventData)
    {
        base.OnSubmit(eventData);
        UISoundManager.Instance.PlayDecideSE();
    }

    public override void OnCancel(BaseEventData eventData)
    {
        base.OnCancel(eventData);
        UISoundManager.Instance.PlayCancelSE();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        UISoundManager.Instance.PlaySelectSE();
    }
}
