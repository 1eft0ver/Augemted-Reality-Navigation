using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LabelToggleClick : MonoBehaviour {

    public void editLabelDetail()
    {
        Debug.Log("editLabelDetail");

        LabelMain.Instance.selectedToEditLabelDetail = gameObject.name;
        Debug.Log(LabelMain.Instance.selectedToEditLabelDetail);

        // 從 SetLabel Scene 跳到 EditLabel Scene
        SceneManager.LoadScene("EditLabel");
    }
}
