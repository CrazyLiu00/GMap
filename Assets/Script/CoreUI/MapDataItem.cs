using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GMap.NET;
public class HandleData
{
    public string name;
    public MapLayer m_layer;
    public MapRoute route;
    public HandleData(string name,MapLayer layer,MapRoute route)
    {
        this.name = name;
        this.route = route;
        this.m_layer = layer;
    }
}

public class MapDataItem : MonoBehaviour {

    private Button m_BtnItem;
    private Text m_LabTitle;
    private Button m_BtnRemove;
    private HandleData m_handleData = null;
    private void Awake()
    {
        this.m_BtnItem = GetComponent<Button>();
        this.m_LabTitle = transform.Find("title").GetComponent<Text>();
        this.m_BtnRemove = transform.Find("btnRemove").GetComponent<Button>();
    }

    public void SetData(HandleData handleData)
    {
        this.m_handleData = handleData;
        this.m_LabTitle.text = handleData.name;
    }

    // Use this for initialization
    void Start () {

        this.m_BtnRemove.onClick.AddListener(BtnRemove);
        this.m_BtnItem.onClick.AddListener(BtnSelectItem);
    }

    private void BtnRemove()
    {
        if (GameEvent.MapEvent.OnRemoveRoute != null)
        {
            GameEvent.MapEvent.OnRemoveRoute(m_handleData.name,m_handleData.m_layer,m_handleData.route);
        }
    }

    private void BtnSelectItem()
    {
    }

}
