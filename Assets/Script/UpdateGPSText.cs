using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateGPSText : MonoBehaviour {

    public Text coordinates;

    public Text CompassText;

    private float destLatitude;
    private float destLongitude;

    private void Update()
    {
        if(coordinates != null)
            coordinates.text = "Lat: " + GPS.Instance.latitude.ToString("0.00000000") + "   Long: " + GPS.Instance.longitude.ToString("0.00000000") + "   Time: " + GPS.Instance.timestamp.ToString();

        if(CompassText != null)
            CompassText.text = "跟正北方的角度：" + GPS.Instance.compass.ToString();
    }
}
