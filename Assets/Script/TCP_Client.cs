using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class TCP_Client {

    private TcpClient tcpClient;
    private Socket clientSocket;

    private string serverIP;
    private int serverPort;

    public TCP_Client(string serverIP, int serverPort)
    {
        this.serverIP = serverIP;
        this.serverPort = serverPort;
    }

    public bool ClientConnect()
    {
        try
        {
            // 建立 Client
            tcpClient = new TcpClient();

            // 測試連線是否存在 TimeOut 2 Second
            IAsyncResult result = tcpClient.BeginConnect(serverIP, serverPort, null, null);
            System.Threading.WaitHandle handler = result.AsyncWaitHandle;

            if (!result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2), false))
            {
                tcpClient.Close();
                tcpClient.EndConnect(result);
                handler.Close();
                throw new TimeoutException();
            }

            // 連線至 Server
            tcpClient.Connect(serverIP, serverPort);
            clientSocket = tcpClient.Client;

            Debug.Log("連線成功");

            return true;
        }
        catch
        {
            Debug.Log("連線失敗");
            return false;
        }
    }

    /*public bool ClientConnect()
    {
        try
        {
            //測試連線至遠端主機
            tcpClient = new TcpClient();
            tcpClient.Connect(serverIP, serverPort);
            clientSocket = tcpClient.Client;

            Debug.Log("連線成功");

            return true;
        }
        catch
        {
            Debug.Log("連線失敗");
            return false;
        }
    }*/

    //寫入資料
    public void WriteString(String message)
    {
        // 將字串轉 byte 陣列，使用 UTF8 編碼
        byte[] dataBufferBytes = Encoding.UTF8.GetBytes(message);

        Debug.Log("傳送字串: " + message);
        clientSocket.Send(dataBufferBytes);

        dataBufferBytes = null;
    }

    //讀取資料
    public String ReadString()
    {
        String message;
        int stringLength;
        int stringMaxLength = 1000;
        byte[] dataBufferBytes = new byte[stringMaxLength];

        stringLength = clientSocket.Receive(dataBufferBytes);

        Debug.Log("接收字串大小: " + stringLength);

        message = Encoding.UTF8.GetString(dataBufferBytes, 0, stringLength);

        dataBufferBytes = null;

        // 回傳字串
        return message;
    }

    // 接收小檔案
    public int ReceiveFile(String fileName)
    {
        int stringLength;
        int stringMaxLength = 4;
        byte[] dataBufferBytes = new byte[stringMaxLength];

        stringLength = clientSocket.Receive(dataBufferBytes, stringMaxLength, SocketFlags.None);

        int fileSize = BitConverter.ToInt32(dataBufferBytes, 0);

        Debug.Log("接收檔案大小:" + fileSize);

        if (fileSize == 0)
        {
            return 0;
        }

        byte[] fileBuffer = new byte[fileSize];
        int receiveBytesLength = 0;
        int receiveBytesTotalLength = 0;

        FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);

        while (receiveBytesTotalLength != fileSize)
        {
            receiveBytesLength = clientSocket.Receive(fileBuffer);
            fs.Write(fileBuffer, 0, receiveBytesLength);
            receiveBytesTotalLength += receiveBytesLength;
        }

        fs.Flush();
        fs.Close();

        //File.WriteAllBytes(fileName, fileBuffer);

        Debug.Log("寫入檔案: " + fileName);

        // 釋放記憶體
        dataBufferBytes = null;
        fileBuffer = null;

        return receiveBytesTotalLength;
    }

    // 傳送小檔案
    public int SendFile(String fileName)
    {
        if (File.Exists(fileName))
        {
            byte[] fileBuffer = File.ReadAllBytes(fileName);

            int fileSize = fileBuffer.Length;

            byte[] fileSizeBytes = BitConverter.GetBytes(fileSize);

            Debug.Log("傳送檔案大小: " + fileSize);

            clientSocket.Send(fileSizeBytes, fileSizeBytes.Length, 0);

            clientSocket.Send(fileBuffer, fileBuffer.Length, 0);

            Debug.Log("傳送檔案成功");

            fileBuffer = null;
            fileSizeBytes = null;

            return fileSize;
        }
        else
        {
            int fileSize = 0;
            byte[] fileSizeBytes = BitConverter.GetBytes(fileSize);

            Debug.Log("傳送檔案大小: " + fileSize + "不傳送");
            clientSocket.Send(fileSizeBytes, fileSizeBytes.Length, 0);

            fileSizeBytes = null;

            return 0;
        }
    }

    /*public int SendFile(String fileName)
    {
        if (File.Exists(fileName))
        {
            //byte[] fileBuffer = File.ReadAllBytes(fileName);

            //int fileSize = fileBuffer.Length;

            FileInfo fileInfo = new FileInfo(fileName);

            int fileSize = Convert.ToInt32(fileInfo.Length);

            byte[] fileSizeBytes = BitConverter.GetBytes(fileSize);

            Debug.Log("傳送檔案大小: " + fileSize);

            clientSocket.Send(fileSizeBytes, fileSizeBytes.Length, 0);

            byte[] fileBuffer;
            int needToSendBytes = 2048;
            int alreadySendBytes = 0;

            fileBuffer = new byte[needToSendBytes];

            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            while (alreadySendBytes != fileSize)
            {
                if(fileSize - alreadySendBytes >= needToSendBytes)
                {
                    fs.Read(fileBuffer, 0, needToSendBytes);
                    clientSocket.Send(fileBuffer, 0, needToSendBytes, SocketFlags.None);
                    alreadySendBytes += needToSendBytes;
                }
                else
                {
                    needToSendBytes = fileSize - alreadySendBytes;

                    fileBuffer = null;
                    fileBuffer = new byte[needToSendBytes];

                    fs.Read(fileBuffer, 0, needToSendBytes);
                    clientSocket.Send(fileBuffer, 0, needToSendBytes, SocketFlags.None);
                    alreadySendBytes += needToSendBytes;
                }

                Debug.Log("已經傳送的檔案大小: " + alreadySendBytes);
            }

            //clientSocket.Send(fileBuffer, fileBuffer.Length, 0);

            Debug.Log("傳送檔案成功");

            fileBuffer = null;
            fileSizeBytes = null;

            return alreadySendBytes;
        }

        return 0;
    }*/

    public void CloseClient()
    {
        clientSocket.Close();
        tcpClient.Close();
    }
}
