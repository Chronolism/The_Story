using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapLayer : MonoBehaviour
{
    public int type;
    public Tilemap tilemap;
    public MapDetile mapDetile;
    private Button btnMap;
    private Toggle tgActive;

    private UnityEvent<MapLayer> m_OnClik = new UnityEvent<MapLayer>();
     
    public UnityEvent<MapLayer> OnClik => m_OnClik;

    public void Init(Tilemap tilemap, MapDetile mapDetile = null)
    {
        this.tilemap = tilemap;
        this.mapDetile = mapDetile;
        btnMap.onClick.AddListener(() =>
        {
            m_OnClik?.Invoke(this); 
        });
        tgActive.isOn = tilemap.gameObject.activeSelf;
        tgActive.onValueChanged.AddListener((o) =>
        {
            tgActive.isOn = o;
            tilemap.gameObject.SetActive(o);
        });
    }
}
