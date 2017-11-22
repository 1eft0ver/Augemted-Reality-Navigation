using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ArrowPoint : MonoBehaviour {
	void Update () {
		Vector2 face = new Vector2(0,1);
		Vector2 target = new Vector2(GameObject.Find("Target").transform.position.x - GameObject.Find("LabelCamera").transform.position.x, GameObject.Find("Target").transform.position.z - GameObject.Find("LabelCamera").transform.position.z);
		float angle = Vector2.SignedAngle(face, target);
		GameObject.Find("arrow").transform.localEulerAngles = new Vector3(90, -angle, 0);

	}
}
