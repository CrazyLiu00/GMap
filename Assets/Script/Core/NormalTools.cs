using UnityEngine;
using System.Collections;
using Alt.Sketch.Demo.GMap.NET;
using Alt.Sketch.GMap.NET;
using Alt.Sketch;
using GMap.NET;

public class NormalTools {

    //public static PointLatLng ChangToMe(PointLatLng p)
    //{
    //    PointLatLng m_p = new PointLatLng();

    //    double lng = p.Lng;
    //    if (lng >= 0 )
    //    {
    //        lng = lng - 180;
    //    }
    //    else if (lng < 0 )
    //    {
    //        lng = lng + 180;
    //    }
    //    if (lng > 180)
    //    {
    //        lng = lng - 360;
    //    }
    //    if (lng < -180)
    //    {
    //        lng = lng + 360;
    //    }
    //    m_p.Lat = p.Lat;
    //    m_p.Lng = lng;
    //    return m_p;
    //}

    //public static PointLatLng ChangToMe1(PointLatLng p)
    //{
    //    PointLatLng m_p = new PointLatLng();

    //    double lng = p.Lng;
    //    if (lng >= 0)
    //    {
    //        lng = lng - 180;
    //    }
    //    else if (lng < 0)
    //    {
    //        lng = lng + 180;
    //    }
    //    if (lng > 180)
    //    {
    //        lng = lng - 360;
    //    }
    //    if (lng < -180)
    //    {
    //        lng = lng + 360;
    //    }
    //    m_p.Lat = p.Lat;
    //    m_p.Lng = lng;
    //    return m_p;
    //}

    public static UnityEngine.Vector2 RealPos(PointLatLng t_p)
    {
        UnityEngine.Vector2 realP = new UnityEngine.Vector2();
        PointLatLng screen_pos = MapInfo.Instance.GetCurrentCenter();
        float pix_length = 256 * Mathf.Pow(2, (float)MapInfo.Instance.GetCurrentZoom());
        float m_x = (float)(t_p.Lng - screen_pos.Lng);
        float m_y = Mathf.Log(Mathf.Tan(Mathf.PI / 4 + (float)t_p.Lat * Mathf.PI / 360)) * 180 / Mathf.PI - Mathf.Log(Mathf.Tan(Mathf.PI / 4 + (float)screen_pos.Lat * Mathf.PI / 360)) * 180 / Mathf.PI;
        realP.x = m_x * pix_length / 360;
        realP.y = m_y * pix_length / 360;
        return realP;
    }

}
