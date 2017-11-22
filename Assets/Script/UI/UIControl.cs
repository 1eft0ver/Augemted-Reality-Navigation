using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIControl : MonoBehaviour {
	private Vector3 AnchorOri;
    public static int uiStatus = 0;
	// Use this for initialization
	void Start () {
		Vector3 AnchorOri = GameObject.Find("ListScrollAnchor").GetComponent<RectTransform>().localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)) {
            backToHomePage();
        }
	}

	public void backToHomePage() {
		if(uiStatus == 1 || uiStatus == 2) {
			uiStatus = 0;
			GameObject TagCamera = GameObject.Find("LabelCamera");
			GameObject Arrow = GameObject.Find("arrow");
			GameObject Anchor =  GameObject.Find("ScrollAnchor");
			GameObject leftBackPoint =  GameObject.Find("BackPointLeft");
			GameObject rightBackPoint =  GameObject.Find("BackPointRight");
			MonoBehaviour scroll= Anchor.GetComponent("FasterScroll") as MonoBehaviour;
			TagCamera.GetComponent<Camera>().enabled = true;
			if(Dijkstra.inNavi)
				Arrow.transform.Find("default").gameObject.SetActive(true);
			Anchor.GetComponent<ScrollRect>().enabled = false;
			scroll.enabled = false;
			leftBackPoint.GetComponent<ScrollRect>().enabled = true;
			rightBackPoint.GetComponent<ScrollRect>().enabled = true;
		}
		else if(uiStatus == 3 || uiStatus == 4 || uiStatus == 5 || uiStatus == 6) {
			uiStatus = 1;
			
			//箭頭按鈕互換
			GameObject.Find("BackButtonHide").GetComponent<ScrollRect>().enabled = true;
			MonoBehaviour BackButtonHide = GameObject.Find("BackButtonHide").GetComponent("FasterScroll") as MonoBehaviour;
			BackButtonHide.enabled = false;
			GameObject.Find("BackButtonAnchor").GetComponent<ScrollRect>().enabled = true;
			MonoBehaviour BackButtonAnchor = GameObject.Find("BackButtonAnchor").GetComponent("FasterScroll") as MonoBehaviour;
			BackButtonAnchor.enabled = false;

			GameObject.Find("AnchorNavi").GetComponent<ScrollRect>().enabled = true;
			GameObject.Find("AnchorMap").GetComponent<ScrollRect>().enabled = true;
			GameObject.Find("AnchorSet").GetComponent<ScrollRect>().enabled = true;
			GameObject.Find("AnchorAbout").GetComponent<ScrollRect>().enabled = true;
			MonoBehaviour TitleScrollNavi = GameObject.Find("TitleAnchor").GetComponent("ScrollNavi") as MonoBehaviour;
			TitleScrollNavi.enabled = false;
			MonoBehaviour TitleScrollMap = GameObject.Find("TitleAnchor").GetComponent("ScrollMap") as MonoBehaviour;
			TitleScrollMap.enabled = false;
			MonoBehaviour TitleScrollSet = GameObject.Find("TitleAnchor").GetComponent("ScrollSet") as MonoBehaviour;
			TitleScrollSet.enabled = false;
			MonoBehaviour TitleScrollAbout = GameObject.Find("TitleAnchor").GetComponent("ScrollAbout") as MonoBehaviour;
			TitleScrollAbout.enabled = false;
			MonoBehaviour BotNaviScroll = GameObject.Find("BotAnchor").GetComponent("ScrollNavi") as MonoBehaviour;
			BotNaviScroll.enabled = false;
			MonoBehaviour BotMapScroll = GameObject.Find("BotAnchor").GetComponent("ScrollMap") as MonoBehaviour;
			BotMapScroll.enabled = false;
			MonoBehaviour BotSetScroll = GameObject.Find("BotAnchor").GetComponent("ScrollSet") as MonoBehaviour;
			BotSetScroll.enabled = false;
			MonoBehaviour BotAboutScroll = GameObject.Find("BotAnchor").GetComponent("ScrollAbout") as MonoBehaviour;
			BotAboutScroll.enabled = false;
			MonoBehaviour TopNaviScroll = GameObject.Find("TopAnchor").GetComponent("ScrollNavi") as MonoBehaviour;
			TopNaviScroll.enabled = false;
			MonoBehaviour TopMapScroll = GameObject.Find("TopAnchor").GetComponent("ScrollMap") as MonoBehaviour;
			TopMapScroll.enabled = false;
			MonoBehaviour TopSetScroll = GameObject.Find("TopAnchor").GetComponent("ScrollSet") as MonoBehaviour;
			TopSetScroll.enabled = false;
			MonoBehaviour TopAboutScroll = GameObject.Find("TopAnchor").GetComponent("ScrollAbout") as MonoBehaviour;
			TopAboutScroll.enabled = false;
			
			//退回Menu
			GameObject.Find("MenuAnchorAbout").GetComponent<ScrollRect>().enabled = true;
			MonoBehaviour CallMenu1 = GameObject.Find("LeftMenu").GetComponent("MenuAbout") as MonoBehaviour;
			CallMenu1.enabled = false;
			GameObject.Find("MenuAnchorMap").GetComponent<ScrollRect>().enabled = true;
			MonoBehaviour CallMenu2 = GameObject.Find("LeftMenu").GetComponent("MenuMap") as MonoBehaviour;
			CallMenu2.enabled = false;
			GameObject.Find("MenuAnchorNavi").GetComponent<ScrollRect>().enabled = true;
			MonoBehaviour CallMenu3 = GameObject.Find("LeftMenu").GetComponent("MenuNavi") as MonoBehaviour;
			CallMenu3.enabled = false;
			GameObject.Find("MenuAnchorSet").GetComponent<ScrollRect>().enabled = true;
			MonoBehaviour CallMenu4 = GameObject.Find("LeftMenu").GetComponent("MenuSet") as MonoBehaviour;
			CallMenu4.enabled = false;
			GameObject.Find("arrow").transform.Find("default").gameObject.SetActive(false);
			GameObject.Find("LabelCamera").GetComponent<Camera>().enabled = false;
		}
		else if(uiStatus == 8) {
			uiStatus = 4;
			//釋放
			MonoBehaviour Scroll = GameObject.Find("MenuMapTitleAnchor").GetComponent("ScrollSelect") as MonoBehaviour;
			Scroll.enabled = false;
			Scroll = GameObject.Find("MenuMapTitleAnchor").GetComponent("ScrollDownload") as MonoBehaviour;
			Scroll.enabled = false;
			Scroll = GameObject.Find("MenuMapTitleAnchor").GetComponent("ScrollDelete") as MonoBehaviour;
			Scroll.enabled = false;

			Scroll = GameObject.Find("MenuMapTopAnchor").GetComponent("ScrollSelect") as MonoBehaviour;
			Scroll.enabled = false;
			Scroll = GameObject.Find("MenuMapTopAnchor").GetComponent("ScrollDownload") as MonoBehaviour;
			Scroll.enabled = false;
			Scroll = GameObject.Find("MenuMapTopAnchor").GetComponent("ScrollDelete") as MonoBehaviour;
			Scroll.enabled = false;

			Scroll = GameObject.Find("MenuMapBotAnchor").GetComponent("ScrollSelect") as MonoBehaviour;
			Scroll.enabled = false;
			Scroll = GameObject.Find("MenuMapBotAnchor").GetComponent("ScrollDownload") as MonoBehaviour;
			Scroll.enabled = false;
			Scroll = GameObject.Find("MenuMapBotAnchor").GetComponent("ScrollDelete") as MonoBehaviour;
			Scroll.enabled = false;



			//歸位
			GameObject.Find("AnchorSelectMap").GetComponent<ScrollRect>().enabled = true;
			GameObject.Find("AnchorDownloadMap").GetComponent<ScrollRect>().enabled = true;
			GameObject.Find("AnchorDeleteMap").GetComponent<ScrollRect>().enabled = true;

			MonoBehaviour ItemScroll = GameObject.Find("ItemViewAnchor").GetComponent("DeleteViewScroll") as MonoBehaviour;
			ItemScroll.enabled = false;
			ItemScroll = GameObject.Find("ItemViewAnchor").GetComponent("DownloadViewScroll") as MonoBehaviour;
			ItemScroll.enabled = false;
			ItemScroll = GameObject.Find("ItemViewAnchor").GetComponent("SelectViewScroll") as MonoBehaviour;
			ItemScroll.enabled = false;
			ItemScroll = GameObject.Find("ItemViewHide").GetComponent("DeleteViewScroll") as MonoBehaviour;
			ItemScroll.enabled = true;
			ItemScroll = GameObject.Find("ItemViewHide").GetComponent("DownloadViewScroll") as MonoBehaviour;
			ItemScroll.enabled = true;
			ItemScroll = GameObject.Find("ItemViewHide").GetComponent("SelectViewScroll") as MonoBehaviour;
			ItemScroll.enabled = true;

			GameObject.Find("arrow").transform.Find("default").gameObject.SetActive(false);
			GameObject.Find("LabelCamera").GetComponent<Camera>().enabled = false;
		}
	}
}

