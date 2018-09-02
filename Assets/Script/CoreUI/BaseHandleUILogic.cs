using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BaseHandleUILogic : MonoBehaviour {

    private Transform m_Root;
    private Button m_btnSelect;
    private Button m_btnDot;
    private Button m_btnLine;
    private Button m_btnMesh;

    private void Awake()
    {
        this.m_Root = transform;//transform.Find("baseHandle");
        this.m_btnSelect = this.m_Root.Find("Btnselect").GetComponent<Button>();
        this.m_btnDot = this.m_Root.Find("Btndot").GetComponent<Button>();
        this.m_btnLine = this.m_Root.Find("BtnLine").GetComponent<Button>();
        this.m_btnMesh = this.m_Root.Find("BtnMesh").GetComponent<Button>();
    }

    // Use this for initialization
    void Start () {

        this.m_btnSelect.onClick.AddListener(BtnSelect);
        this.m_btnDot.onClick.AddListener(BtnDot);
        this.m_btnLine.onClick.AddListener(BtnLine);
        this.m_btnMesh.onClick.AddListener(BtnMesh);
    }

    private void BtnSelect()
    {
        MapInfo.Instance.mapStatus = MapStatus.Normal;
    }

    private void BtnDot()
    {
        MapInfo.Instance.mapStatus = MapStatus.DrawDot;
    }

    private void BtnLine()
    {
        MapInfo.Instance.mapStatus = MapStatus.DrawLine;
    }

    private void BtnMesh()
    {
        MapInfo.Instance.mapStatus = MapStatus.DrawArea;
    }

    private void OnDisable()
    {
        this.m_btnSelect.onClick.RemoveListener(BtnSelect);
        this.m_btnDot.onClick.RemoveListener(BtnDot);
        this.m_btnLine.onClick.RemoveListener(BtnLine);
        this.m_btnMesh.onClick.RemoveListener(BtnMesh);
    }
}
