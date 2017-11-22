using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelClick : MonoBehaviour {

    private Dictionary<string, LabelNode> labelList;

    // Label 的內容
    GameObject labelContent; // 內容的 Panel
    Text labelContentText; // 內容文字
    /*Image labelContentBackground; // 背景顏色
    GameObject navigationButton; // 導航的按鈕
    GameObject detailContentButton; // 詳細內容的按鈕*/

    public void Start()
    {
        // 取得主要的 labelList
        labelList = LabelMain.Instance.labelList;

        // 初始化所有 LabelContent 有關的原件
        labelContent = GameObject.Find("LabelContent");
        labelContentText = labelContent.transform.Find("LabelContentText").GetComponent<Text>();
        /*labelContentBackground = labelContent.transform.Find("LabelContentBackground").GetComponent<Image>();
        navigationButton = labelContent.transform.Find("NavigationButton").gameObject;
        detailContentButton = labelContent.transform.Find("DetailContentButton").gameObject;*/
    }

    public void getLabelContent()
    {
        Debug.Log("getLabelContent");
        GameObject TagCamera = GameObject.Find("LabelCamera");
		TagCamera.GetComponent<Camera>().enabled = false;
        // 讓 LabelContent 出現
        /*labelContentBackground.enabled = true;
        labelContentText.enabled = true;
        navigationButton.GetComponent<Button>().enabled = true;
        navigationButton.GetComponent<Image>().enabled = true;
        detailContentButton.GetComponent<Button>().enabled = true;
        detailContentButton.GetComponent<Image>().enabled = true;*/
        labelContent.transform.localPosition = new Vector3(0, -375, 0);

        Debug.Log(labelList[GetComponent<Image>().name].labelContent);
        LabelMain.Instance.clickedLabel = GetComponent<Image>().name;
        GameObject.Find("ClickedLabelName").GetComponent<Text>().text = LabelMain.Instance.clickedLabel;
        // 設定內容
        LabelMain.Instance.selectedToLabelDetailContent = GetComponent<Image>().name;
        labelContentText.text = labelList[GetComponent<Image>().name].labelContent;
    }
}
