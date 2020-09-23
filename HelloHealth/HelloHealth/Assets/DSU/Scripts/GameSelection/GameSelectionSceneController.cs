using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSelectionSceneController : MonoBehaviour
{
    public GameSelectionAccessor m_Accessor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            onClickBack();
        }   
    }

    public void onClickMiniGame1()
    {
        SceneManager.LoadScene("StageSelection", LoadSceneMode.Single);
    }

    public void onClickPrepare()
    {
        StartCoroutine(m_Accessor.PrepareDialog.show());
    }

    public void onClickBack()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
