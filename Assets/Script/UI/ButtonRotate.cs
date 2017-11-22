using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRotate : MonoBehaviour {
	public float smooth = 1f;
	private Vector3 targetAngles;
	public static bool rotating;
	public static int rotateState;
	// Use this for initialization
	void Start () {
		rotating = false;
		rotateState = 0;
	}
	
	// Update is called once per frame
	void Update () {

		if (rotating && rotateState == 0) {
			Vector3 to = new Vector3(0, 0, 180);
			if (Vector3.Distance(transform.eulerAngles, to) > 0.01f) {
				transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, to, 15 *Time.deltaTime);
			}
			else {
				transform.eulerAngles = to;
				rotating = false;
				rotateState = 1;
			}
		}
		else if (rotating && rotateState == 1) {
			Vector3 to = new Vector3(0, 0, 360);
			if (Vector3.Distance(transform.eulerAngles, to) > 0.01f) {
				transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, to, 15 * Time.deltaTime);
			}
			else {
				transform.eulerAngles = to;
				rotating = false;
				rotateState = 0;
			}
		}
	}
}
