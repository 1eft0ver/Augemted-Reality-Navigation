using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLebelDisplayDist : MonoBehaviour {
	private Dictionary<string, LabelNode> labelList;

	IEnumerator ShowAndHide( GameObject go, float delay ) {
        go.transform.Find("ApplyStatus").gameObject.SetActive(true);
        GameObject.Find("ApplyStatusText").GetComponent<Text>().text = "設定套用完成！";
        yield return new WaitForSeconds(delay);
        go.transform.Find("ApplyStatus").gameObject.SetActive(false);
    }
	public void distanceChange(Text input) {
		
		int distance = Int32.Parse(input.text);
		LabelMain.Instance.distChanged = true;
		LabelMain.Instance.labelDistanceLimit = distance;
		Debug.Log("可視距離改變為：" + LabelMain.Instance.labelDistanceLimit );
		// 取消顯是距離超過的 Label
		labelList = LabelMain.Instance.labelList;
		foreach (KeyValuePair<string, LabelNode> labelTemp in labelList) {
			if(labelTemp.Value.labelDistance > LabelMain.Instance.labelDistanceLimit)
			{   
				//Debug.Log("labelDistance > labelDistanceLimit");
				labelTemp.Value.labelObject.SetActive(false);
				//labelList[labelTemp.Key].labelChoose = false;
				//LabelMain.Instance.labelList[labelTemp.Key].labelChoose = false;
				labelTemp.Value.RightMenuListItem.transform.Find("ButtonVisibility").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/RightMenu/invisible");
			}
			else
			{
				//Debug.Log("labelDistance <= labelDistanceLimit");
				labelTemp.Value.labelObject.SetActive(true);
				//LabelMain.Instance.labelList[labelTemp.Key].labelChoose = true;
				//labelTemp.Value.RightMenuListItem.transform.Find("ButtonVisibility").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/RightMenu/visible");
			}
		}
		StartCoroutine( ShowAndHide(GameObject.Find("MenuSet"), 3.0f) );
	}
	
}
