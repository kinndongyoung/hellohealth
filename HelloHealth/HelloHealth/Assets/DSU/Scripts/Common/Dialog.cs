using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    public enum BUTTONRESULT_STATE
    {
        NONE,
        OK,
        YES,
        NO,
    }

    public BUTTONRESULT_STATE ResultState = BUTTONRESULT_STATE.NONE;

    public IEnumerator show()
    {
        ResultState = BUTTONRESULT_STATE.NONE;
        gameObject.SetActive(true);
        while (ResultState == BUTTONRESULT_STATE.NONE)
        {
            yield return null;
        }
    }

    public void hide()
    {
        gameObject.SetActive(false);
    }

    public void onClickOK()
    {
        hide();
        ResultState = BUTTONRESULT_STATE.OK;
    }

    public void onClickYes()
    {
        hide();
        ResultState = BUTTONRESULT_STATE.YES;
    }

    public void onClickNo()
    {
        hide();
        ResultState = BUTTONRESULT_STATE.NO;
    }
}
