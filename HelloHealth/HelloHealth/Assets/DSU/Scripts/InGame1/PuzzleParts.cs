using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GPSBound
{
    // Left/Top 
    [Space]
    public int m_LTI;
    public double m_LT_Lati;
    public double m_LT_Long;
    // Right/Top
    [Space]
    public int m_RTI;
    public double m_RT_Lati;
    public double m_RT_Long;
    // Left/Bottom
    [Space]
    public int m_LBI;
    public double m_LB_Lati;
    public double m_LB_Long;
    // Right/Bottom
    [Space]
    public int m_RBI;
    public double m_RB_Lati;
    public double m_RB_Long;

    public bool contains(float Lati, float Long)
    {
        bool bResult = ptInTriangle(Lati, Long, m_LT_Lati, m_LT_Long, m_RT_Lati, m_RT_Long, m_LB_Lati, m_LB_Long);
        if (bResult == true)
            return true;
        bResult = ptInTriangle(Lati, Long, m_LB_Lati, m_LB_Long, m_RB_Lati, m_RB_Long, m_RT_Lati, m_RT_Long);
        return bResult;
    }

    private bool ptInTriangle(
        double px, double py, double p0x,
        double p0y, double p1x, double p1y, 
        double p2x, double p2y) {
        var dX = px - p2x;
        var dY = py - p2y;
        var dX21 = p2x - p1x;
        var dY12 = p1y - p2y;
        var D = dY12 * (p0x - p2x) + dX21 * (p0y - p2y);
        var s = dY12 * dX + dX21 * dY;
        var t = (p2y - p0y) * dX + (p0x - p2x) * dY;
        if (D < 0) return s <= 0 && t <= 0 && s + t >= D;
        return s >= 0 && t >= 0 && s + t <= D;
    }
}

public class PuzzleParts : MonoBehaviour
{
    public enum OpenState
    {
        CLOSE,
        OPEN,
        FIND,
    }

    private static Color FINDED_COLOR = new Color(1, 0, 0, 0.6509804f);
    private static Color CLOSE_COLOR = new Color(0, 0, 0, 0.1803922f);

    private const string SPRITE_PATH = "InGame1/sprite2";

    private Image   m_EffectImage;
    private Image   m_BuildImage;
    private Image   m_GPSPoint;
    public GPSBound m_GPSBound;

    //
    public bool         Contain = false;    // 검색하는 것에 포함될 것인지에 대한 플레그.
    public int          BeaconID = -1;    // 0일 경우 비콘 체크 및 퍼즐찾기에서 제외함.
    public int          Index = -1;
    public OpenState    State = OpenState.CLOSE;
    public string       BuildingName = "";

    private void Awake()
    {
        m_EffectImage = transform.GetChild(0).GetComponent<Image>();
        m_BuildImage = transform.GetChild(1).GetComponent<Image>();
        m_GPSPoint = transform.GetChild(2).GetComponent<Image>();
        m_BuildImage.gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isLocated(float latitude, float longtitude)
    {
        return m_GPSBound.contains(latitude, longtitude);
    }

    public void open()
    {
        m_BuildImage.gameObject.SetActive(true);
        m_EffectImage.gameObject.SetActive(false);
        //m_EffectImage.sprite = null;
        State = OpenState.OPEN;
    }

    public void close()
    {
        m_EffectImage.gameObject.SetActive(true);
        m_EffectImage.sprite = null;
        m_EffectImage.color = CLOSE_COLOR;
        m_BuildImage.gameObject.SetActive(false);
        State = OpenState.CLOSE;
    }

    public void find()
    {
        m_EffectImage.gameObject.SetActive(true);
        m_EffectImage.sprite = null;
        m_EffectImage.color = FINDED_COLOR;
        m_BuildImage.gameObject.SetActive(false);
        State = OpenState.FIND;
    }
}
