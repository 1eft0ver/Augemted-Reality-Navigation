using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelCamera : MonoBehaviour {

    // Gyro
    private Gyroscope gyro;
    private GameObject cameraContainer;
    private Quaternion rotation;

    private bool gyroReady = false;

    private void Start()
    {
        // Check if we support both service
        // Gyroscope
        if (!SystemInfo.supportsGyroscope)
        {
            Debug.Log("This device does not have Gyroscope");
            //return;
        }

        // Both services are supported, let's enable them!
        cameraContainer = new GameObject("Camera Container 2"); // Create with name
        cameraContainer.transform.position = transform.position;
        transform.SetParent(cameraContainer.transform, true);
        transform.GetComponent<Camera>().depth = 5;

        gyro = Input.gyro;
        gyro.enabled = true;

        //cameraContainer.transform.rotation = Quaternion.Euler(0, 0, 0);
        cameraContainer.transform.rotation = Quaternion.Euler(90f, 0, 0);
        rotation = new Quaternion(0, 0, 1, 0);

        gyroReady = true;
    }

    private void Update()
    {
        if(gyroReady)
        {
            // Update Gyroscope
            transform.localRotation = gyro.attitude * rotation;
            //GameObject.Find("arrow").transform.localRotation =  gyro.attitude;
        }
    }
}
 