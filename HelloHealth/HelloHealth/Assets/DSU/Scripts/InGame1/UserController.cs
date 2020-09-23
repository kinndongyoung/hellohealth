using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : MonoBehaviour
{

    public int PartsIndex = -1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setPosition(Vector3 v3Position)
    {
        transform.position = v3Position;
    }

    public void invisible()
    {
        gameObject.SetActive(false);
    }

    public void visible()
    {
        if (gameObject.activeSelf == false)
            gameObject.SetActive(true);
    }

    public float calculateCalorie(int nPartsIndex)
    {
        // 시작시 변동이 클 수 있어 계산을 하지는 않음.
        if(PartsIndex == -1)
        {
            PartsIndex = nPartsIndex;
            return 0.0f;
        }
        else if(PartsIndex == nPartsIndex)
        {
            return 0.0f;
        }
        else
        {   // MET * 3.5 * kg * 분 * 5 / 1000.
            int oldRow = PartsIndex / 5;
            int oldColum = PartsIndex % 5;
            int newRow = nPartsIndex / 5;
            int newColum = nPartsIndex % 5;
            //
            int row = Mathf.Abs(oldRow - newRow);
            int colum =  Mathf.Abs(oldColum - newColum);
            float MET = row * 7.5f + colum * 3.8f;
            float fCalorie = (MET * 3.5f * 70 * 5 * 5 / 1000);
            PartsIndex = nPartsIndex;
            return fCalorie;
        }
    }
}
