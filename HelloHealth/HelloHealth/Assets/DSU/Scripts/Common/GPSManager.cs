using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.Android;
using Glitch;

public class GPSManager : MonoSingleton<GPSManager>
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
        awake(this);
    }

    // public ViewHandler v;
    public Text debugText;
    public Text texLatitude;
    public Text texLongtitude;
    public Text texCount;
    public string sState;

    LocationInfo myGPSLocation;
    float fiveSecondCounter = 0.0f;

    public string LocationName;

    double MyLatitude, MyLongtitude;
    public float Latitude;
    public float Longtitude;
    DistUnit unit;

    /**
    * Latitude - 경도, Longtitude - 위도 
    */
    public double TargetLatitude, TargetLongtitude; // 37.507839, 127.039864

    IEnumerator Start()
    {
        Debug.Log("Start");

        //
        if(debugText != null)
            debugText.text += "Starting the GPS Script\n";
        sState = "Starting the GPS\n";
#if UNITY_ANROID
         Input.compass.enabled = true;
#endif
        yield return StartCoroutine(checkPermission());
        yield return InitializeGPSServices();
    }

    IEnumerator checkPermission()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.CoarseLocation))
        {
            Permission.RequestUserPermission(Permission.CoarseLocation);
            while (!Permission.HasUserAuthorizedPermission(Permission.CoarseLocation))
            {
                yield return null;
            }

        }
#endif
    }

    IEnumerator InitializeGPSServices()
    {
        Debug.Log("InitializeGPSServices");

        // First, check if user has location service enabled
        //if (!Input.location.isEnabledByUser)
        //{
        //    if(debugText != null)
        //        debugText.text += "GPS disabled by user\n";
        //    sState = "GPS disabled by user\n";
        //    yield break;
        //}

        // Start service before querying location
        //Input.location.Start(0.1f, 0.1f);
        Input.location.Start();

        // Wait until service initializes
        //int maxWait = 20;
        //while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        //{
        //    yield return new WaitForSeconds(1);
        //    maxWait--;
        //}

        //// Service didn't initialize in 20 seconds
        //if (maxWait < 1)
        //{
        //    if(debugText != null)
        //        debugText.text += "Timed out\n";
        //    sState = "Timed out\n";
        //    yield break;
        //}
        yield return null;
    }

    void Update()
    {
        fiveSecondCounter += Time.deltaTime;
        if (fiveSecondCounter > 0.5)
        {
            UpdateGPS();
            fiveSecondCounter = 0.0f;
        }
    }

    int updateCnt = 0;

    void UpdateGPS()
    {
        // Connection has failed
        //if (Input.location.status == LocationServiceStatus.Failed)
        //{
        //    if (debugText != null)
        //        debugText.text += "Unable to determine device location\n";
        //    sState = "Unable to determine device location\n";
        //    Input.location.Stop();
        //    StartCoroutine(Start());
        //}
        //else
        //{
        //    updateCnt++;
        //    if(texCount != null)
        //        texCount.text = updateCnt.ToString();
        //    updateGPSData();
        //    // debugText.text = "update count : " + updateCnt + "\n" + getUpdatedGPSstring ();
        //}

        if (Input.location.isEnabledByUser)
        {
            updateCnt++;
            if(texCount != null)
                texCount.text = Input.location.status + "__" +updateCnt.ToString();
            updateGPSData();
        }
    }

    void updateGPSData()
    {
        myGPSLocation = Input.location.lastData;
        MyLongtitude = Math.Round(myGPSLocation.longitude, 6);
        MyLatitude = Math.Round(myGPSLocation.latitude, 6);
        Latitude = (float)MyLatitude;
        Longtitude = (float)MyLongtitude;
        if(texLongtitude != null)
            texLongtitude.text = MyLongtitude.ToString();
        if(texLatitude != null)
            texLatitude.text = MyLatitude.ToString();

    }

    string getUpdatedGPSstring()
    {
        myGPSLocation = Input.location.lastData;

        MyLongtitude = Math.Round(myGPSLocation.longitude, 6);
        MyLatitude = Math.Round(myGPSLocation.latitude, 6);
        if (texLongtitude != null)
            texLongtitude.text = MyLongtitude.ToString();
        if (texLatitude != null)
            texLatitude.text = MyLatitude.ToString();

        double DistanceToMeter;
        string storeRange;

        //두 점간의 거리
        DistanceToMeter = distance(MyLatitude, MyLongtitude, TargetLatitude, TargetLongtitude, DistUnit.meter);
        //DistanceToMeter = distance (37.507775, 127.039675, 37.507660, 127.039530, "meter"); // 20미터 이내 거리체크

        if (DistanceToMeter < 20)
        {// 건물의 높낮이 등 환경적인 요소로 인해 오차가 발생 할 수 있음.
            storeRange = "근처매장 O";

            // if (!v.flowAHandler.NotiOpenChk)
            //     v.flowAHandler.OpenNoti ();
        }
        else
        {
            storeRange = "근처매장 X";
        }

        return "\n현재위치 :\n" +
                            "경도 - " + Math.Round(MyLatitude, 6) + "\n" +
                            "위도 - " + Math.Round(MyLongtitude, 6) +

            "\n\n" + "목표위치 : " + LocationName + "\n" +
                            "경도 - " + TargetLatitude + "\n" +
                            "위도 - " + TargetLongtitude +

            "\n\n목표와의거리 : 약 " + DistanceToMeter + "M" + "\n" +
                            "-------------------------------\n\n" +

                            storeRange;
    }

    /**
     * 두 지점간의 거리 계산
     *
     * @param lat1 지점 1 위도
     * @param lon1 지점 1 경도
     * @param lat2 지점 2 위도
     * @param lon2 지점 2 경도
     * @param unit 거리 표출단위
     * @return
     */
    static double distance(double lat1, double lon1, double lat2, double lon2, DistUnit unit)
    {

        double theta = lon1 - lon2;
        double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));

        dist = Math.Acos(dist);
        dist = rad2deg(dist);
        dist = dist * 60 * 1.1515;

        if (unit == DistUnit.kilometer)
        {
            dist = dist * 1.609344;
        }
        else if (unit == DistUnit.meter)
        {
            dist = dist * 1609.344;
        }

        return (dist);
    }

    // This function converts decimal degrees to radians
    static double deg2rad(double deg)
    {
        return (deg * Math.PI / 180.0);
    }

    // This function converts radians to decimal degrees
    static double rad2deg(double rad)
    {
        return (rad * 180 / Math.PI);
    }
}

enum DistUnit
{
    kilometer,
    meter
}
