using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BtnRoom : MonoBehaviour
{
    public FriendRoom room;
    public Button btnRoom;
    public Text roomName;
    private void Awake()
    {
        btnRoom = GetComponent<Button>();
        roomName = GetComponentInChildren<Text>();
    }
    public void Init(FriendRoom room , string name)
    {
        this.room = room;
        roomName.text = name;
    }
}
