using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlreadyClearDialog : Dialog
{
    public Text     MessageText;

    public void setData(int nStage)
    {
        MessageText.text = string.Format("이미 스테이지{0} 을/를 클리어 했습니다.\n다시 초기화 하여 플레이 하시겠습니까?", nStage);
    }
}
