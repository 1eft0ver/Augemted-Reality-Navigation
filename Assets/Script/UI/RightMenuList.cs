using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightMenuList : MonoBehaviour {
	private Dictionary<string, LabelNode> labelList;
    public GameObject ButtonListPrefab; // ButtonList 模板
    public GameObject ButtonListParent; // ButtonList 放置的 Canvas
    private float ButtonListX; // ButtonList 初始位置 X
    private float ButtonListY; // ButtonList 初始位置 Y
    private float ButtonListHeight; // ButtonList 的高度

    //private char[] labelMarker; // labelMarker 給 Map 用
    //private int labelMarkerCounter; // labelMarkerCounter 計算 labelMarker
	
	private GameObject ScrollParent;
    private GameObject ParentAnchor;
    private GameObject ScrollBar;
    private Vector3 AnchorOri;
    public void createList() {
        // 設定 ButtonList 初始位置
        
        ButtonListHeight = 175;
        
        int supposedHeight = 0;
        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList) {
            supposedHeight += 175;
        }
        
        ScrollParent.GetComponent<RectTransform>().sizeDelta = new Vector2(1088, supposedHeight + 350);
        if(supposedHeight <= 1575){
            ParentAnchor.GetComponent<RectTransform>().localPosition = new Vector3(ParentAnchor.GetComponent<RectTransform>().localPosition.x, -200 ,ParentAnchor.GetComponent<RectTransform>().localPosition.z);
            ScrollParent.GetComponent<RectTransform>().position = new Vector3(ScrollParent.GetComponent<RectTransform>().position.x, -5, ScrollParent.GetComponent<RectTransform>().position.z );
        }
        else 
            ParentAnchor.GetComponent<RectTransform>().localPosition = new Vector3(ParentAnchor.GetComponent<RectTransform>().localPosition.x,AnchorOri.y - 175 ,ParentAnchor.GetComponent<RectTransform>().localPosition.z);
        ButtonListX = ButtonListPrefab.transform.localPosition.x;
        ButtonListY = ScrollParent.GetComponent<RectTransform>().rect.height/2 + 180;
        // 建立 ButtonList 選單
        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            
            if (!labelTemp.Value.isNode)
            {	
				labelTemp.Value.RightMenuListItem = Instantiate(ButtonListPrefab, ButtonListParent.transform);
				labelTemp.Value.RightMenuListItem.transform.SetParent(ScrollParent.transform, false);
				if(labelTemp.Value.labelDistance <= LabelMain.Instance.labelDistanceLimit) 
					labelTemp.Value.RightMenuListItem.transform.Find("ButtonVisibility").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/RightMenu/visible");
				else if(labelTemp.Value.labelDistance > LabelMain.Instance.labelDistanceLimit) 
					labelTemp.Value.RightMenuListItem.transform.Find("ButtonVisibility").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/RightMenu/invisible");
				labelTemp.Value.RightMenuListItem.name = "item_" + labelTemp.Value.labelName;
                labelTemp.Value.RightMenuListItem.transform.Find("TagName").GetComponent<Text>().text = labelTemp.Value.labelName;
				labelTemp.Value.RightMenuListItem.transform.Find("TagName").GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
				labelTemp.Value.RightMenuListItem.transform.localPosition = new Vector2(ButtonListX, ButtonListY);
				ButtonListY -= ButtonListHeight;
            }
        }
        
        
        ScrollBar.GetComponent<Scrollbar>().value = 1;
        //LabelMain.Instance.selectFileNameChange = false;
    }
	void Start () {
		// 取得主要的 labelList
        labelList = LabelMain.Instance.labelList;
        ScrollParent = GameObject.Find("ListScrollPanel");
        ParentAnchor = GameObject.Find("ListScrollAnchor");
        ScrollBar = GameObject.Find("RightMenuScrollbar");
        Vector3 AnchorOri = ParentAnchor.GetComponent<RectTransform>().localPosition;
        // 建立 labelMarker
        //labelMarkerCounter = 0;
	}
	
	// Update is called once per frame
	void Update () {
        //labelList = LabelMain.Instance.labelList;
        if(LabelMain.Instance.selectMapChosen == true) {
            Debug.Log("Map changed");
            foreach (KeyValuePair<string, LabelNode> labelTemp in labelList) {
                Destroy(GameObject.Find("item_" + labelTemp.Value.labelName));
                Debug.Log("Destroy item_"+ labelTemp.Value.labelName);
            }
            //Debug.Log("Change map");
            labelList = LabelMain.Instance.labelList;
            createList();
            LabelMain.Instance.selectMapChosen = false;
            GameObject.Find("RightMenuScrollbar").GetComponent<Scrollbar>().value = 1;
        }
        if(LabelMain.Instance.distChanged == true) {
            Debug.Log("Distance changed");
            LabelMain.Instance.distChanged = false;
            foreach (KeyValuePair<string, LabelNode> labelTemp in labelList) {
                Destroy(GameObject.Find("item_" + labelTemp.Value.labelName));
            }
            //Debug.Log("Change map");
            labelList = LabelMain.Instance.labelList;
            createList();
            GameObject.Find("RightMenuScrollbar").GetComponent<Scrollbar>().value = 1;
        }
	}
}
