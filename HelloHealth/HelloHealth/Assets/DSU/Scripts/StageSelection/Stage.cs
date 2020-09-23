using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{

    private Image   m_EffectImage;
    private Image   m_LockImage;

    public int StageNumber = 0;

    private void Awake()
    {
        m_EffectImage = transform.GetChild(1).GetComponent<Image>();
        m_LockImage = transform.GetChild(2).GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void close(){
        m_EffectImage.gameObject.SetActive(true);
        m_LockImage.gameObject.SetActive(true);
    }


    public void open()
    {
        m_EffectImage.gameObject.SetActive(false);
        m_LockImage.gameObject.SetActive(false);
    }
}
