using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateDialog : Dialog
{
    private BeaconManager   m_BeaconManager;
    private GPSManager      m_GPSManager;

    public Text GPSText;
    public Text BeaconText;

    void Start()
    {
        m_BeaconManager = BeaconManager.Instance;
        m_GPSManager = GPSManager.Instance;
    }

    // Start is called before the first frame update
    private void Update()
    {
        updateBeaconInfo();
        updateGPSInfo();
    }

    private void updateBeaconInfo()
    {
        List<Beacon> beacons = m_BeaconManager.mybeacons;
        string info = BluetoothState.GetBluetoothLEStatus().ToString();
        foreach(var beacon in beacons)
        {
            info += beacon.minor.ToString() + "\n";
        }
        BeaconText.text = info;
    }

    private void updateGPSInfo()
    {
        string info = m_GPSManager.sState + "\n" + Input.location.status + "\n";
        info += m_GPSManager.Latitude + "\n";
        info += m_GPSManager.Longtitude + "\n";
        GPSText.text = info;
    }

}
