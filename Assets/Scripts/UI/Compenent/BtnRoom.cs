using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BtnRoom : MonoBehaviour
{
    public ulong steamID;
    public Button btnRoom;
    public Text roomName;
    private void Awake()
    {
        btnRoom = GetComponent<Button>();
        roomName = GetComponentInChildren<Text>();
    }
    public void Init(ulong steamID , string name)
    {
        this.steamID = steamID;
        roomName.text = name;
    }
}
