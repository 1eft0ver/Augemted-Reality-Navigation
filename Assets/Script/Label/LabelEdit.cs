using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LabelEdit : MonoBehaviour {

    private Dictionary<string, LabelNode> labelList;
    private string selectedToEditLabelDetail;

    private LabelNode label;

    public Image labelImage; // label 的圖片
    public Text labelName; // label 的名稱
    public Text labelLatitude; // label 的緯度
    public Text labelLongitude; // label 的經度
    public Text labelStars; // label 的星級

    public InputField labelNameInputField; // label 的名稱，顯示用
    public InputField labelStarsInputField; // label 的星級，顯示用

    // 選擇可到達的 Node 用
    public GameObject labelTogglePrefab; // labelToggle 模板
    public GameObject labelToggleParent; // labelToggle 放置的 Panel
    public Image arrivedPanel;

    private void Start()
    {
        // 取得主要的 labelList
        labelList = LabelMain.Instance.labelList;

        // 取得選擇的 label
        selectedToEditLabelDetail = LabelMain.Instance.selectedToEditLabelDetail;
        label = labelList[selectedToEditLabelDetail];

        labelNameInputField.text = label.labelName;
        labelLatitude.text += label.labelLatitude.ToString();
        labelLongitude.text += label.labelLongitude.ToString();


        if (label.isNode)
        {
            GameObject.Find("ImageFrame").SetActive(false);
            GameObject.Find("LabelStarsInputField").SetActive(false);
            GameObject.Find("LabelStars").SetActive(false);
        }
        else
        {
            labelStarsInputField.text = label.labelStars.ToString();
            labelImage.sprite = label.labelSprite;
        }
    }

    private void Update()
    {
        // 偵測手機返回鍵
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 從 EditLabel Scene 跳到 SetLabel Scene
            SceneManager.LoadScene("SetLabel");
        }
    }

    public void selectArrivedNode()
    {

        arrivedPanel.transform.localPosition = new Vector3(0, 0, 0);

        // 設定 labelToggle 初始位置
        float labelToggleX = labelTogglePrefab.transform.localPosition.x; // labelToggle 初始位置 X
        float labelToggleY = labelTogglePrefab.transform.localPosition.y; // labelToggle 初始位置 Y
        float labelToggleHeight = 100; // labelToggle 的高度

        // 建立 Toggle 選單
        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
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

            // 設應 isON
            if(label.arriveNodeList.ContainsKey(labelTemp.Value.labelName))
                labelTemp.Value.labelToggle.isOn = true;
            else
                labelTemp.Value.labelToggle.isOn = false;
        }
    }

    public void saveArrivedNode()
    {
        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            if (labelTemp.Value.labelToggle.isOn)
            {
                if(!label.arriveNodeList.ContainsKey(labelTemp.Value.labelName))
                {
                    label.arriveNodeList.Add(labelTemp.Value.labelName, labelTemp.Value);
                }
            }
            else
            {
                if (label.arriveNodeList.ContainsKey(labelTemp.Value.labelName))
                {
                    label.arriveNodeList.Remove(labelTemp.Value.labelName);
                }
            }
                
        }

        arrivedPanel.transform.localPosition
            = new Vector3(-1920, 0, 0);
    }

    public void saveEditData()
    {
        Debug.Log(labelName.text);

        int labelStarsNum;

        if (label.isNode)
        {
            // 更新 labelName
            if(labelName.text != label.labelName)
            {
                labelList.Remove(label.labelName);
                label.labelName = labelName.text;

                labelList.Add(label.labelName, label);
            }
            
            // 從 EditLabel Scene 跳到 SetLabel Scene
            SceneManager.LoadScene("SetLabel");
        }
        else
        {
            if (int.TryParse(labelStars.text, out labelStarsNum))
            {
                // 更新 labelName
                labelList.Remove(label.labelName);

                label.labelName = labelName.text;
                label.labelStars = labelStarsNum;
                label.labelSprite = labelImage.sprite;

                labelList.Add(label.labelName, label);

                // 從 EditLabel Scene 跳到 SetLabel Scene
                SceneManager.LoadScene("SetLabel");
            }
        }
    }
}
