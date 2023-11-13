using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadingPanel : BasePanel
{
    public float LoadingBaseTime = 2f;
    float _timer;
    bool _isLoading = false;
    float _targetHight;
    Vector3 _startVector3;
    Image _image;
    event UnityAction _ActionOnEnterCompletelyBlack;
    public override void Init()
    {
        _image = GetControl<Image>("Image");
        _isLoading = true;
        _targetHight = 0.6f * Screen.height;
        //_image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
        _image.transform.position =2.4f * Vector3.up * Screen.height;
     

    }
    protected override void Update()
    {
  
        base.Update();
        _timer += Time.deltaTime;
        
        if (_image.transform.position.y > _targetHight)
            _image.transform.Translate(Vector3.down * Time.deltaTime * 2 * Screen.height, Space.World);
        if (_isLoading && _timer > LoadingBaseTime) 
        {
            _targetHight = -1.1f * Screen.height;
            _ActionOnEnterCompletelyBlack?.Invoke();
            _ActionOnEnterCompletelyBlack = null;
            _isLoading = false;
        }

        if (_image.transform.position.y < -1.1f * Screen.height) UIManager.Instance.HidePanel<LoadingPanel>();
        
    }
    public override void ShowMe()
    {
        base.ShowMe();
        _isLoading = true;
        if (_image != null)_image.transform.position= 2.4f * Vector3.up * Screen.height;
        _targetHight = 0.6f * Screen.height;
        _timer = 0;
    }
    public void AddWhileEnterCompletelyBlack(UnityAction Doing)
    {
        _ActionOnEnterCompletelyBlack += Doing;
    }
    // «∑Ò‘⁄º”‘ÿ
    public void SetLoading(bool isLoading)
    {
        _isLoading = isLoading;
    }
}
