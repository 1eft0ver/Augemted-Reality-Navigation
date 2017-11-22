using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    // 從 UI Scene 跳到 AR Scene
    public void uiToAR()
    {
        if (LabelMain.Instance.selectFileName != string.Empty)
        {
            Debug.Log("To AR Button");
            SceneManager.LoadScene("AR");
        }
    }

    // 從 AR Scene 跳到 SetLabel Scene
    public void arToSetLabel()
    {
        Debug.Log("To SetLabel Button");
        SceneManager.LoadScene("SetLabel");
    }

    // 從 SetLabel Scene 跳到 AR Scene
    public void setLabelToAR()
    {
        Debug.Log("To AR Button");
        SceneManager.LoadScene("AR");
    }

    // 從 SetLabel Scene 跳到 Map Scene
    public void setLabelToMap()
    {
        Debug.Log("To Map Button");
        SceneManager.LoadScene("GoogleMap");
    }

    // 從 Map Scene 跳到 SetLabel Scene
    public void mapToSetLabel()
    {
        Debug.Log("To SetLabel Button");
        SceneManager.LoadScene("SetLabel");
    }

    // 從 AR Scene 跳到 ChooseLabel Scene
    public void arToChooseLabel()
    {
        Debug.Log("To ChooseLabel Button");
        SceneManager.LoadScene("ChooseLabel");
    }

    // 從 ChooseLabel Scene 跳到 AR Scene
    public void chooseLabelToAR()
    {
        Debug.Log("To AR Button");
        SceneManager.LoadScene("AR");
    }

    // 從 ChooseLabel Scene 跳到 Map Scene (Choose)
    public void chooseLabelToMap()
    {
        Debug.Log("To MapChoose Button");
        SceneManager.LoadScene("GoogleMapChoose");
    }

    // 從 Map Scene (Choose) 跳到 ChooseLabel Scene
    public void mapToChooseLabel()
    {
        Debug.Log("To ChooseLabel Button");
        SceneManager.LoadScene("ChooseLabel");
    }
}
