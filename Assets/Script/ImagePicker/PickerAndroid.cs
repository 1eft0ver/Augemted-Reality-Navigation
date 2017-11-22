using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickerAndroid {

    private static readonly string PickerClass = "com.kakeragames.unimgpicker.Picker";

    public void Show(string title, string outputFileName, int maxSize)
    {
        using (AndroidJavaClass picker = new AndroidJavaClass(PickerClass))
        {
            picker.CallStatic("show", title, outputFileName, maxSize);
        }
    }
}
