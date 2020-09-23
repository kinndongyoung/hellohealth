using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneController : MonoBehaviour
{
    public MainAccessor m_Accessor;

    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            onClickQuit();
        }
    }

    public void onClickGameSelection()
    {
        SceneManager.LoadScene("GameSelection", LoadSceneMode.Single);
    }

    public void onClickInputInfo()
    {
        //SceneManager.LoadScene();
        StartCoroutine(m_Accessor.PrepareDialog.show());
    }

    public void onClickViewCharacter()
    {
        SceneManager.LoadScene("ViewCharacter", LoadSceneMode.Single);
        //StartCoroutine(m_Accessor.PrepareDialog.show());
    }

    public void onClickTargetSettting()
    {
        //SceneManager.LoadScene();
        StartCoroutine(m_Accessor.PrepareDialog.show());
    }

    public void onClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void onClickSetting()
    {
        StartCoroutine(m_Accessor.StateDialog.show());
    }
}
