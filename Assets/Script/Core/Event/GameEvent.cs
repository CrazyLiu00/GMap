using UnityEngine;
using System.Collections;
using System;
using GMap.NET;

public class GameEvent {

    public class MapEvent
    {
        public static Action<PointLatLng> OnMapMouseDown;
        public static Action<PointLatLng> OnMapMouseUp;
        public static Action OnCancelDraw;

        public static Action OnLoadComplete;

        public static Action<string, MapLayer,MapRoute> OnAddRoute;
        public static Action<string, MapLayer, MapRoute> OnRemoveRoute;
    }

    public class MapHandleEvent
    {
    }
}
