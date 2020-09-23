using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LitJson;

public class InGameSceneController : MonoBehaviour
{
    private BeaconManager m_BeaconManager;
    private GPSManager m_GPSManager;
    private PuzzlePattern m_Pattern;

    private StageData m_Data;
    public InGameAccessor m_Accessor;
    // 해당 스테이지에 찾아야하는 Index를 보관.
    public List<int> m_PartsIndexList = new List<int>();
    int m_nStageNumber = 1;
    float m_SecondCounter = 0.0f;
    bool m_bPlaying = false;

    private void Awake()
    {
        m_BeaconManager = BeaconManager.Instance;
        m_GPSManager = GPSManager.Instance;
        m_Pattern = new PuzzlePattern();

        if ((Input.deviceOrientation == DeviceOrientation.LandscapeLeft) && (Screen.orientation != ScreenOrientation.LandscapeLeft))
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }

        if ((Input.deviceOrientation == DeviceOrientation.LandscapeRight) && (Screen.orientation != ScreenOrientation.LandscapeRight))
        {
            Screen.orientation = ScreenOrientation.LandscapeRight;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_nStageNumber = PlayerPrefs.GetInt(GO.STAGE_INDEX, 1);
        bool bExist = initData();
        if (bExist == true)
        {
            reloadStage();
        }
        else
        {
            initStage();
        }
        //
        m_bPlaying = true;
    }

    /// <summary>
    /// 이미 데이터를 가지고 있었다면 true, 가지고 있지 않거나 초기화 했다면 false
    /// </summary>
    /// <returns><c>true</c>, if data was inited, <c>false</c> otherwise.</returns>
    bool initData()
    {
        var stageName = getStageName();
        var sData = PlayerPrefs.GetString(stageName);
        if (string.IsNullOrEmpty(sData))
        {
            m_Data = new StageData();
            return false;
        }
        else
        {
            m_Data = JsonMapper.ToObject<StageData>(sData);
            return true;
        }
    }

    void initStage()
    {
        //
        m_Data.initData();
        m_PartsIndexList = m_Pattern.getPatternByStage(m_Accessor.PuzzleList, m_nStageNumber);
        // 데이터 초기화.
        m_Data.initAreaOpens(m_PartsIndexList);
        //
        openParts();
    }

    void reloadStage()
    {
        for (int i = 0; i < m_Data.Areas.Count; i++)
        {
            Area area = m_Data.Areas[i];
            int index = area.Index;
            m_PartsIndexList.Add(index);
            if (area.Finded == false)
            {
                m_Accessor.PuzzleList[index].open();
            }
            else
            {
                m_Accessor.PuzzleList[index].find();
            }
        }
        // 포인트 표시 갱신.
        string fmt1 = "00";
        m_Accessor.PointText.text = m_Data.Point.ToString(fmt1) + " P";
        // 칼로리 표시 갱신.
        string fmt2 = "00.##";
        m_Accessor.InfoText.text = m_Data.Calorie.ToString(fmt2) + "kcal";
    }

    private void Update()
    {
        if (m_bPlaying == false)
            return;
        updateUserPosition();
        updateFindBeacon();
        m_Data.Time += Time.unscaledDeltaTime;
        //
        m_SecondCounter += Time.deltaTime;
        if (m_SecondCounter > 1.0f)
        {
            checkClearState();
            m_SecondCounter = 0.0f;
        }

    }

    /// <summary>
    /// 찾아야 하는 Puzzle을 Open
    /// 이미 찾았는데 것과 그외의 Puzzle은 Close
    /// </summary>
    private void openParts()
    {
        foreach (var index in m_PartsIndexList)
        {
            m_Accessor.PuzzleList[index].open();
        }
    }

    private void closeParts()
    {
        foreach (var index in m_PartsIndexList)
        {
            m_Accessor.PuzzleList[index].close();
        }
    }

    float lat = 0.0f;
    float log = 0.0f;
    private void updateUserPosition()
    {
        UserController user = m_Accessor.User;
        GPSManager manager = GPSManager.Instance;
#if !UNITY_EDITOR
        float latitude = manager.Latitude;
        float longtitude = manager.Longtitude;
#else
        float latitude = lat;
        float longtitude = log;
        //float latitude = 35.144789f;
        //float longtitude = 129.009605f;
        //float latitude = 35.144731f;
        //float longtitude = 129.007813f;
#endif
        PuzzleParts parts = null;
        foreach (var pp in m_Accessor.PuzzleList)
        {
            if (pp.isLocated(latitude, longtitude) == true)
            {
                parts = pp;
                break;
            }
        }
        if (parts == null)
        {
            user.invisible();
        }
        else
        {
            user.visible();
            user.setPosition(parts.transform.position);
            var calorie = user.calculateCalorie(parts.Index);
            addTotalCalorieToApplication(calorie);
            m_Data.Calorie += calorie;
            string fmt = "00.##";
            m_Accessor.InfoText.text = m_Data.Calorie.ToString(fmt) + "kcal";
        }
    }

