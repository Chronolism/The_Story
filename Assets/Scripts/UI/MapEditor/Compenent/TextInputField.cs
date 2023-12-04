using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextInputField : MonoBehaviour
{
    [SerializeField] private Text txtName;
    [SerializeField] private InputField ifContent;

    public void Init(string name, string content, UnityAction<string> callback)
    {
        txtName.text = name;
        ifContent.text = content;
        ifContent.onValueChanged.AddListener(callback);
    }
}
