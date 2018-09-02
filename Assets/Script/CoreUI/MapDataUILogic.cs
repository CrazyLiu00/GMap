using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GMap.NET;

public class MapDataUILogic : MonoBehaviour {

    private GameObject m_item;
    private Transform m_grid;

    private void Awake()
    {
        this.m_grid = transform.Find("drag/grid");
        this.m_item = m_grid.transform.Find("item").gameObject;
    }

    // Use this for initialization
    void Start () {

        GameEvent.MapEvent.OnAddRoute += OnAddRoute;
        GameEvent.MapEvent.OnRemoveRoute += OnRemoveRoute;
    }

    private void OnAddRoute(string routeName,MapLayer layer,MapRoute route)
    {
        CreateItem(routeName, layer, route);
    }

    private void CreateItem(string routeName,MapLayer layer,MapRoute route)
    {
        GameObject itemObj = Instantiate(m_item) as GameObject;
        itemObj.name = routeName;
        itemObj.transform.SetParent(m_grid,false);
        itemObj.transform.localScale = Vector3.one;
        MapDataItem dataItem = itemObj.AddComponent<MapDataItem>();
        itemObj.SetActive(true);
        dataItem.SetData(new HandleData(routeName, layer,route));
        Debug.Log("====" + routeName);

    }

    private void OnDisable()
    {
        GameEvent.MapEvent.OnAddRoute -= OnAddRoute;
        GameEvent.MapEvent.OnRemoveRoute -= OnRemoveRoute;
    }

    private void OnRemoveRoute(string name,MapLayer layer,MapRoute route)
    {
        Transform tran = this.m_grid.Find(name);
        if (tran == null)
        {
            return;
        }
        GameObject.DestroyImmediate(tran.gameObject);
    }
}
