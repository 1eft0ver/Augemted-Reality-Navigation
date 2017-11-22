using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unimgpicker : MonoBehaviour {

    public delegate void ImageDelegate(string path);

    public delegate void ErrorDelegate(string message);

    public event ImageDelegate Completed;

    public event ErrorDelegate Failed;

    private PickerAndroid picker = new PickerAndroid();

    public void Show(string title, string outputFileName, int maxSize)
    {
        picker.Show(title, outputFileName, maxSize);
    }

    private void OnComplete(string path)
    {
        var handler = Completed;
        if (handler != null)
        {
            handler(path);
        }
    }

    private void OnFailure(string message)
    {
        var handler = Failed;
        if (handler != null)
        {
            handler(message);
        }
    }
}
