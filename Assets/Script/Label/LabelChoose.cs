using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LabelChoose : MonoBehaviour {

    //public bool labelChoose; True 顯示，False 隱藏;

    private Dictionary<string, LabelNode> labelList;

    public GameObject labelTogglePrefab; // labelToggle 模板
    public GameObject labelToggleParent; // labelToggle 放置的 Canvas
    private float labelToggleX; // labelToggle 初始位置 X
    private float labelToggleY; // labelToggle 初始位置 Y
    private float labelToggleHeight; // labelToggle 的高度

    private char[] labelMarker; // labelMarker 給 Map 用
    private int labelMarkerCounter; // labelMarkerCounter 計算 labelMarker

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

        // 建立 Toggle 選單
        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            if (!labelTemp.Value.isNode)
            {
                // 建立 labelToggle 物件, 指派 parent 為 LabelCanvas
                labelTemp.Value.labelToggle = Instantiate(labelTogglePrefab, labelToggleParent.transform).GetComponent<Toggle>();
                labelTemp.Value.labelToggle.name = labelTemp.Value.labelName;

                // 取消 Edit Button
                labelTemp.Value.labelToggle.transform.Find("Button").gameObject.SetActive(false);

                // 調整 labelToggle 的 Y 軸位置
                labelTemp.Value.labelToggle.transform.localPosition = new Vector2(labelToggleX, labelToggleY);
                labelToggleY -= labelToggleHeight;

                // 設定 labelToggle 中的文字
                Text labelTempText = labelTemp.Value.labelToggle.transform.Find("Label").GetComponent<Text>();
                labelTempText.text = labelTemp.Value.labelName;

                // 改變 Node 顏色
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
                labelTemp.Value.labelToggle.isOn = labelTemp.Value.labelChoose;
            }
        }
    }

    private void Update()
    {
        // 偵測手機返回鍵
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 從 ChooseLabel Scene 跳到 AR Scene
            SceneManager.LoadScene("AR");
        }
    }

    public void setlabelChoose()
    {
        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            if (!labelTemp.Value.isNode)
                labelTemp.Value.labelChoose = labelTemp.Value.labelToggle.isOn;
        }
    }
}
