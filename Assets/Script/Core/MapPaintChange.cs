using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapPaintChange : MonoBehaviour {

    private AltSketchPaint m_paint = null;
    private void Awake()
    {
        this.m_paint = GetComponent<AltSketchPaint>();
    }
    // Use this for initialization
    void Start () {

        GameEvent.MapEvent.OnLoadComplete += OnLoadComplete;
    }

    private void OnLoadComplete()
    {
        m_paint.RenderType = Alt.Sketch.Rendering.RenderType.Hardware;
    }

    private void OnDisable()
    {
        GameEvent.MapEvent.OnLoadComplete -= OnLoadComplete;
    }

}
