using System;
using System.Collections.Generic;
using Alt.Sketch.Demo.GMap.NET;
using GMap.NET;
public class MapEventSystem
{
    private Map m_map;
    public MapEventSystem(Map map)
    {
        this.m_map = map;
        RegisterEvent();
    }

    private void RegisterEvent()
    {
        m_map.MouseDown += M_map_MouseDown;
        m_map.MouseUp += M_map_MouseUp;
    }

    private void UnRegisterEvent()
    {
        m_map.MouseDown -= M_map_MouseDown;
        m_map.MouseUp -= M_map_MouseUp;
    }

    public void Clear()
    {
        UnRegisterEvent();
    }

    private PointLatLng GetPointLatLng(int x,int y)
    {
        PointLatLng point = m_map.FromLocalToLatLng(x, y);
        //var px = m_map.MapProvider.Projection.FromLatLngToPixel(point.Lat, point.Lng, (int)MapInfo.Instance.GetCurrentZoom());
        //UnityEngine.Debug.Log("click  " +  point+ "  " + px);
        return point;
    }

    private void M_map_MouseUp(object sender, Alt.GUI.MouseEventArgs e)
    {
        if (e.Button != Alt.GUI.MouseButtons.Left)
        {
            return;
        }
        if (MapInfo.Instance.mapStatus == MapStatus.Normal)
        {
            return;
        }
        GetPointLatLng(e.X,e.Y);
    }

    private void M_map_MouseDown(object sender, Alt.GUI.MouseEventArgs e)
    {
        if (e.Button != Alt.GUI.MouseButtons.Left)
        {
            return;
        }
        if (MapInfo.Instance.mapStatus == MapStatus.Normal)
        {
            return;
        }
        GetPointLatLng(e.X, e.Y);
    }
}
