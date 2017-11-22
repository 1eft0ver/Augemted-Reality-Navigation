using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPS : MonoBehaviour {

    public static GPS Instance { set; get; }

    public float latitude; // 緯度
    public float longitude; // 經度
    public double timestamp; // 更新時間戳

    public float compass; // 跟正北方的角度 (順時針增加)

    //public Dictionary<string, labelNode> labelList;

    private void Start()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
        StartCoroutine(StartLocationService());
    }

    private IEnumerator StartLocationService()
    {
        // Specifies whether location service is enabled in user settings.
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("User has not enabled GPS");
            yield break;
        }

        // Compass
        Input.compass.enabled = true;

        // Starts location service updates. Last location coordinates could be.
        Input.location.Start(1, 0.1f);
        int maxWait = 20;

        while(Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if(maxWait <= 0)
        {
            Debug.Log("Time out");
            yield break;
        }

        if(Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determin device location");
            yield break;
        }
    }

    private void Update()
    {
        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude;
        timestamp = Input.location.lastData.timestamp;

        compass = Input.compass.trueHeading;
    }
}