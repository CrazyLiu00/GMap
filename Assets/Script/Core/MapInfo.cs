using UnityEngine;
using System.Collections;
using Alt.Sketch.Demo.GMap.NET;
using Alt.Sketch.GMap.NET;
using Alt.Sketch;
using System.Collections.Generic;
using GMap.NET;

public class MapInfo{

    private static MapInfo instance;
    public static MapInfo Instance
    {
        get {
            if (instance == null)
            {
                instance = new MapInfo();
            }
            return instance;
        }
    }
    private MapEventSystem m_mapEventSys;
    private MapNormalSystem m_normalSystem = null;
    private MapStatus m_mapStatus = MapStatus.Normal;
    public MapStatus mapStatus
    {
        get {
            return m_mapStatus;
        }

        set {
            if (GameEvent.MapEvent.OnCancelDraw != null)
            {
                GameEvent.MapEvent.OnCancelDraw();
            }
            m_mapStatus = value;
        }
    }
    private Map m_map = null;
    public void SetMap(Map map)
    {
        this.m_map = map;
    }

    public PointLatLng GetCurrentCenter()
    {
        return m_map.Position;
    }

    public float GetCurrentZoom()
    {
        return (float)m_map.Zoom;
    }

    public void RegisterMapEvent()
    {
        m_mapEventSys = new MapEventSystem(m_map);
        m_normalSystem = new MapNormalSystem(m_map);
    }

    //坐标转换
    #region
    public UnityEngine.Vector2 PointLatlngToLocalPos(PointLatLng latlng)
    {
        GPoint point = m_map.MapProvider.Projection.FromLatLngToPixel(latlng.Lat, latlng.Lng, (int)m_map.Zoom);
        GPoint centerPoint = m_map.MapProvider.Projection.FromLatLngToPixel(m_map.Position.Lat, m_map.Position.Lng, (int)m_map.Zoom);
        return UnityEngine.Vector2.right * (point.X - centerPoint.X) + UnityEngine.Vector2.up * (point.Y - centerPoint.Y);
    }

    public PointLatLng LocalPosToPointLatlng(UnityEngine.Vector2 localPos)
    {
        GPoint centerPoint = m_map.MapProvider.Projection.FromLatLngToPixel(m_map.Position.Lat, m_map.Position.Lng, (int)m_map.Zoom);
        PointLatLng latlng = m_map.MapProvider.Projection.FromPixelToLatLng((long)(localPos.x + centerPoint.X),(long)(localPos.y + centerPoint.Y), (int)m_map.Zoom);
        return latlng;
    }
    #endregion
   
    public void LoadComplete()
    {
        if (!isload)
        {
            RegisterMapEvent();
            if (GameEvent.MapEvent.OnLoadComplete != null)
            {
                GameEvent.MapEvent.OnLoadComplete();
            }
        }
        isload = true;
    }

    public bool isload = false;

    public void Clear()
    {
        isload = false;
        m_map = null;
        if (m_mapEventSys != null)
        {
            m_mapEventSys.Clear();
        }
        if (m_normalSystem != null)
        {
            m_normalSystem.Clear();
        }
    }

}
