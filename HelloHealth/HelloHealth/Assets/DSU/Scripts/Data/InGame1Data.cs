using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area
{
    public int Index { get; set; }
    public bool Finded { get; set; }
}

public class StageData
{
    public bool Clear { get; set; }
    public int Point { get; set; }
    public float Calorie { get; set; }
    public float Time { get; set; }
    public List<Area> Areas { get; set; }

    public StageData()
    {
        Areas = new List<Area>();
        initData();
    }

    public void initData()
    {
        Clear = false;
        Calorie = 0.0f;
        Point = 0;
        Time = 0.0f;
        Areas.Clear();
    }

    public void initAreaOpens(List<int> indexList)
    {
        foreach (var index in indexList)
        {
            Area area = new Area();
            area.Index = index;
            area.Finded = false;
            Areas.Add(area);
        }
    }

    public void setAreaFind(int nIndex)
    {
        foreach (var area in Areas)
        {
            if( area.Index == nIndex )
            {
                area.Finded = true;
            }
        }
    }
}
