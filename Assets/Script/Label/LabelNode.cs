using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelNode
{
    public string labelName; // label 的名稱
    public float labelLatitude; // label 的緯度
    public float labelLongitude; // label 的經度
    public double labelDistance; // label 跟人的距離

    public Sprite labelSprite; // label 背景圖片
    public string labelContent; // label 的詳細資料
    public int labelStars; // label 的星級

    // 一開始讀地圖和設定地圖時使用
    public bool isNode; // 判斷是 tag 或 map

    // SetLabel 中使用
    public GameObject labelObject; // 指向 Scene 上的 Toggle

    // SetLabel 中使用
    public Toggle labelToggle; // 指向 Scene 上的 Toggle

    // 給 AR 選擇要顯示的 label
    public bool labelChoose; // True 顯示，False 隱藏
    public GameObject RightMenuListItem;
    public GameObject DestMenuListItem;
    // 紀錄可到達的點
    public Dictionary<string, LabelNode> arriveNodeList;

    public LabelNode(string labelName, float labelLatitude, float labelLongitude, int labelStars=0)
    {
        this.labelName = labelName;
        this.labelLatitude = labelLatitude;
        this.labelLongitude = labelLongitude;
        this.labelStars = labelStars;

        // 計算 label 跟人的距離
        updateDistance();

        isNode = false;
        labelSprite = null;
        labelContent = string.Empty;
        labelChoose = true;

        this.arriveNodeList = new Dictionary<string, LabelNode>();
    }

    /*public LabelNode(string labelName, float labelLatitude, float labelLongitude, double labelDistance)
    {
        this.labelName = labelName;
        this.labelLatitude = labelLatitude;
        this.labelLongitude = labelLongitude;
        this.labelDistance = labelDistance;

        isNode = false;
        labelSprite = null;
        labelContent = string.Empty;
        labelChoose = true;
    }*/

    public void updateDistance()
    {
        Calc(labelLatitude, labelLongitude, labelLatitude, labelLongitude);
    }

    // calculates distance between two sets of coordinates, taking into account the curvature of the earth.
    // 程式內會修改 labelDistance 的值
    private void Calc(float lat1, float lon1, float lat2, float lon2)
    {

        var R = 6378.137; // Radius of earth in KM
        var dLat = lat2 * Mathf.PI / 180 - lat1 * Mathf.PI / 180;
        var dLon = lon2 * Mathf.PI / 180 - lon1 * Mathf.PI / 180;
        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
            Mathf.Cos(lat1 * Mathf.PI / 180) * Mathf.Cos(lat2 * Mathf.PI / 180) *
            Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);
        var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        labelDistance = R * c;
        labelDistance = labelDistance * 1000f; // meters
                                               //set the distance text on the canvas
        //convert distance from double to float
        //float distanceFloat = (float)distance;
        //set the target position of the ufo, this is where we lerp to in the update function
        //targetPosition = originalPosition - new Vector3(0, 0, distanceFloat * 12);
        //distance was multiplied by 12 so I didn't have to walk that far to get the UFO to show up closer
    }
}
