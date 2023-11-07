using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IStayInImageDisplay<T> where T : BasePanel
{
    void OnPointerEnter(string name, PointerEventData eventData);
    void OnPointerExit(string name, PointerEventData eventData);
}
