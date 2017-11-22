using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAbout : MonoBehaviour {
	public void AboutAnimation() {

		UIControl.uiStatus = 5;
		//ButtonRotate.rotating = true;
		//箭頭按鈕互換
		GameObject.Find("BackButtonHide").GetComponent<ScrollRect>().enabled = false;
		MonoBehaviour BackButtonHide = GameObject.Find("BackButtonHide").GetComponent("FasterScroll") as MonoBehaviour;
		BackButtonHide.enabled = true;
		GameObject.Find("BackButtonAnchor").GetComponent<ScrollRect>().enabled = false;
		MonoBehaviour BackButtonAnchor = GameObject.Find("BackButtonAnchor").GetComponent("FasterScroll") as MonoBehaviour;
		BackButtonAnchor.enabled = true;
		
		//釋放
		GameObject.Find("AnchorNavi").GetComponent<ScrollRect>().enabled = false;
		GameObject.Find("AnchorMap").GetComponent<ScrollRect>().enabled = false;
		GameObject.Find("AnchorSet").GetComponent<ScrollRect>().enabled = false;
		GameObject.Find("AnchorAbout").GetComponent<ScrollRect>().enabled = false;

		//放置當前選擇到Title
		MonoBehaviour TitleScroll = GameObject.Find("TitleAnchor").GetComponent("ScrollAbout") as MonoBehaviour;
		TitleScroll.enabled = true;
		MonoBehaviour NaviScroll = GameObject.Find("TopAnchor").GetComponent("ScrollNavi") as MonoBehaviour;
		NaviScroll.enabled = true;
		MonoBehaviour MapScroll = GameObject.Find("TopAnchor").GetComponent("ScrollMap") as MonoBehaviour;
		MapScroll.enabled = true;
		MonoBehaviour SetScroll = GameObject.Find("TopAnchor").GetComponent("ScrollSet") as MonoBehaviour;
		SetScroll.enabled = true;

		//召喚Menu
		GameObject.Find("MenuAnchorAbout").GetComponent<ScrollRect>().enabled = false;
		MonoBehaviour CallMenu = GameObject.Find("LeftMenu").GetComponent("MenuAbout") as MonoBehaviour;
		CallMenu.enabled = true;
	}

}
