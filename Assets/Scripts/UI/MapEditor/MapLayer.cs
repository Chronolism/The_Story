using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapLayer : MonoBehaviour
{
    public int type;
    public Tilemap tilemap;
    public MapDetile mapDetile;
    [SerializeField] private Button btnMap;
    [SerializeField] private Toggle tgActive;
    public Text txtName;

    private UnityEvent<MapLayer> m_OnClik = new UnityEvent<MapLayer>();
     
    public UnityEvent<MapLayer> OnClik => m_OnClik;

    private void Awake()
    {
        btnMap.onClick.AddListener(() =>
        {
            m_OnClik?.Invoke(this);
        });
        tgActive.onValueChanged.AddListener((o) =>
        {
            tgActive.isOn = o;
            tilemap.gameObject.SetActive(o);
        });
    }

    public void Init(Tilemap tilemap, MapDetile mapDetile = null)
    {
        this.tilemap = tilemap;
        this.mapDetile = mapDetile;
        tgActive.isOn = tilemap.gameObject.activeSelf;
        if(mapDetile != null && mapDetile.id != -1)
        {
            txtName.text = mapDetile.name;
            tilemap.name = mapDetile.name;
            tilemap.gameObject.name = mapDetile.name;
            tilemap.GetComponent<TilemapRenderer>().sortingLayerID = SortingLayer.NameToID(mapDetile.layer);
        }
    }

    public void ReLoad()
    {
        if (tilemap!= null && mapDetile.id != -1)
        {
            txtName.text = mapDetile.name;
            tilemap.name = mapDetile.name;
            tilemap.gameObject.name = mapDetile.name;
            tilemap.GetComponent<TilemapRenderer>().sortingLayerID = SortingLayer.NameToID(mapDetile.layer);
        }
    }
}
