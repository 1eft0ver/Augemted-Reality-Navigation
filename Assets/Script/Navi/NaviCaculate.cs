using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class NaviCaculate : MonoBehaviour {
	//算經緯度距離 要用的rad
        private T[] SubArray<T>(T[] data, int index, int length)
	    {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
	    }
        private const double EARTH_RADIUS = 6378.137;
        static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }
    
        static int[] path_num;//node之間所存在的路徑數
        static double[] la1 = new double[1000];//node緯度數
        static double[] ln1 = new double[1000];//node經度數 
        
        static int noPath = 20000000;//表示不存在的路徑  
        static int MaxSize = 1000;
       
        static int[] path1;        
        static int location = 0, location1 = 0; //user的位置node
        static int destination = 0, destination1 = 0; //目標位置
        static int temp;
        static int[,] G;//轉為二維 表示nodeA 與 nodeB 路徑距離
        
		public void short_algo()
        {
            int node_num = 0;//node數有紀錄
            string[] xxxstring = new string[1000];//存經度字串陣列
            string[] xxxstring1 = new string[1000];//存緯度字串陣列
            int[] no_pathline = new int[1000]; //表示不存在的路徑陣列

			string line;  //讀取map.txt裡的字串
            string[] lineSplite;
			string currentPath = Application.persistentDataPath;
			string mapName = "導航測試";
			string fullPath = currentPath + "/" + mapName + "/";
            Debug.Log(fullPath + "num.txt");
			StreamReader sr = new StreamReader( fullPath + "num.txt", Encoding.UTF8);
			
            for (int i = 0; i < 1000; i++)//表示不存在的路徑陣列
            {
                no_pathline[i] = -1;
            }

            while ((line = sr.ReadLine()) != null) {

				if (line == "") break;
                
                lineSplite = line.Split(' ');
                xxxstring[node_num] = lineSplite[3];
                xxxstring1[node_num] = lineSplite[2];
                node_num++;
			}

            sr.Close();
            sr = new StreamReader( fullPath + "num.txt", Encoding.UTF8);

            //處理node與node之間二維關係 轉乘路徑一維陣列的方式
            
            for (int i = 0; i < node_num - 1; i++)
            {
                int j = i;
                int real_temp = 0;
                if (j != 0)
                {
                    while (j != 0)
                    {
                        real_temp += (node_num - j);
                        j--;
                    }
                }
                else { real_temp = 0; }
                //Console.Write(real_temp);
                //有num.txt檔就可以直接讀取相鄰節點的index
                while ((line = sr.ReadLine()) != null) {
                    if (line == "") break;
                    lineSplite = line.Split(' ');

                    int[] temp_path = new int[lineSplite.Length-4];
                    //temp_path = SubArray(Array.ConvertAll(lineSplite, int.Parse), 4, lineSplite.Length); 
                    for(int z = 4 ; z < lineSplite.Length ; ++z) {
                        temp_path[z-4] = Int32.Parse(lineSplite[z]);
                    }
                    foreach (int temp1 in temp_path)
                    {
                        int real_path = 0;
                        real_path = (temp1 - i) + real_temp;
                        no_pathline[real_path] = 1;
                        Debug.Log("no_pathline[" + real_path + "] = 1");
                    }
                }
            }            
            path_num = new int[1000];
            int k = 1;
            //算出各個間的node的距離
            for (int i = 0; i < node_num; i++)
            {
                for (int j = i + 1; j < node_num; j++)
                {
                    double radLat1 = rad(la1[i]);
                    double radLat2 = rad(la1[j]);
                    double a = radLat1 - radLat2;
                    double b = rad(ln1[i]) - rad(ln1[j]);
                    double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
                     Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
                    s = s * EARTH_RADIUS;
                    s = Math.Round(s * 10000) / 10;
                    path_num[k] = (int)s;
                    k++;
                }
            }
            G = new int[node_num, node_num];            
            int m = 1;
            for (int i = 0; i < node_num; i++)
            {
                for (int j = i; j < node_num; j++)
                {
                    if (i == j) //loop不是路徑
                        G[i, j] = noPath;
                    else if (no_pathline[m] == -1)//node間不存在的路徑
                    {
                        G[i, j] = noPath;
                        G[j, i] = noPath;
                        m++;
                    }
                    else
                    {
                        G[i, j] = path_num[m];//node間存在的路徑
                        G[j, i] = path_num[m];
                        m++;
                    }
                }
            }
            path1 = new int[node_num];
            //Console.WriteLine(node_num + "個節點號碼是0~" + (node_num - 1));

            //輸入起點位置
            //Console.Write("輸入User的位置節點:");
            int start = Int32.Parse(GameObject.Find("inputStartText").GetComponent<Text>().text);
            location = start;
            location1 = start;
            /*
            int[] input = Console.ReadLine().Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.Parse(x.Trim())).ToArray();
            foreach (int i in input)
            {
                location = i;
                location1 = i;
            }
            */
            int stop = Int32.Parse(GameObject.Find("inputStopText").GetComponent<Text>().text);
            destination = stop;
            destination1 = stop;
            //Console.WriteLine(location);
            //Console.Write("輸入目標位置節點:");
            //輸入目標位置
            /* 
            int[] input1 = Console.ReadLine().Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.Parse(x.Trim())).ToArray();
            foreach (int i in input1)
            {
                destination = i;
                destination1 = i;
            }
            */
            //Console.WriteLine(destination);
            //考慮順向還是逆向
            if (location > destination)
            {
                temp = location;
                location = destination;
                destination = temp;
            }
            //順向最短路徑演算法
            if (location1 <= destination1)
            {
                bool[] s = new bool[node_num]; //起始點與當前節點間的最短路徑
                int min;
                int curNode = 0;
                int[] dist = new int[node_num];
                int[] prev = new int[node_num];
                //初始化
                for (int v = 0; v < node_num; v++)
                {
                    s[v] = false;
                    dist[v] = G[location, v];
                    if (dist[v] > MaxSize)
                        prev[v] = 0;
                    else
                        prev[v] = location;
                }
                path1[0] = destination;
                dist[location] = 0;
                s[location] = true;
                for (int i = 0; i < node_num; i++)
                {
                    min = MaxSize;
                    for (int w = 0; w < node_num; w++)
                    {
                        if (!s[w] && dist[w] < min)
                        {
                            curNode = w;
                            min = dist[w];
                        }
                    }
                    s[curNode] = true;
                    for (int j = 0; j < node_num; j++)
                        if (!s[j] && min + G[curNode, j] < dist[j])
                        {
                            dist[j] = min + G[curNode, j];
                            prev[j] = curNode;
                        }
                }
                //輸出路徑節點
                int e = destination, step = 0;
                while (e != location)
                {
                    step++;
                    path1[step] = prev[e];
                    e = prev[e];
                }
                for (int i = step; i > step / 2; i--)
                {
                    int temp = path1[step - i];
                    path1[step - i] = path1[i];
                    path1[i] = temp;
                }
                int dist1 = dist[destination];
                //Console.Write("點" + location1 + "到點" + destination1 + "最短路徑:");
                Debug.Log("點" + location1 + "到點" + destination1 + "最短路徑:");
                for (int i = 0; i < path1.Length; i++)
                {
                    if (path1[i] == destination1)
                    {
                        Debug.Log(path1[i] + " ");
                        break;
                    }
                    else
                        Debug.Log(path1[i] + " ");
                }
                Debug.Log("長度:" + dist1);
            }
            //逆向最短路徑演算法
            else
            {               
                bool[] s = new bool[node_num]; //起始點與當前節點間的最短路徑
                int min;
                int curNode = 0;
                int[] dist = new int[node_num];
                int[] prev = new int[node_num];
                //初始化
                for (int v = 0; v < node_num; v++)
                {
                    s[v] = false;
                    dist[v] = G[location, v];
                    if (dist[v] > MaxSize)
                        prev[v] = 0;
                    else
                        prev[v] = location;
                }
                path1[0] = destination;
                dist[location] = 0;
                s[location] = true;
                for (int i = 0; i < node_num; i++)
                {
                    min = MaxSize;
                    for (int w = 0; w < node_num; w++)
                    {
                        if (!s[w] && dist[w] < min)
                        {
                            curNode = w;
                            min = dist[w];
                        }
                    }
                    s[curNode] = true;
                    for (int j = 0; j < node_num; j++)
                        if (!s[j] && min + G[curNode, j] < dist[j])
                        {
                            dist[j] = min + G[curNode, j];
                            prev[j] = curNode;
                        }
                }
                //輸出路徑節點
                int e = destination, step = 0;
                while (e != location)
                {
                    step++;
                    path1[step] = prev[e];
                    e = prev[e];
                }
                for (int i = step; i > step / 2; i--)
                {
                    int temp = path1[step - i];
                    path1[step - i] = path1[i];
                    path1[i] = temp;
                }
                int dist1 = dist[destination];
                int end_path = 0;
                Debug.Log("點" + location1 + "到點" + destination1 + "最短路徑:");
                int[] print_path = new int[node_num];
                for (int i = 0; i < path1.Length; i++)
                {
                    print_path[i] = path1[i];
                    if (path1[i] == location1)
                    {
                        end_path = i;
                        break;
                    }
                }
                for (int j = end_path; j >= 0; j--)
                {
                    Debug.Log(print_path[j] + " ");
                }
                Debug.Log("長度:" + dist1);
            }
        }
}
