using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FloatInputField : MonoBehaviour
{
    [SerializeField] private Text txtName;
    [SerializeField] private InputField ifContent;

    public void Init(string name, float value, UnityAction<float> callback)
    {
        txtName.text = name;
        ifContent.text = value.ToString();
        ifContent.onValueChanged.AddListener((o) =>
        {
            if (float.TryParse(o, out float newValue))
            {
                callback?.Invoke(newValue);
            }
        });
    }
}
