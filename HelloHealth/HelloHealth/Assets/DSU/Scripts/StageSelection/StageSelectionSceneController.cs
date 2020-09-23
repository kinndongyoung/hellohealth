using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectionSceneController : MonoBehaviour
{
    public StageSelectionAccessor m_Accessor;
    private List<bool> m_ClearList = new List<bool>();

    // Start is called before the first frame update
    void Start()
    {
        initStageButtons();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            onClickBack();
        }
    }

    void initStageButtons()
    {
        string      json = PlayerPrefs.GetString(GO.CLEAR_INFO);
        if (string.IsNullOrEmpty(json))
        {
            PlayerPrefs.SetString(GO.CLEAR_INFO, GO.CLEAR_INFO_DEFAULT_DATA);
            PlayerPrefs.Save();
            m_Accessor.StageList[0].open();
            m_Accessor.StageList[1].close();
            m_Accessor.StageList[2].close();
        }
        else
        {
            JsonData data = JsonMapper.ToObject(json);
            m_ClearList.Add(bool.Parse(data[0].ToString()));
            m_ClearList.Add(bool.Parse(data[1].ToString()));
            m_ClearList.Add(bool.Parse(data[2].ToString()));
            //
            m_Accessor.StageList[0].open();

            if (m_ClearList[0])
            {
                m_Accessor.StageList[1].open();
            }
            else
            {
                m_Accessor.StageList[1].close();
            }
            if (m_ClearList[1])
            {
                m_Accessor.StageList[2].open();
            }
            else
            {
                m_Accessor.StageList[2].close();
            }
        }
    }

    public void onClickStage(Stage stage)
    {
        bool bClear = isClearStage(stage.StageNumber);
        // 이미 Clear된 스테이지일 경우.
        if(bClear == true)
        {
            StartCoroutine(cor_showAlreadyClearDialog(stage.StageNumber));
        }
        else
        {
            bool bPrevClear = false;
            if (stage.StageNumber == 1)
                bPrevClear = true;
            else
                bPrevClear = m_ClearList[stage.StageNumber - 2];
            if (bPrevClear == true)
            {
                PlayerPrefs.SetInt(GO.STAGE_INDEX, stage.StageNumber);
                PlayerPrefs.Save();
                SceneManager.LoadScene("InGame1", LoadSceneMode.Single);
            }
            else
            {
                showPrepareDailog();
            }
        }
    }

    private IEnumerator cor_showAlreadyClearDialog(int nStageNumber)
    {
        var dialog = (AlreadyClearDialog)m_Accessor.AlreadyClearDialog;
        dialog.setData(nStageNumber);
        yield return StartCoroutine(dialog.show());
        if (dialog.ResultState == Dialog.BUTTONRESULT_STATE.YES)
        {
            m_ClearList[nStageNumber - 1] = false;
            saveClearInfo(nStageNumber);
        }
    }

    private bool isClearStage(int nStageNumber)
    {
        return m_ClearList[nStageNumber - 1];
    }

    private void saveClearInfo(int nStageNumber)
    {
        string json = JsonMapper.ToJson(m_ClearList);
        PlayerPrefs.SetString(GO.CLEAR_INFO, json);
        //
        PlayerPrefs.DeleteKey(GO.STAGE_DATA + nStageNumber);
        //
        PlayerPrefs.Save();

    }

    private void showPrepareDailog()
    {
        StartCoroutine(m_Accessor.PrepareDialog.show());
    }

    public void onClickGameRule()
    {
        StartCoroutine(m_Accessor.GameRuleDialog.show());
    }

    public void onClickBack()
    {
        SceneManager.LoadScene("GameSelection", LoadSceneMode.Single);
    }
}
