using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class IntegerTrigerGUI : MonoBehaviour
{
    [SerializeField] private Text txtName;
    [SerializeField] private IntegerTriger intTriger;
    // Start is called before the first frame update
    public void Init(string name,int value,UnityAction<int> callback)
    {
        txtName.text = name;
        intTriger.value = value;
        intTriger.OnValueChanged.AddListener(callback);
    }
}
