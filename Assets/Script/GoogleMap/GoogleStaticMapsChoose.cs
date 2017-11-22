using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoogleStaticMapsChoose : MonoBehaviour {

    List<GoogleMapMarker> markers;

    GoogleMapLocation center;
    //string zoom = "18";
    string size = "640x640";
    string scale = "2";
    string maptype = "roadmap";

    private Dictionary<string, LabelNode> labelList;

    private char[] labelMarker; // labelMarker 給 Map 用
    private int labelMarkerCounter; // labelMarkerCounter 計算 labelMarker

    private float moveRange; // 地圖可滑動的範圍

    private void Start()
    {
        labelList = LabelMain.Instance.labelList;

        // 建立 labelMarker
        labelMarkerCounter = 0;
        labelMarker = new char[] {
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
        'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
        'U', 'V', 'W', 'X', 'Y', 'Z', '1', '2', '3', '4',
        '5', '6', '7', '8', '9', '0'};

        markers = new List<GoogleMapMarker>();

        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            GoogleMapMarker tempMarker;
            if (!labelTemp.Value.isNode)
            {
                tempMarker = new GoogleMapMarker("mid", "red", labelMarker[labelMarkerCounter].ToString(), new GoogleMapLocation(labelTemp.Value.labelLatitude, labelTemp.Value.labelLongitude));
                markers.Add(tempMarker);
                labelMarkerCounter++;
            }
        }

        // 取得放置地圖的 Background 的 Canvas 寬度
        GameObject mapCanvas = GameObject.Find("Canvas");
        float mapCanvasWidth = mapCanvas.GetComponent<RectTransform>().rect.width;

        // 取得放置地圖的 Background 寬度
        float mapWidth = GetComponent<RectTransform>().rect.width;

        // 計算地圖可滑動的範圍
        moveRange = (mapWidth - mapCanvasWidth) / 2;

        StartCoroutine(GetGoogleMap());
    }

    private void Update()
    {
        // 偵測手機返回鍵
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 從 Map Scene (Choose) 跳到 ChooseLabel Scene
            SceneManager.LoadScene("ChooseLabel");
        }

        // 取得地圖當下位置
        Vector3 maplocalPosition = gameObject.GetComponent<RectTransform>().localPosition;

        // 手指滑動 (僅限左右)
        if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Moved)
            {
                // 水平移動距離
                float moveDistance = Input.touches[0].deltaPosition.x;

                // 手指移動，範圍界定在 +-moveRange 之間
                if (maplocalPosition.x + moveDistance >= moveRange)
                    gameObject.GetComponent<RectTransform>().localPosition = new Vector3(moveRange, maplocalPosition.y, maplocalPosition.z);
                else if (maplocalPosition.x + moveDistance <= -moveRange)
                    gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-moveRange, maplocalPosition.y, maplocalPosition.z);
                else
                    gameObject.GetComponent<RectTransform>().localPosition = new Vector3(maplocalPosition.x + moveDistance, maplocalPosition.y, maplocalPosition.z);
            }
        }
    }

    IEnumerator GetGoogleMap()
    {
        string googleMapUrl = "https://maps.googleapis.com/maps/api/staticmap?";
        string parameters = string.Empty;

        //parameters += ("zoom="+zoom);
        parameters += ("size=" + size);
        parameters += ("&scale=" + scale);
        parameters += ("&maptype=" + maptype);

        foreach(GoogleMapMarker temp in markers)
        {
            parameters += "&markers=" + string.Format("size:{0}|color:{1}|label:{2}", temp.size, temp.color, temp.label);
            parameters += "|" + WWW.UnEscapeURL(string.Format("{0},{1}", temp.location.latitude, temp.location.longitude));
        }

        WWW url = new WWW(googleMapUrl + parameters);

        yield return url;
        //GetComponent<CanvasRenderer>().SetTexture(url.texture);

        GetComponent<RawImage>().texture = url.texture;

        Debug.Log(googleMapUrl + parameters);
    }
}