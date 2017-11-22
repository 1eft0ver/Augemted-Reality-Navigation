using System;
using UnityEngine;
using UnityEngine.UI;

public class GoogleMapTool
{

}

public class GoogleMapLocation
{
    public float latitude;
    public float longitude;

    public GoogleMapLocation(float latitude, float longitude)
    {
        this.latitude = latitude;
        this.longitude = longitude;
    }
}

public class GoogleMapMarker
{
    // 標記大小 (選擇性) {tiny, mid, small}
    public string size;
    // 標記顏色 {black, brown, green, purple, yellow, blue, gray, orange, red, white}
    public string color;
    // 標記標籤 {A-Z, 0-9} 單一字元
    public string label;
    // 標記位置
    public GoogleMapLocation location;

    public GoogleMapMarker(string size, string color, string label, GoogleMapLocation location)
    {
        this.size = size;
        this.color = color;
        this.label = label;
        this.location = location;
    }
}

public class GoogleMapPath
{
}
