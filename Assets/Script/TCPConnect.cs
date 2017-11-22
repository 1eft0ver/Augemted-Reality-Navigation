using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class TCPConnect : MonoBehaviour {

    //public static TCPConnect Instance { set; get; }

    TCP_Client tcpClient;
    String serverIP = "192.168.43.8";
    int serverPort = 3500;

    public Dropdown mapListDropDown;
    public List<string> mapNameList;
    public string selectMapName;
    public string uploadMapName;

    //public Text debugFileSizeText;

    public void Dropdown_IndexChanged(int index)
    {
        selectMapName = mapNameList[index];

        Debug.Log(selectMapName);
    }

    IEnumerator ShowAndHide( GameObject go, float delay ) {
        go.transform.Find("DownloadStatus").gameObject.SetActive(true);
        GameObject.Find("DownloadStatusText").GetComponent<Text>().text = "下載完成！";
        yield return new WaitForSeconds(delay);
        go.transform.Find("DownloadStatus").gameObject.SetActive(false);
    }
    private void Start()
    {
        selectMapName = "temp";
        uploadMapName = "temp";
        //Zip(selectMapName);
    }

    public void createMapList()
    {
        bool connectSuccess = false;
        mapNameList.Clear();
        mapListDropDown.ClearOptions();

        tcpClient = new TCP_Client(serverIP, serverPort);
        connectSuccess = tcpClient.ClientConnect();

        if (!connectSuccess) return;

        tcpClient.WriteString("List");

        string mapListString = tcpClient.ReadString();

        Debug.Log(mapListString);

        tcpClient.CloseClient();

        // 產生 Map List
        mapNameList.Clear();
        foreach (string mapName in mapListString.Split(' '))
        {
            mapNameList.Add(mapName);
        }

        if(!(selectMapName != mapNameList[0] && mapNameList.Contains(selectMapName)))
            selectMapName = mapNameList[0];

        // 建立 mapListDropDown
        mapListDropDown.AddOptions(mapNameList);
    }

    public void downloadMap()
    {
        int fileSize;

        bool connectSuccess = false;

        tcpClient = new TCP_Client(serverIP, serverPort);
        connectSuccess = tcpClient.ClientConnect();

        if (!connectSuccess) return;

        tcpClient.WriteString("Download " + selectMapName);
        fileSize = tcpClient.ReceiveFile(Application.persistentDataPath + "/" + selectMapName + ".zip");

        tcpClient.CloseClient();

        //debugFileSizeText.text = fileSize.ToString();

        Unzip();
        StartCoroutine( ShowAndHide(GameObject.Find("MenuMap"), 3.0f) ); // 1 second
    }

    public void uploadMap(Text FileName)
    {
        int fileSize;

        uploadMapName = FileName.text;

        Debug.Log("Upload " + uploadMapName);

        Zip(uploadMapName);

        bool connectSuccess = false;

        tcpClient = new TCP_Client(serverIP, serverPort);
        connectSuccess = tcpClient.ClientConnect();

        if (!connectSuccess) return;

        tcpClient.WriteString("Upload " + uploadMapName);

        fileSize = tcpClient.SendFile(Application.persistentDataPath + "/" + uploadMapName + ".zip");

        tcpClient.CloseClient();

        //debugFileSizeText.text = fileSize.ToString();

        //System.Diagnostics.Process.Start(Application.persistentDataPath);
    }

    public void Zip(string mapName)
    {
        string[] exportZip
            = new string[]{ Application.persistentDataPath + "/" + mapName + "/content.zip", Application.persistentDataPath + "/" + mapName + ".zip" };
        string[] fileDir
            = new string[]{ Application.persistentDataPath + "/" + mapName + "/content", Application.persistentDataPath + "/" + mapName };

        // 當 Map 的 Zip 存在，先刪除，在 Zip
        if(File.Exists(exportZip[1]))
        {
            File.Delete(exportZip[1]);
        }

        string[] files = Directory.GetFiles(fileDir[0]);

        for(int i= 0;i < files.Length; i++)
        {
            files[i] = files[i].Replace('\\', '/');
            Debug.Log(files[i]);
        }
            
        ZipUtil.Zip(exportZip[0], files);

        files = Directory.GetFiles(fileDir[1]);
        ZipUtil.Zip(exportZip[1], files);

        // 刪除資料夾中的 content.zip
        File.Delete(exportZip[0]);

        //System.Diagnostics.Process.Start(Application.persistentDataPath);
    }

    /*public void Unzip()
    {
        string[] zipfilePath
            = new string[] { Application.persistentDataPath + "/" + selectMapName + ".zip", Application.persistentDataPath + "/" + selectMapName + "/content.zip" };
        string[] exportLocation
            = new string[] { Application.persistentDataPath + "/" + selectMapName, Application.persistentDataPath + "/" + selectMapName + "/content" };

        if(Directory.Exists(exportLocation[0]))
            Directory.Delete(exportLocation[0], true);

        if (File.Exists(zipfilePath[0]) != false)
        {
            ZipUtil.Unzip(zipfilePath[0], exportLocation[0]);
            ZipUtil.Unzip(zipfilePath[1], exportLocation[1]);

            //File.Delete(zipfilePath[1]);
        }
        else
        {
            Debug.Log("File Not Found");
        }

        System.Diagnostics.Process.Start(Application.persistentDataPath);
    }*/

    public void Unzip()
    {
        string zipfilePath = Application.persistentDataPath + "/" + selectMapName + ".zip";
        string exportLocation = Application.persistentDataPath + "/" + selectMapName;

        if (File.Exists(zipfilePath))
        {
            // 當 UnZip 後的目錄存在，先刪除，在 UnZip
            if (Directory.Exists(exportLocation))
                Directory.Delete(exportLocation, true);
            ZipUtil.Unzip(zipfilePath, exportLocation);
            
            //System.Diagnostics.Process.Start(Path.GetDirectoryName(zipfilePath));
        }
    }
}
