using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagVisibility : MonoBehaviour {
	public void VisibilitySwitch() {
		string targetName = this.transform.parent.Find("TagName").GetComponent<Text>().text;
		GameObject target = GameObject.Find(targetName);
		GameObject parentCanvas = GameObject.Find("LabelCanvas");
		
		if(this.transform.parent.Find("ButtonVisibility").GetComponent<Image>().sprite.name == "visible" ) {
			this.transform.parent.Find("ButtonVisibility").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/RightMenu/invisible");
			target.SetActive(false);
		}
		else {
			this.transform.parent.Find("ButtonVisibility").GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/RightMenu/visible");
			parentCanvas.transform.Find(targetName).gameObject.SetActive(true);
		}
	}
}
