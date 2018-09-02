using System;
using System.Collections.Generic;
using Alt.Sketch.Demo.GMap.NET;
using GMap.NET;
using Alt.Sketch.GMap.NET;
using Alt.Sketch;
using GMap.NET.MapProviders;
public class MapNormalSystem
{
    private Map m_map;
    private Dictionary<MapLayer, GMapOverlay> m_mapLayers = new Dictionary<MapLayer, GMapOverlay>();
    private GMapPolygon m_currentPolygon = null;
    private GMapRoute m_currentRoute = null;
    public MapNormalSystem(Map map)
    {
        this.m_map = map;
        InitMapLayer();
        RegisterEvent();
    }

    private void InitMapLayer()
    {
        m_mapLayers.Clear();
        m_mapLayers.Add(MapLayer.Area, new GMapOverlay());
        m_mapLayers.Add(MapLayer.Line, new GMapOverlay());
        m_mapLayers.Add(MapLayer.Dot, new GMapOverlay());
        foreach (var kv in m_mapLayers)
        {
            m_map.Overlays.Add(kv.Value);
        }
    }

    private void RegisterEvent()
    {
        UnityEngine.Debug.Log("register event =============");
        m_map.MouseDown += MouseUp;
        m_map.MouseDoubleClick += MouseDoubleClick;
        GameEvent.MapEvent.OnCancelDraw += CancelDraw;
        GameEvent.MapEvent.OnRemoveRoute += OnRemoveRoute;

    }

    private void UnRegisterEvent()
    {
        m_map.MouseUp -= MouseUp;
        m_map.MouseDoubleClick -= MouseDoubleClick;
        GameEvent.MapEvent.OnCancelDraw -= CancelDraw;
        GameEvent.MapEvent.OnRemoveRoute -= OnRemoveRoute;
    }

    public void Clear()
    {
        CancelDraw();
        UnRegisterEvent();
    }

    private void OnRemoveRoute(string routeName,MapLayer layer,MapRoute route)
    {
        if (layer == MapLayer.Area)
        {
            m_mapLayers[layer].Polygons.Remove(route as GMapPolygon);
            UnityEngine.Debug.Log("remove Line=== " + routeName);
            //m_mapLayers[layer].
        }
        else if (layer == MapLayer.Line)
        {
            m_mapLayers[layer].Routes.Remove(route as GMapRoute);
            UnityEngine.Debug.Log("remove area=== " + routeName);
        }
        m_map.Refresh();
    }

    private MapStatus mapStatus
    {
        get {
            return MapInfo.Instance.mapStatus;
        }
    }

    private DrawStatus m_DrawStatus = DrawStatus.None;

    private void MouseUp(object sender, Alt.GUI.MouseEventArgs e)
    {
        if (mapStatus == MapStatus.Normal || e.Button != Alt.GUI.MouseButtons.Left)
        {
            return;
        }
        PointLatLng latlng = m_map.FromLocalToLatLng(e.X, e.Y);
        if (mapStatus == MapStatus.DrawLine)
        {
            DrawLine(latlng);
        }
        else if (mapStatus == MapStatus.DrawArea)
        {
            DrawArea(latlng);
        }
    }

    private void MouseDoubleClick(object sender, Alt.GUI.MouseEventArgs e)
    {
        if (mapStatus == MapStatus.Normal)
        {
            return;
        }
        CancelDraw();
    }

    private void CancelDraw()
    {
        if (m_currentPolygon != null)
        {
            m_currentPolygon = null;
        }
        if (m_currentRoute != null)
        {
            m_currentRoute = null;
        }
        m_currentPoints.Clear();
        //mapStatus == MapStatus.Normal;
        UnityEngine.Debug.Log("cancel draw ");
    }

    private void DrawDot(PointLatLng latlng)
    {

    }
    private List<PointLatLng> m_currentPoints = new List<PointLatLng>();
    private void DrawArea(PointLatLng latlng)
    {
        //UnityEngine.Debug.Log("draw line " + latlng);
        if (m_currentPolygon == null)
        {
            UnityEngine.Debug.Log("create area " + latlng);
            string handleName = MapTools.CreateMapName(MapLayer.Area);
            m_currentPoints.Clear();
            m_currentPoints.Add(latlng);
            m_currentPolygon = new GMapPolygon(m_currentPoints, handleName);
            m_currentPolygon.Fill = new Alt.Sketch.SolidColorBrush(Color.FromArgb(50, Color.Red));
            m_currentPolygon.Stroke = new Pen(Color.Blue, 2);
            m_currentPolygon.IsHitTestVisible = true;
            if (GameEvent.MapEvent.OnAddRoute != null)
            {
                GameEvent.MapEvent.OnAddRoute(handleName,MapLayer.Area,m_currentPolygon);
            }
            m_mapLayers[MapLayer.Area].Polygons.Add(m_currentPolygon);
        }
        else
        {
            m_currentPolygon.Points.Add(latlng);
            m_map.UpdatePolygonLocalPosition(m_currentPolygon);
            m_map.Refresh();
        }
    }

    private void DrawLine(PointLatLng latlng)
    {
        //UnityEngine.Debug.Log("draw line " + latlng);
        if (m_currentRoute == null)
        {
            UnityEngine.Debug.Log("create line " + latlng);
            string handleName = MapTools.CreateMapName(MapLayer.Line);
            m_currentPoints.Clear();
            m_currentPoints.Add(latlng);
            m_currentRoute = new GMapRoute(m_currentPoints, handleName);
            m_currentRoute.Stroke = new Pen(Color.Yellow, 3);
            m_mapLayers[MapLayer.Line].Routes.Add(m_currentRoute);
            if (GameEvent.MapEvent.OnAddRoute != null)
            {
                GameEvent.MapEvent.OnAddRoute(handleName, MapLayer.Line, m_currentRoute);
            }
        }
        else
        {
            m_currentRoute.Points.Add(latlng);
            //m_currentRoute.
            m_map.UpdateRouteLocalPosition(m_currentRoute);
            m_map.Refresh();
        }
    }
}

