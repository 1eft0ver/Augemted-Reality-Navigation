using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSet : MonoBehaviour {
	public void setAnimation() {

		UIControl.uiStatus = 5;
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
		MonoBehaviour TitleScroll = GameObject.Find("TitleAnchor").GetComponent("ScrollSet") as MonoBehaviour;
		TitleScroll.enabled = true;
		MonoBehaviour NaviScroll = GameObject.Find("TopAnchor").GetComponent("ScrollNavi") as MonoBehaviour;
		NaviScroll.enabled = true;
		MonoBehaviour MapScroll = GameObject.Find("TopAnchor").GetComponent("ScrollMap") as MonoBehaviour;
		MapScroll.enabled = true;
		MonoBehaviour AboutScroll = GameObject.Find("BotAnchor").GetComponent("ScrollAbout") as MonoBehaviour;
		AboutScroll.enabled = true;

		//召喚Menu
		GameObject.Find("MenuAnchorSet").GetComponent<ScrollRect>().enabled = false;
		MonoBehaviour CallMenu = GameObject.Find("LeftMenu").GetComponent("MenuSet") as MonoBehaviour;
		CallMenu.enabled = true;
	}

}
