using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelectDest : MonoBehaviour {
	private Dictionary<string, LabelNode> labelList;
	public GameObject DestListPrefab; // ButtonList 模板
	public GameObject ScrollParent;
	private GameObject ParentAnchor;
	private GameObject ScrollBar;
	private float ButtonListHeight;
	private float ButtonListX;
	private float ButtonListY;
	private Vector3 AnchorOri;
	public void createList() {
		ScrollParent = GameObject.Find("DestScrollPanel");
        ParentAnchor = GameObject.Find("DestScrollAnchor");
        ScrollBar = GameObject.Find("DestMenuScrollbar");
        ButtonListHeight = 175;

		int supposedHeight = 0;
        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList) {
            supposedHeight += 175;
        }
		ScrollParent.GetComponent<RectTransform>().sizeDelta = new Vector2(1088, supposedHeight + 350);

		if(supposedHeight <= 1225){
			ParentAnchor.GetComponent<RectTransform>().localPosition = new Vector3(ParentAnchor.GetComponent<RectTransform>().localPosition.x, -200 ,ParentAnchor.GetComponent<RectTransform>().localPosition.z);
            ScrollParent.GetComponent<RectTransform>().position = new Vector3(ScrollParent.GetComponent<RectTransform>().position.x, -5, ScrollParent.GetComponent<RectTransform>().position.z );
		}
		else 
            ParentAnchor.GetComponent<RectTransform>().localPosition = new Vector3(ParentAnchor.GetComponent<RectTransform>().localPosition.x, AnchorOri.y - 175 ,ParentAnchor.GetComponent<RectTransform>().localPosition.z);
        ButtonListX = DestListPrefab.transform.localPosition.x;
        ButtonListY = ScrollParent.GetComponent<RectTransform>().rect.height/2 + 180;
        // 建立 ButtonList 選單
        foreach (KeyValuePair<string, LabelNode> labelTemp in labelList)
        {
            
            if (!labelTemp.Value.isNode)
            {	
				labelTemp.Value.DestMenuListItem = Instantiate(DestListPrefab, ScrollParent.transform);
				labelTemp.Value.DestMenuListItem.transform.SetParent(ScrollParent.transform, false);
				labelTemp.Value.DestMenuListItem.name = "dest_" + labelTemp.Value.labelName;
                labelTemp.Value.DestMenuListItem.transform.Find("DestName").GetComponent<Text>().text = labelTemp.Value.labelName;
				labelTemp.Value.DestMenuListItem.transform.localPosition = new Vector2(ButtonListX, ButtonListY);
				ButtonListY -= ButtonListHeight;
            }
        }
        
        
        ScrollBar.GetComponent<Scrollbar>().value = 1;
        //LabelMain.Instance.selectFileNameChange = false;
	}
	void Start () {
		labelList = LabelMain.Instance.labelList;
	}
	
	// Update is called once per frame
	void Update () {
		if(LabelMain.Instance.selectMapChosen == true) {
            
            foreach (KeyValuePair<string, LabelNode> labelTemp in labelList) {
                Destroy(GameObject.Find("dest_" + labelTemp.Value.labelName));
            }
            //Debug.Log("Change map");
            labelList = LabelMain.Instance.labelList;
            createList();
            GameObject.Find("DestMenuScrollbar").GetComponent<Scrollbar>().value = 1;
        }
	}
}
