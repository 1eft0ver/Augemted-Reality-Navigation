using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AniDelete : MonoBehaviour {

	public void DeleteAnimation() {
		UIControl.uiStatus = 8;

		//釋放
		GameObject.Find("AnchorSelectMap").GetComponent<ScrollRect>().enabled = false;
		GameObject.Find("AnchorDownloadMap").GetComponent<ScrollRect>().enabled = false;
		GameObject.Find("AnchorDeleteMap").GetComponent<ScrollRect>().enabled = false;

		//放置當前選擇到Title
		MonoBehaviour SelectScroll = GameObject.Find("MenuMapTopAnchor").GetComponent("ScrollSelect") as MonoBehaviour;
		SelectScroll.enabled = true;
		MonoBehaviour DownloadScroll = GameObject.Find("MenuMapTopAnchor").GetComponent("ScrollDownload") as MonoBehaviour;
		DownloadScroll.enabled = true;
		MonoBehaviour DeleteScroll = GameObject.Find("MenuMapTitleAnchor").GetComponent("ScrollDelete") as MonoBehaviour;
		DeleteScroll.enabled = true;

		//呼叫ItemView
		MonoBehaviour ItemScroll = GameObject.Find("ItemViewAnchor").GetComponent("DeleteViewScroll") as MonoBehaviour;
		ItemScroll.enabled = true;
		ItemScroll = GameObject.Find("ItemViewAnchor").GetComponent("DownloadViewScroll") as MonoBehaviour;
		ItemScroll.enabled = false;
		ItemScroll = GameObject.Find("ItemViewAnchor").GetComponent("SelectViewScroll") as MonoBehaviour;
		ItemScroll.enabled = false;
		ItemScroll = GameObject.Find("ItemViewHide").GetComponent("DeleteViewScroll") as MonoBehaviour;
		ItemScroll.enabled = false;
		ItemScroll = GameObject.Find("ItemViewHide").GetComponent("DownloadViewScroll") as MonoBehaviour;
		ItemScroll.enabled = true;
		ItemScroll = GameObject.Find("ItemViewHide").GetComponent("SelectViewScroll") as MonoBehaviour;
		ItemScroll.enabled = true;

		
	}
}
