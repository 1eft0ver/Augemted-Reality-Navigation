using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class mapData{
	public string nodeLat{ set; get; }
	public string nodeLong{ set; get; }
	public List<string> canGoTo{ set; get; }

	public mapData() {
		this.canGoTo = new List<string>();
	}
}

public class MapToDistanceByMe : MonoBehaviour {
	public static MapToDistanceByMe Instance { set; get; }
	public string currentPath;
	public string mapName;
	public string fullPath;
	public float curlongitude;
    public float curlatitude;
	public Dictionary<string, mapData> dataBase;

	public float Calc(float lat1, float lon1, float lat2, float lon2) {
        float R = 6371.137f; // Radius of earth in KM
        float dLat = lat2 * Mathf.PI / 180 - lat1 * Mathf.PI / 180;
        float dLon = lon2 * Mathf.PI / 180 - lon1 * Mathf.PI / 180;
        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
        Mathf.Cos(lat1 * Mathf.PI / 180) * Mathf.Cos(lat2 * Mathf.PI / 180) *
        Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);
        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        return (R * c * 1000f); // meters
    }
	public void MapToDistance() {
		
        dataBase = new Dictionary<string, mapData>();
		if(File.Exists(fullPath + "distance.txt")) {
            File.Delete(fullPath + "distance.txt");
        }
        if(File.Exists(fullPath + "nearistNode.txt")) {
            File.Delete(fullPath + "nearistNode.txt");
        }
        curlongitude = GPS.Instance.longitude;
        curlatitude = GPS.Instance.latitude;
		//curlongitude = 120.267313f;
        //curlatitude = 22.627271f;

		//讀取map.txt存放入dataBase
		using (StreamReader sreader = new StreamReader(fullPath + "map.txt", Encoding.UTF8)) {
            string[] lineSplite;
			string line;
                while ((line = sreader.ReadLine()) != null) {
					
                    if (line == "") break;

                    lineSplite = line.Split(' ');
					mapData temp = new mapData();
					temp.nodeLat = lineSplite[1];
					temp.nodeLong = lineSplite[2];
					for(int i = 3 ; i < lineSplite.Length ; ++i) {
						temp.canGoTo.Add(lineSplite[i]);
					}
					dataBase.Add(lineSplite[0], temp);
                }
            sreader.Close();
        }
		float shortest = 1000000000;
		string nearestNode = "NodeNotFound";
		foreach(KeyValuePair<string, mapData> temp in dataBase) {
			
			float tempDis = Calc(curlatitude, curlongitude, float.Parse(temp.Value.nodeLat), float.Parse(temp.Value.nodeLong));
			nearestNode = (shortest > tempDis) ? temp.Key : nearestNode;
			shortest = (shortest > tempDis) ? tempDis : shortest;

			if(temp.Value.canGoTo.Count > 0) {
				using (StreamWriter swriter = new StreamWriter(fullPath + "distance.txt", true, Encoding.UTF8))
            	{
					foreach(string arrivable in temp.Value.canGoTo) {
					//swriter.Write(dataBase[temp.Key].nodeLat + " " + dataBase[temp.Key].nodeLong + " " + dataBase[arrivable].nodeLat.ToString() + " " + dataBase[arrivable].nodeLong.ToString() + Environment.NewLine);	
					swriter.Write(temp.Key + " " + arrivable + " " + Calc(float.Parse(dataBase[temp.Key].nodeLat), float.Parse(dataBase[temp.Key].nodeLong), float.Parse(dataBase[arrivable].nodeLat), float.Parse(dataBase[arrivable].nodeLong)) + Environment.NewLine);
					}
					swriter.Close();
            	}
			}
		}
		using (StreamWriter swriter = new StreamWriter(fullPath + "nearistNode.txt", true, Encoding.UTF8)) {
			swriter.Write("目前位置 " + nearestNode + " " + shortest + Environment.NewLine);
			swriter.Close();
		}
	}
	void Start () {
		currentPath = Application.persistentDataPath;
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		mapName = LabelMain.Instance.selectFileName;
		fullPath = currentPath + "/" + mapName + "/";
	}
}
