using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mono_GashaponMachine : MonoBehaviour
{
    public Button gacha;
    public Image egg;
    private void Awake()
    {
        gacha.onClick.AddListener(Gacha);
    }
    public void Gacha()
    {
        int random = Random.Range(0, 1000);
        if(random > 993)
        {
            egg.color = Color.yellow;
        }else if(random > 700)
        {
            egg.color = Color.blue;
        }else
            egg.color = Color.white;
        GameObject newEgg = Instantiate<GameObject>(egg.gameObject);
        Rigidbody2D newEggRB = newEgg.AddComponent<Rigidbody2D>();
        newEggRB.gravityScale = 10;
        newEggRB.AddForce(new Vector2(Random.Range(-5000, 5000), 20000));
        newEgg.transform.SetParent(gacha.transform);
        newEgg.transform.localPosition = new Vector3(0, 0, 0);
        Destroy(newEgg, 5f);
    }
}
