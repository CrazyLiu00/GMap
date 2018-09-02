using UnityEngine;
using System.Collections;
using GMap.NET;
using Alt.Sketch;

public class GMapTest : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {

        UnityEngine.Vector2 pos = MapInfo.Instance.PointLatlngToLocalPos(new PointLatLng(40f, 116.4f));
        Debug.Log(pos);
        //GPoint point = MapInfo.Instance.m_map.FromLatLngToLocal(new PointLatLng(40f, 116.4f));
        //Debug.Log(point.X + "  " + point.Y);
        //PointI pointI = MapInfo.Instance.m_map.LocalPosToCanvas(new PointI(40, 116));
        //Debug.Log(pointI.X + "  " + pointI.Y);

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
