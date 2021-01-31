using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResetSlope : MonoBehaviour, IEndDragHandler
{
    Slider slider;
    GameObject nullText;
    GameObject tmpText;
    GameObject dff;
    ChangeText ct;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.wholeNumbers = true;
        nullText = GameObject.Find("NullText");
        dff = transform.parent.Find("DropdownForFont").gameObject;
        ct = dff.GetComponent<ChangeText>();
    }

    public void OnEndDrag(PointerEventData data)
    {
        tmpText = ct.targetText;
        ct.targetText = nullText;
        slider.value = 0;
        ct.targetText = tmpText;
    }
}
