using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindAreaDialog : Dialog
{
    public Text     AreaNameText;


    public void setData(string sAreaName)
    {
        AreaNameText.text = "[ " + sAreaName + " ]";
    }
}
