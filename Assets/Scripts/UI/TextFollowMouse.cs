using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFollowMouse : Text
{
    float _timer;
    bool _needDisplay = false;
    string _prepareText;
    public float Timer = 2f; 
    public void DisplayIt(string targetString)
    {
        _timer = 0;
        _prepareText = targetString;
        _needDisplay = true;
    }
    public void DisplayClear()
    {
        _timer = 0;
        text = "";
        _needDisplay = false;
    }
    private void Update()
    {
        if (_needDisplay)
        {
            _timer += Time.deltaTime;
            if(_timer > Timer)
            {
                text = _prepareText;
                _needDisplay = false;
            }
        }
    }
}
