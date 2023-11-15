using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mono_GashaponMachine : MonoBehaviour
{
    public Button gacha;
    public Button gacha10;
    public Image egg;
    [SerializeField]
    int _unGetTime;
    [SerializeField]
    int _dynamicChance;
    [Header("概率公式")]
    public float baseChance100Percent;
    public int minChanceUp;
    public int maxGetNeed;

    private void Awake()
    {
        //概率初始化
        _unGetTime = 0;
        _dynamicChance = (int)(10 * baseChance100Percent);

        gacha.onClick.AddListener(Gacha);
        gacha10.onClick.AddListener(() => { for (int i = 0; i < 10; i++) Gacha();});
    }
    public void Gacha()
    {
        int random = Random.Range(0, 1000);
        if(random < _dynamicChance)
        {
            egg.color = Color.yellow;
            _dynamicChance = (int)(10 * baseChance100Percent);
            _unGetTime = 0;
        }
        else if(random < 300)
        {
            egg.color = Color.blue;
            _unGetTime++;
        }
        else
        {
            egg.color = Color.white;
            _unGetTime++;
        }

        GameObject newEgg = Instantiate<GameObject>(egg.gameObject);
        Rigidbody2D newEggRB = newEgg.AddComponent<Rigidbody2D>();
        newEggRB.gravityScale = 10;
        newEggRB.AddForce(new Vector2(Random.Range(-5000, 5000), 20000));
        newEgg.transform.SetParent(gacha.transform);
        newEgg.transform.localPosition = new Vector3(0, 0, 0);
        Destroy(newEgg, 5f);

        //触发保底
        if(_unGetTime > minChanceUp)
        {
            _dynamicChance += (int)((100 - baseChance100Percent) * 10 / (maxGetNeed - minChanceUp));
        }
    }
}
