using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameClearDialog : Dialog
{
    public Text     TitleText;
    public Text     TimeText;
    public Text     CalorieText;
    public Text     PointText;

    public void setData(int nStageNumber, float fPlayTime, float fCalorie, int nPoint) {
        //yield return StartCoroutine(base.show());
        TitleText.text = string.Format("스테이지 {0} 클리어!", nStageNumber);
        TimeText.text = "시간 : " + ToTime(fPlayTime);
        CalorieText.text = string.Format("소모칼로리 : {0}kcal", fCalorie.ToString(".##"));
        PointText.text = string.Format("획득 포인트 : {0}Point", nPoint);
    }

    private string ToTime(float seconds)
    {
        var span = TimeSpan.FromSeconds(seconds);
        return string.Format("{0}:{1:00}:{2:00}",
            (int)span.TotalHours, span.Minutes, span.Seconds);
    }
}
