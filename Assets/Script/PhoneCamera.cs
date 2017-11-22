using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour {

    // Gyro
    private Gyroscope gyro;
    private GameObject cameraContainer;
    private Quaternion rotation;

    // Camera
    private WebCamTexture cam;
    public RawImage background;
    public AspectRatioFitter fit;

    // Debug
    public Text debugText;

    private bool arReady = false;

    private void Start()
    {
        // Check if we support both service
        // Gyroscope
        if (!SystemInfo.supportsGyroscope)
        {
            Debug.Log("This device does not have Gyroscope");
            return;
        }

        // Back Camera
        for (int i = 0; i < WebCamTexture.devices.Length; i++)
        {
            if (!WebCamTexture.devices[i].isFrontFacing)
            {
                cam = new WebCamTexture(WebCamTexture.devices[i].name, Screen.width, Screen.height);
                break;
            }
        }

        // if we did not find a back camera, exit
        if (cam == null)
        {
            Debug.Log("This device does not have back Camera");
            return;
        }

        // Start Camera
        cam.Play();
        background.texture = cam;

        // Both services are supported, let's enable them!
        cameraContainer = new GameObject("Camera Container"); // Create with name
        cameraContainer.transform.position = transform.position;
        transform.SetParent(cameraContainer.transform);

        gyro = Input.gyro;
        gyro.enabled = true;

        cameraContainer.transform.rotation = Quaternion.Euler(90f, 0, 0);
        rotation = new Quaternion(0, 0, 1, 0);

        arReady = true;
    }

    private void Update()
    {
        if (arReady)
        {
            // Update Camera
            float ratio = (float)cam.width / (float)cam.height;
            fit.aspectRatio = ratio;

            // 1.0f : -1.0f 左右相反
            float scaleY = cam.videoVerticallyMirrored ? -1.0f : 1.0f;
            background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

            // 螢幕旋轉 0 (橫向), 90, 180, 270 (順時針轉動增加)
            int orient = -cam.videoRotationAngle;
            background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

            // Update Gyroscope
            transform.localRotation = gyro.attitude * rotation;

            if(debugText != null)
                debugText.text = cam.videoRotationAngle + " ; " + cam.videoVerticallyMirrored + " ; " + gyro.attitude.ToString();
        }
    }

}
