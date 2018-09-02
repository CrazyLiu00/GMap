using System;
using System.Collections.Generic;
using GMap.NET;
using UnityEngine;
using Alt.Sketch.Demo.GMap.NET;

public class MapTools
{
    //    public Vector2 PointLatlngToLocalPos(PointLatLng latlng)
    //    {
    //        m_map.MapProvider.Projection.FromLatLngToPixel(latlng.Lat, latlng.Lng, (int)MapInfo.Instance.GetCurrentZoom());
    //    }

    public static string CreateMapName(MapLayer mapLayer)
    {
        string layerName = "dot_";
        if (mapLayer == MapLayer.Line)
        {
            layerName = "line_";
        }
        else if (mapLayer == MapLayer.Area)
        {
            layerName = "area_";
        }
        layerName += DateTime.Now.ToString("yyyyMMddHHmmss");
        return layerName;
    }
}
