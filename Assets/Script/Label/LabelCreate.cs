using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LabelCreate : MonoBehaviour {
    
    private readonly float maxDistance = 1000f;

    private float latitude; // 人所在的緯度
    private float longitude; // 人所在的經度
    //private float compass; // 跟北方的角度

    private float vectorDistance; // label 向量計算用的距離
    private float labelLatitude; // label 的緯度
    private float labelLongitude; // label 的經度
    private double labelDistance; // label 跟人的距離

    // label 的 X Y Z 位置
    private float labelPositionX, labelPositionY, labelPositionZ;

    private GameObject label; // RawImage + Text
    private Text labelNameText; // label 底下的 Name
    private Text labelDistanceText; // label 底下的 Distance
    private Image labelImage; // label 底下的 Image
    private Image labelDistanceFrame; // label 底下的 Frame
    private GameObject labelParent; // LabelCanvas
    private Dictionary<string, LabelNode> labelList;

    public GameObject labelPrefab; // label 模板

    // Label 的內容
    GameObject labelContent; // 內容的 Panel
    Text labelContentText; // 內容文字
    /*Image labelContentBackground; // 背景顏色
    GameObject navigationButton; // 導航的按鈕
    GameObject detailContentButton; // 詳細內容的按鈕*/
    GameObject labelDetailContent; // 詳細內容的 Panel
    public Text LabelText; // Debug Label
    //public Text DistanceText; // Debug Distance
    //public Text labelListText; // Debug labelList

    // 選擇地圖用
    GameObject mapNameButton;
    GameObject deleteNameButton;
    public GameObject mapNameButtonPrefab; // MapNameButton 模板
    public GameObject mapNameButtonParent; // Canvas
    public GameObject deleteNameButtonPrefab; // MapNameButton 模板
    public GameObject deleteNameButtonParent; // Canvas
    private List<string> CreatedMapButtons;
    private List<string> CreatedDeleteButtons;
    //public Text LabelName; // 顯示地圖名稱，Debug 用
    public void DeleteButtonGenerate() {
        // 建立地圖選擇
        foreach(string temp in CreatedMapButtons) {
            Destroy(GameObject.Find(temp));
        }
        CreatedMapButtons.Clear();

        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
        DirectoryInfo[] info = dir.GetDirectories();
        GameObject parent = GameObject.Find("DeleteMapList");
        RectTransform parentRt = (RectTransform)parent.transform;
        float deleteNameButtonHeight = deleteNameButtonPrefab.GetComponent<RectTransform>().rect.height;
        float deleteNameButtonY = parentRt.rect.height / 2 - deleteNameButtonHeight;
        foreach (DirectoryInfo dirinfo in info)
        {
            if (dirinfo.Name != "Unity")
            {
                deleteNameButton = Instantiate(deleteNameButtonPrefab, deleteNameButtonParent.transform);
                deleteNameButton.name = "delButton_" + dirinfo.Name;
                CreatedDeleteButtons.Add(deleteNameButton.name);
                //mapNameButton.GetComponent<RectTransform>().sizeDelta = new Vector2 (1086, 175);
                Sprite image = Resources.Load<Sprite>("UI/LeftMenu/underline_left");
                deleteNameButton.GetComponent<Image>().sprite = image;
                deleteNameButton.transform.SetParent(parent.transform);
                deleteNameButton.transform.Find("Text").GetComponent<Text>().text = dirinfo.Name;
                //mapNameButton.transform.Find("Text").GetComponent<Text>().transform.localPosition
                //mapNameButton.transform.localPosition = new Vector3(mapNameButton.transform.localPosition.x, mapNameButtonY, mapNameButton.transform.localPosition.z);
                deleteNameButton.transform.localPosition = new Vector3(-85, deleteNameButtonY, 0);
                deleteNameButtonY -= deleteNameButtonHeight;
            }
        }
    }
    public void mapButtonGenerate() {
        // 建立地圖選擇
        foreach(string temp in CreatedMapButtons) {
            Destroy(GameObject.Find(temp));
        }
        CreatedMapButtons.Clear();

        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
        DirectoryInfo[] info = dir.GetDirectories();
        GameObject parent = GameObject.Find("SelectMapList");
        RectTransform parentRt = (RectTransform)parent.transform;
        float mapNameButtonHeight = mapNameButtonPrefab.GetComponent<RectTransform>().rect.height;
        float mapNameButtonY = parentRt.rect.height / 2 - mapNameButtonHeight;
        foreach (DirectoryInfo dirinfo in info)
        {
            if (dirinfo.Name != "Unity")
            {
                mapNameButton = Instantiate(mapNameButtonPrefab, mapNameButtonParent.transform);
                mapNameButton.name = dirinfo.Name;
                CreatedMapButtons.Add(mapNameButton.name);
                //mapNameButton.GetComponent<RectTransform>().sizeDelta = new Vector2 (1086, 175);
                Sprite image = Resources.Load<Sprite>("UI/LeftMenu/underline_left");
                mapNameButton.GetComponent<Image>().sprite = image;
                mapNameButton.transform.SetParent(parent.transform);
                mapNameButton.transform.Find("Text").GetComponent<Text>().text = dirinfo.Name;
                //mapNameButton.transform.Find("Text").GetComponent<Text>().transform.localPosition
                //mapNameButton.transform.localPosition = new Vector3(mapNameButton.transform.localPosition.x, mapNameButtonY, mapNameButton.transform.localPosition.z);
                mapNameButton.transform.localPosition = new Vector3(-85, mapNameButtonY, 0);
                mapNameButtonY -= mapNameButtonHeight;
            }
        }
    }
    private void Start()
    {
        //labelList = new Dictionary<string, LabelNode>();
        CreatedMapButtons = new List<string>();
        // 取得主要的 labelList
        labelList = LabelMain.Instance.labelList;

        // 取得 LabelCanvas
        labelParent = GameObject.Find("LabelCanvas");

        // 初始化所有 LabelContent 有關的原件
        labelContent = GameObject.Find("LabelContent");
        labelContentText = labelContent.transform.Find("LabelContentText").GetComponent<Text>();
        /*labelContentBackground = labelContent.transform.Find("LabelContentBackground").GetComponent<Image>();
        navigationButton = labelContent.transform.Find("NavigationButton").gameObject;
        detailContentButton = labelContent.transform.Find("DetailContentButton").gameObject;*/
        labelDetailContent = GameObject.Find("LabelDetailContent");

        // 讓 LabelContent 消失
        labelContent.transform.localPosition = new Vector3(0, -2500, 0);
/* 
        // 建立地圖選擇
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
        DirectoryInfo[] info = dir.GetDirectories();
        GameObject parent = GameObject.Find("SelectMapList");
        RectTransform parentRt = (RectTransform)parent.transform;
        float mapNameButtonHeight = mapNameButtonPrefab.GetComponent<RectTransform>().rect.height;
        float mapNameButtonY = parentRt.rect.height / 2 - mapNameButtonHeight;
        foreach (DirectoryInfo dirinfo in info)
        {
            if (dirinfo.Name != "Unity")
            {
                mapNameButton = Instantiate(mapNameButtonPrefab, mapNameButtonParent.transform);
                mapNameButton.name = dirinfo.Name;
                //mapNameButton.GetComponent<RectTransform>().sizeDelta = new Vector2 (1086, 175);
                Sprite image = Resources.Load<Sprite>("UI/LeftMenu/underline_left");
                mapNameButton.GetComponent<Image>().sprite = image;
                mapNameButton.transform.SetParent(parent.transform);
                mapNameButton.transform.Find("Text").GetComponent<Text>().text = dirinfo.Name;
                //mapNameButton.transform.Find("Text").GetComponent<Text>().transform.localPosition
                //mapNameButton.transform.localPosition = new Vector3(mapNameButton.transform.localPosition.x, mapNameButtonY, mapNameButton.transform.localPosition.z);
                mapNameButton.transform.localPosition = new Vector3(-85, mapNameButtonY, 0);
                mapNameButtonY -= mapNameButtonHeight;
            }
        }
*/
        // 顯示地圖名稱
        //LabelName.text = LabelMain.Instance.selectFileName;

        createLabel();
    }

    private void Update()
    {   
        // 判斷地圖是否更新
        if(LabelMain.Instance.selectFileNameChange)
        {
            LabelMain.Instance.selectFileNameChange = false;
            //LabelName.text = LabelMain.Instance.selectFileName;
            resetLabel();

            LabelMain.Instance.selectMapChosen = true;
        }

        // 取得使用者的經緯度
        latitude = GPS.Instance.latitude;
        longitude = GPS.Instance.longitude;
        //compass = GPS.Instance.compass;

        // 偵測手機返回鍵
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (labelDetailContent.transform.localPosition.y != 0)
            {
                // 讓 LabelContent 消失
                labelContent.transform.localPosition = new Vector3(0, -2500, 0);
                labelContentText.text = string.Empty;
                GameObject TagCamera = GameObject.Find("LabelCamera");
			    TagCamera.GetComponent<Camera>().enabled = true;
            }
            else
            {
                labelDetailContent.transform.localPosition = new Vector3(0, 2500, 0);
                GameObject TagCamera = GameObject.Find("LabelCamera");
			    TagCamera.GetComponent<Camera>().enabled = true;
            }
        }

        

        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            if (!labelTemp.Value.isNode)
            {
                //label = GameObject.Find(labelTemp.Value.labelName);
                label = labelTemp.Value.labelObject;
                if (label == null) continue;

                labelLatitude = labelTemp.Value.labelLatitude;
                labelLongitude = labelTemp.Value.labelLongitude;

                // 計算向量長度
                vectorDistance = Mathf.Sqrt(Mathf.Pow(labelLatitude - latitude, 2) + Mathf.Pow(labelLongitude - longitude, 2));
                
                // 經度-X, 緯度-Z
                labelPositionX = -1 * (maxDistance * (labelLongitude - longitude)) / vectorDistance;
                labelPositionY = 0f;
                labelPositionZ = -1 * (maxDistance * (labelLatitude - latitude)) / vectorDistance;

                label.transform.position = new Vector3(labelPositionX, labelPositionY, labelPositionZ);

                // 計算新的 labelDistance
                Calc(latitude, longitude, labelLatitude, labelLongitude);

                // 更新 labelNode 的 labelDistance
                labelTemp.Value.labelDistance = labelDistance;
                //labelTemp.Value.updateDistance();

                // 更新 label 中的 Distance 文字
                labelDistanceText = label.transform.Find("Distance").GetComponent<Text>();
                labelDistanceText.text = labelTemp.Value.labelDistance.ToString("00.00") + " 公尺";

                // 利用顏色區分距離大小
                labelDistanceFrame = label.transform.Find("Frame").GetComponent<Image>();

                if (labelTemp.Value.labelDistance <= 50)
                {
                    labelDistanceFrame.color = Color.red;
                }
                else if(labelTemp.Value.labelDistance <= 100)
                {
                    labelDistanceFrame.color = Color.yellow;
                }
                else if (labelTemp.Value.labelDistance <= 200)
                {
                    labelDistanceFrame.color = Color.green;
                }
                else if (labelTemp.Value.labelDistance <= 300)
                {
                    labelDistanceFrame.color = Color.cyan;
                }
                else if (labelTemp.Value.labelDistance <= 500)
                {
                    labelDistanceFrame.color = Color.blue;
                }
                else
                {
                    labelDistanceFrame.color = Color.black;
                }

                // 調整 label 顯示時面對使用者的角度
                Vector3 relativePos = label.transform.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(relativePos);
                label.transform.rotation = rotation;
                
                /* 
                // 取消顯是距離超過的 Label
                if(labelTemp.Value.labelDistance > LabelMain.Instance.labelDistanceLimit)
                {   
                    //Debug.Log("labelDistance > labelDistanceLimit");
                    labelTemp.Value.labelObject.SetActive(false);
                    //labelList[labelTemp.Key].labelChoose = false;
                    //LabelMain.Instance.labelList[labelTemp.Key].labelChoose = false;
					labelTemp.Value.RightMenuListItem.transform.Find("ButtonVisibility").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/RightMenu/invisible");
                }
                else
                {
                    //Debug.Log("labelDistance <= labelDistanceLimit");
                    labelTemp.Value.labelObject.SetActive(true);
                    //LabelMain.Instance.labelList[labelTemp.Key].labelChoose = true;
                    //labelTemp.Value.RightMenuListItem.transform.Find("ButtonVisibility").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/RightMenu/visible");
                }
                */

                // Debug Label Text
                LabelText.text = "labelDistance : " + vectorDistance.ToString() + " ; labelPositionX : " + labelPositionX.ToString() + " ; labelPositionZ : " + labelPositionZ.ToString();
            }
        }
    }

    private void createLabel()
    {
        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            if (!labelTemp.Value.isNode)
            {
                // 建立 label 物件, 指派 parent 為 LabelCanvas
                label = Instantiate(labelPrefab, labelParent.transform);
                label.name = labelTemp.Value.labelName;
                // 左右相反 (?)
                label.transform.localScale = new Vector3(label.transform.localScale.x, label.transform.localScale.y, label.transform.localScale.z);
                // label layer
                label.layer = 8;

                // label 的 Name
                labelNameText = label.transform.Find("Name").GetComponent<Text>();
                labelNameText.text = labelTemp.Value.labelName;

                // label 的 Image
                labelImage = label.transform.Find("Image").GetComponent<Image>();
                labelImage.sprite = labelTemp.Value.labelSprite;

                // label 的星級 labelStars，最高 5 顆
                for(int i=1; i<=5; i++)
                {
                    if (i <= labelTemp.Value.labelStars)
                        label.transform.Find("Star" + i).GetComponent<Image>().enabled = true;
                    else
                        label.transform.Find("Star" + i).GetComponent<Image>().enabled = false;
                }

                // 存 label
                labelTemp.Value.labelObject = label;
            }
        }

        Update();
    }

    // 當地圖變更時，清空現有的 label，重新向 LabelMain 索取 labelList
    public void resetLabel()
    {
        // 刪除所有的 label
        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            Destroy(GameObject.Find(labelTemp.Value.labelName));
        }
        // 重新索取 labelList
        labelList = LabelMain.Instance.labelList;
        
        // 建立 label
        createLabel();
    }

    //calculates distance between two sets of coordinates, taking into account the curvature of the earth.
    // 程式內會修改 labelDistance 的值
    public void showDetailContent()
    {
        labelDetailContent.transform.Find("LabelDetailContentText").GetComponent<Text>().text = labelList[LabelMain.Instance.selectedToLabelDetailContent].labelContent;
        labelDetailContent.transform.localPosition = new Vector3(0, 0, 0);
        GameObject TagCamera = GameObject.Find("LabelCamera");
		TagCamera.GetComponent<Camera>().enabled = false;
    }
    public void Calc(float lat1, float lon1, float lat2, float lon2)
    {

        var R = 6371.137; // Radius of earth in KM
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
