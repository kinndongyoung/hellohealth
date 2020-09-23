using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameAccessor : Accessor
{
    public List<PuzzleParts>    PuzzleList;
    public UserController       User;
    public Text                 InfoText;
    public Text                 PointText;
    public Dialog           ResetDialog;
    public Dialog           StateDialog;
    public Dialog           GameClearDialog;
    public Dialog           FindAreaDialog;

}
