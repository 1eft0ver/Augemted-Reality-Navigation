using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LabelSetting : MonoBehaviour {

    private float latitude; // 人所在的緯度
    private float longitude; // 人所在的經度

    private Dictionary<string, LabelNode> labelList;

    private LabelNode label; // 暫存 label
    private LabelNode node; // 暫存 node

    public GameObject labelTogglePrefab; // labelToggle 模板
    public GameObject labelToggleParent; // labelToggle 放置的 Canvas
    private float labelToggleX; // labelToggle 初始位置 X
    private float labelToggleY; // labelToggle 初始位置 Y
    private float labelToggleHeight; // labelToggle 的高度

    private char[] labelMarker; // labelMarker 給 Map 用
    private int labelMarkerCounter; // labelMarkerCounter 計算 labelMarker

    public Text labelLatitudeText;
    public Text labelLongitudeText;
    public Text labelListText;

    private void Start()
    {
        // 取得主要的 labelList
        labelList = LabelMain.Instance.labelList;

        // 建立 labelMarker
        labelMarkerCounter = 0;
        labelMarker = new char[] {
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
        'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
        'U', 'V', 'W', 'X', 'Y', 'Z', '1', '2', '3', '4',
        '5', '6', '7', '8', '9', '0'};

        // 設定 labelToggle 初始位置
        labelToggleX = labelTogglePrefab.transform.localPosition.x;
        labelToggleY = labelTogglePrefab.transform.localPosition.y;
        labelToggleHeight = 100;

        // 如果 labelList 本來就有東西
        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            //Instantiate(labelTemp.Value.labelToggle);

            // 建立 labelToggle 物件, 指派 parent 為 LabelCanvas
            labelTemp.Value.labelToggle = Instantiate(labelTogglePrefab, labelToggleParent.transform).GetComponent<Toggle>();
            labelTemp.Value.labelToggle.name = labelTemp.Value.labelName;

            // 調整 labelToggle 的 Y 軸位置
            labelTemp.Value.labelToggle.transform.localPosition = new Vector2(labelToggleX, labelToggleY);
            labelToggleY -= labelToggleHeight;

            // 設定 labelToggle 中的文字
            Text labelTempText = labelTemp.Value.labelToggle.transform.Find("Label").GetComponent<Text>();
            labelTempText.text = labelTemp.Value.labelName;

            if (labelTemp.Value.isNode)
            {
                labelTempText.color = Color.blue;
            }

            if (labelMarkerCounter < labelMarker.Length)
            {
                labelTempText.text = labelMarker[labelMarkerCounter] + ": " + labelTempText.text;
                labelMarkerCounter++;
            }
            
            // 設應 isON
            labelTemp.Value.labelToggle.isOn = false;
        }
    }

    private void Update()
    {
        // 偵測手機返回鍵
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 從 SetLabel Scene 跳到 AR Scene
            SceneManager.LoadScene("AR");
        }

        // 取得使用者的經緯度
        latitude = GPS.Instance.latitude;
        longitude = GPS.Instance.longitude;

        labelLatitudeText.text = "Latitude: " + latitude;
        labelLongitudeText.text = "Longitude: " + longitude;
    }

    public void setLabel(bool isNode)
    {
        Debug.Log("Click Button setLabel");

        Text LabelName = GameObject.Find("LabelName").transform.Find("Text").GetComponent<Text>(); ;

        Debug.Log(LabelName.text.ToString());

        if (LabelName.text == string.Empty)
        {
            Debug.Log("Please Input Text");
            return;
        }
        if (!labelList.ContainsKey(LabelName.text))
        {
            // 建立 label 和 labelToggle
            label = new LabelNode(LabelName.text, latitude, longitude);

            // 設定 isNode
            label.isNode = isNode;

            // 建立 labelToggle 物件, 指派 parent 為 LabelCanvas
            label.labelToggle = Instantiate(labelTogglePrefab, labelToggleParent.transform).GetComponent<Toggle>();
            label.labelToggle.name = label.labelName;

            // 調整 labelToggle 的 Y 軸位置
            label.labelToggle.transform.localPosition = new Vector2(labelToggleX, labelToggleY);
            labelToggleY -= labelToggleHeight;

            // 設定 labelToggle 中的文字
            Text labelText = label.labelToggle.transform.Find("Label").GetComponent<Text>();
            labelText.text = label.labelName;

            if(label.isNode)
            {
                labelText.color = Color.blue;
            }

            if (labelMarkerCounter < labelMarker.Length)
            {
                labelText.text = labelMarker[labelMarkerCounter] + ": " + labelText.text;
                labelMarkerCounter++;
            }

            // 設定 isON
            label.labelToggle.isOn = false;

            // 把 label 加入 List 中
            labelList.Add(LabelName.text, label);
        }
    }

    public void saveMap(Text FileName)
    {
        string dirPath = Application.persistentDataPath + "/" + FileName.text;
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        if (!Directory.Exists(dirPath + "/content"))
        {
            Directory.CreateDirectory(dirPath + "/content");
        }

        string nodePath = Application.persistentDataPath + "/" + FileName.text + "/" + "map.txt";
        string labelPath = Application.persistentDataPath + "/" + FileName.text + "/" + "tag.txt";

        // 寫入 map.txt 和 tag.txt
        StreamWriter nodeWriter = new StreamWriter(nodePath, false, Encoding.UTF8);
        StreamWriter labeWriter = new StreamWriter(labelPath, false, Encoding.UTF8);

        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            string arrivedString = string.Empty;

            foreach (KeyValuePair<string, LabelNode> arrivedTemp in labelTemp.Value.arriveNodeList)
            {
                arrivedString += (" " + arrivedTemp.Value.labelName);
            }

            nodeWriter.WriteLine(labelTemp.Value.labelName + " " + labelTemp.Value.labelLatitude + " " + labelTemp.Value.labelLongitude + arrivedString);

            if(!labelTemp.Value.isNode)
            {
                labeWriter.WriteLine(labelTemp.Value.labelName + " " + labelTemp.Value.labelLatitude + " " + labelTemp.Value.labelLongitude + " " + labelTemp.Value.labelStars);

                // 將圖片存起來
                if (labelTemp.Value.labelSprite != null)
                {
                    string imagePath = Application.persistentDataPath + "/" + FileName.text + "/content/" + labelTemp.Value.labelName + ".jpg";
                    Stream imageFile = File.Open(imagePath, FileMode.Create);
                    BinaryWriter imageWriter = new BinaryWriter(imageFile);
                    byte[] FileData = labelTemp.Value.labelSprite.texture.EncodeToJPG();

                    imageWriter.Write(FileData);
                    imageWriter.Close();
                    imageFile.Close();
                }
            }
        }

        labeWriter.Close();
        nodeWriter.Close();
    }

    public void deleteLabel()
    {
        List<string> deleteKey = new List<string>();

        // 選出要被刪除的 label，利用 Toggle isON 判斷，並刪除所有的 labelToggle
        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            Destroy(GameObject.Find(labelTemp.Value.labelName));

            if (labelTemp.Value.labelToggle.isOn)
            {
                deleteKey.Add(labelTemp.Key);
            }
        }

        // 從 labelList 中刪除 label
        foreach (string keyTemp in deleteKey)
        {
            labelList.Remove(keyTemp);
        }

        // 設定 labelToggle 初始位置
        labelToggleX = labelTogglePrefab.transform.localPosition.x;
        labelToggleY = labelTogglePrefab.transform.localPosition.y;

        // 重新設定 labelMarkerCounter
        labelMarkerCounter = 0;

        // 重新建立 labelToggle
        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            // 建立 labelToggle 物件, 指派 parent 為 LabelCanvas
            labelTemp.Value.labelToggle = Instantiate(labelTogglePrefab, labelToggleParent.transform).GetComponent<Toggle>();
            labelTemp.Value.labelToggle.name = labelTemp.Value.labelName;

            // 調整 labelToggle 的 Y 軸位置
            labelTemp.Value.labelToggle.transform.localPosition = new Vector2(labelToggleX, labelToggleY);
            labelToggleY -= labelToggleHeight;

            // 設定 labelToggle 中的文字
            Text labelTempText = labelTemp.Value.labelToggle.transform.Find("Label").GetComponent<Text>();
            labelTempText.text = labelTemp.Value.labelName;

            if (labelTemp.Value.isNode)
            {
                labelTempText.color = Color.blue;
            }

            if (labelMarkerCounter < labelMarker.Length)
            {
                labelTempText.text = labelMarker[labelMarkerCounter] + ": " + labelTempText.text;
                labelMarkerCounter++;
            }

            // 設應 isON
            labelTemp.Value.labelToggle.isOn = false;
        }
    }

    public void clearMap()
    {   
        // 刪除所有的 labelToggle 並清空 labelList
        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            Destroy(labelTemp.Value.labelToggle);
            Destroy(GameObject.Find(labelTemp.Value.labelName));
        }

        labelList.Clear();

        // 設定 labelToggle 初始位置
        labelToggleX = labelTogglePrefab.transform.localPosition.x;
        labelToggleY = labelTogglePrefab.transform.localPosition.y;

        // 重新設定 labelMarkerCounter
        labelMarkerCounter = 0;
    }
}
