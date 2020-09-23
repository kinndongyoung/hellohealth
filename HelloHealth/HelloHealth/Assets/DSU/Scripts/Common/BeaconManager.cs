using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Glitch;
using UnityEngine.UI;
using System;
using UnityEngine.Android;

public class BeaconManager : MonoSingleton<BeaconManager>
{
    public List<Beacon> mybeacons = new List<Beacon>();
    public List<Beacon> ScannedBeaconList = new List<Beacon>();
	public Text m_Text;

    private void Awake() {
        DontDestroyOnLoad(this);
        awake(this);
        if (GetComponent<BluetoothState>() == null)
            gameObject.AddComponent<BluetoothState>();
        if (GetComponent<iBeaconReceiver>() == null)
            gameObject.AddComponent<iBeaconReceiver>();
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                yield return null;
            }
        }

        BluetoothState.Init();
		StartCoroutine(StartScan());
    }

    IEnumerator StartScan()
    {
        yield return new WaitForSeconds(0.5f);
		iBeaconReceiver.BeaconRangeChangedEvent += OnBeaconRangeChanged;
		iBeaconReceiver.regions = new iBeaconRegion[] { new iBeaconRegion("co.kr.wildmagic.beacon", new Beacon()) };
		iBeaconReceiver.Scan();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBeaconRangeChanged(Beacon[] beacons)
    { //
        //ScannedBeaconList.Clear();
        //ScannedBeaconList = new List<Beacon>(beacons);


        foreach (Beacon b in beacons)
        {
            var index = mybeacons.IndexOf(b);
            if (index == -1)
            {
                mybeacons.Add(b);
            }
            else
            {
                mybeacons[index] = b;
            }
        }
        for (int i = mybeacons.Count - 1; i >= 0; --i)
        {
            if (mybeacons[i].lastSeen.AddSeconds(10) < DateTime.Now)
            {
                mybeacons.RemoveAt(i);
            }
        }


        if (m_Text == null)
            return;
        m_Text.text = "";
		foreach(Beacon b in beacons)
		{
            m_Text.text += b.minor.ToString();
            m_Text.text += " : ";
		}
    }
}