    private void addTotalCalorieToApplication(float calorie)
    {
        float totalCalorie = PlayerPrefs.GetFloat(GO.TOTAL_CALORIE, 0.0f);
        totalCalorie += calorie;
        PlayerPrefs.SetFloat(GO.TOTAL_CALORIE, totalCalorie);
        PlayerPrefs.Save();
    }

    private void updateFindBeacon()
    {
        List<Beacon> beacons = m_BeaconManager.mybeacons;
        foreach (var beacon in beacons)
        {
            foreach (var index in m_PartsIndexList)
            {
                // 퍼즐이 열려 있고, 비콘의 번호가 같은 것이면 상태를 Find로 변경.
                var puzzle = m_Accessor.PuzzleList[index];
                if (puzzle.State == PuzzleParts.OpenState.OPEN &&
                    puzzle.BeaconID == beacon.minor)
                {
                    showFindAreaDialog(puzzle.BuildingName);
                    puzzle.find();
                    m_Data.setAreaFind(index);
                    m_Data.Point += 50;
                    string fmt = "00";
                    m_Accessor.PointText.text = m_Data.Point.ToString(fmt) + " P";
                    //
                    saveData();
                }
            }
        }

    }

    private void checkClearState()
    {
        int nFindCount = 0;
        foreach (var index in m_PartsIndexList)
        {
            var puzzle = m_Accessor.PuzzleList[index];
            if (puzzle.State == PuzzleParts.OpenState.FIND)
            {
                nFindCount++;
            }
        }
        if (m_PartsIndexList.Count == nFindCount)
        {
            m_bPlaying = false;
            showClearDialog();
        }
    }

    public void onClickBack()
    {
        saveData();
        SceneManager.LoadScene("StageSelection", LoadSceneMode.Single);
    }

    public void onClickReset()
    {
        StartCoroutine(cor_onClickReset());
    }

    private IEnumerator cor_onClickReset()
    {
        yield return StartCoroutine(m_Accessor.ResetDialog.show());
        if (m_Accessor.ResetDialog.ResultState == Dialog.BUTTONRESULT_STATE.YES)
        {
            closeParts();
            m_PartsIndexList.Clear();
            string sStageName = getStageName();
            PlayerPrefs.SetString(sStageName, "");
            PlayerPrefs.Save();
            initStage();
        }
    }

    private string getStageName()
    {
        return GO.STAGE_DATA + m_nStageNumber;
    }

    public void onClickTest1()
    {
        lat = 35.144789f;
        log = 129.009605f;
    }

    public void onClickTest2()
    {
        lat = 35.144731f;
        log = 129.007813f;
    }

    public void onClickTest3()
    {
        foreach (var index in m_PartsIndexList)
        {
            if (m_Accessor.PuzzleList[index].State == PuzzleParts.OpenState.OPEN)
            {
                var puzzle = m_Accessor.PuzzleList[index];
                showFindAreaDialog(puzzle.BuildingName);
                puzzle.find();
                m_Data.setAreaFind(index);
                m_Data.Point += 50;
                saveData();
                break;
            }
        }
    }

    public void onClickSetting()
    {
        StartCoroutine(m_Accessor.StateDialog.show());
    }

    private void showClearDialog()
    {
        m_Data.Clear = true;
        saveData();
        saveStageClearData();
        saveTotalPointData();
        StartCoroutine(cor_showClearDialog());
    }

    private IEnumerator cor_showClearDialog()
    {
        var dialog = (GameClearDialog)m_Accessor.GameClearDialog;
        dialog.setData(m_nStageNumber, m_Data.Time, m_Data.Calorie, m_Data.Point);
        yield return StartCoroutine(dialog.show());
        //
        // 데이터 저장 및 씬이동.
        SceneManager.LoadScene("StageSelection", LoadSceneMode.Single);
    }

    private void saveData()
    {
        string json = JsonMapper.ToJson(m_Data);
        string stageName = getStageName();
        PlayerPrefs.SetString(stageName, json);
        PlayerPrefs.Save();
    }

    private void saveStageClearData()
    {
        int nIndex = m_nStageNumber - 1;
        string json = PlayerPrefs.GetString(GO.CLEAR_INFO);
        if (string.IsNullOrEmpty(json))
        {
            json = GO.CLEAR_INFO_DEFAULT_DATA;
        }
        //
        JsonData data = JsonMapper.ToObject(json);
        data[nIndex] = true;
        string  json2 = JsonMapper.ToJson(data);
        PlayerPrefs.SetString(GO.CLEAR_INFO, json2);
        PlayerPrefs.Save();
    }

    private void saveTotalPointData()
    {
        int totlaPoint = PlayerPrefs.GetInt(GO.TOTAL_POINT, 0);
        totlaPoint += m_Data.Point;
        PlayerPrefs.SetInt(GO.TOTAL_POINT, totlaPoint);
        PlayerPrefs.Save();
    }

    private void showFindAreaDialog(string sAreaName)
    {
        var dialog = (FindAreaDialog)m_Accessor.FindAreaDialog;
        dialog.setData(sAreaName);
        StartCoroutine(dialog.show());
    }
}
