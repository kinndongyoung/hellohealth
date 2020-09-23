using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewCharacterSceneController : MonoBehaviour
{
    public ViewCharacterAccessor m_Accessor;

    // Start is called before the first frame update
    void Start()
    {
        initViewData();
    }

    private void initViewData()
    {
        float fCalorie = PlayerPrefs.GetFloat(GO.TOTAL_CALORIE, 0.0f);
        int nPoint = PlayerPrefs.GetInt(GO.TOTAL_POINT, 0);
        m_Accessor.TotalCalorieText.text = string.Format("{0} kcal", fCalorie.ToString("0.##"));
        m_Accessor.TotalPointText.text = string.Format("{0} P", nPoint);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            onClickBack();
        }
    }

    public void onClickBack()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
