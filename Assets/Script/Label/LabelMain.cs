using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LabelMain : MonoBehaviour {

    public static LabelMain Instance { set; get; }

    // 最主要的資料集合 labelList
    public Dictionary<string, LabelNode> labelList;

    // 選擇地圖檔案
    public string selectFileName; // 選擇地圖名稱
    public bool selectFileNameChange; // 判斷選擇地圖是否改變
    public bool selectMapChosen;
    public bool distChanged;
    public string clickedLabel;

    // 給 labelCreate 用，判斷 Label 顯示的範圍
    public double labelDistanceLimit;
    // 給 labelSetting 用，顯示詳細資料的 Label
    public string selectedToLabelDetailContent;

    // 給 labelSetting 用，判斷選擇要編輯的 Label
    public string selectedToEditLabelDetail;

    private void Start()
    {
        Instance = this;

        // 建立全域 labelList
        labelList = new Dictionary<string, LabelNode>();

        // 初始化 labelDistanceLimit
        labelDistanceLimit = 10000000000;

        // 初始化 selectedToEditLabelDetail
        selectedToEditLabelDetail = string.Empty;

        // 預設檔名，防止使用者沒有出入檔名
        selectFileNameChange = false;
        selectMapChosen = false;
        distChanged = false;
        selectFileName = "temp";
        string dirPath = Application.persistentDataPath + "/" + selectFileName;
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        if (!Directory.Exists(dirPath + "/content"))
        {
            Directory.CreateDirectory(dirPath + "/content");
        }

        // 建立暫存檔，selectFileName = "temp"
        string nodePath = Application.persistentDataPath + "/" + selectFileName + "/" + "map.txt";
        string labelPath = Application.persistentDataPath + "/" + selectFileName + "/" + "tag.txt";

        StreamWriter nodeWriter = new StreamWriter(nodePath, false, Encoding.UTF8);
        StreamWriter labeWriter = new StreamWriter(labelPath, false, Encoding.UTF8);
        nodeWriter.Close();
        labeWriter.Close();

        // 全域變數建立完成，跳到 AR Scene
        SceneManager.LoadScene("AR");
    }

    // 利用 LoadTexture 讀取圖片，並將 Texture2D 轉成 Sprite
    public Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f)
    {
        // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference
        Sprite NewSprite = new Sprite();
        Texture2D SpriteTexture = LoadTexture(FilePath);
        NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit);

        return NewSprite;
    }

    // 讀取圖片，並轉成 Texture2D
    public Texture2D LoadTexture(string FilePath)
    {
        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails
        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        return null;                     // Return null if load failed
    }
}
