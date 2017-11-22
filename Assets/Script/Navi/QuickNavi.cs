using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

class QuickGraph
{
    Dictionary<string, Dictionary<string, float>> vertices = new Dictionary<string, Dictionary<string, float>>();
    public void add_vertex(string name, Dictionary<string, float> edges)
    {
        vertices[name] = edges;
    }

    public List<string> shortest_path(string start, string finish)
    {
        var previous = new Dictionary<string, string>();
        var distances = new Dictionary<string, float>();
        var nodes = new List<string>();

        List<string> path = null;

        foreach (var vertex in vertices)
        {
            if (vertex.Key == start)
            {
                distances[vertex.Key] = 0;
            }
            else
            {
            distances[vertex.Key] = float.MaxValue;
            }

            nodes.Add(vertex.Key);
        }

        while (nodes.Count != 0)
        {
            nodes.Sort((x,y) => distances[x].CompareTo(distances[y]));
				
            var smallest = nodes[0];
            nodes.Remove(smallest);

            if (smallest == finish)
            {
                path = new List<string>();
                while (previous.ContainsKey(smallest))
                {
                    path.Add(smallest);
                    smallest = previous[smallest];
                }

                break;
            }

            if (distances[smallest] == float.MaxValue)
            {
                break;
            }

            foreach (var neighbor in vertices[smallest])
            {
                var alt = distances[smallest] + neighbor.Value;
                if (alt < distances[neighbor.Key])
                {
                    distances[neighbor.Key] = alt;
                    previous[neighbor.Key] = smallest;
                }
            }
        }

        return path;
    }
}
        
public class QuickNavi : MonoBehaviour {
	public static bool inNavi;
	private readonly float maxDistance = 1000f;
    private float targetPositionX, targetPositionY, targetPositionZ;
	public List<string> result;
	public int currentStep = 0;
	private GameObject ArrowPointTo;
    private GameObject Arrow;
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
	public void createGraphAndNavi() {
        QuickGraph g = new QuickGraph();
		string currentPath = Application.persistentDataPath;
		string mapName = LabelMain.Instance.selectFileName;
		string fullPath = currentPath + "/" + mapName + "/";
		string line;
		
		using (StreamReader sreader = new StreamReader(fullPath + "distance.txt", Encoding.UTF8)) {
			Dictionary<string, Dictionary<string, float>> buffer = new Dictionary<string, Dictionary<string, float>>();
			while ((line = sreader.ReadLine()) != null) {
				if (line == "") break;
				
				string[] lineSplite;
				lineSplite = line.Split(' ');
				
				if(buffer.ContainsKey(lineSplite[0])) {
					buffer[lineSplite[0]].Add(lineSplite[1], float.Parse(lineSplite[2]));
				}
				else {
					buffer.Add(lineSplite[0], new Dictionary<string, float>() 
														{
															{lineSplite[1], 
															float.Parse(lineSplite[2])
															}
														}
							);
				}

				if(buffer.ContainsKey(lineSplite[1])) {
					buffer[lineSplite[1]].Add(lineSplite[0], float.Parse(lineSplite[2]));
				}
				else {
					buffer.Add(lineSplite[1], new Dictionary<string, float>() 
														{
															{lineSplite[0], 
															float.Parse(lineSplite[2])
															}
														}
							);
				}
        	}
			sreader.Close();

			foreach (var temp in buffer) {
            //Debug.Log("Load: " + temp);
            g.add_vertex(temp.Key, temp.Value);
        	}
		

			string target = this.transform.parent.name;
			string start = "start point not found", dest = "dest point not found";
			float total = 0;
			string[] destSplite;
			string[] startSplite;
			dest = LabelMain.Instance.clickedLabel;
			using (StreamReader sr = new StreamReader(fullPath + "nearistNode.txt", Encoding.UTF8)) {
				line = sr.ReadLine();
				sr.Close();
				if (line != "") {
				startSplite = line.Split(' ');
				start = startSplite[1];
					if(start != dest) {
					result = g.shortest_path(start, dest);
					currentStep = result.Count ;
					Debug.Log("CurStep: " + currentStep);
					Debug.Log("Navi from " + start + " to " + dest);
					//Debug.Log("要走幾步:" + result.Count);
					for (int i = 0 ; i < result.Count - 1 ; ++i) {
						total += buffer[result[i]][result[i+1]];
						//Debug.Log(result[i]);
					}
						total += buffer[result[result.Count - 1]][start];
					//Debug.Log(result[result.Count - 1]);
					//Debug.Log(start);
					Debug.Log("途經 " + total + "公尺");
					}
					else {
					Debug.Log("已到達目的地!");
					}
				}
				
			}
			
		}
		
		
		
	}

	// Use this for initialization
	void Start () {
        ArrowPointTo = GameObject.Find("Target");
        Arrow = GameObject.Find("arrow");
        inNavi = false;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("CurStep: " + currentStep);
		if(currentStep > 0) {
			inNavi = true;
            GameObject.Find("arrow").transform.Find("default").gameObject.SetActive(true);
            GameObject.Find("NaviBackground").GetComponent<RawImage>().enabled = true;
            GameObject.Find("NaviHint").GetComponent<Text>().enabled = true;
			Arrow.transform.Find("default").gameObject.SetActive(true);
            float targetLatitude;
            float targetLongitude;
            float latitude = GPS.Instance.latitude;
            float longitude = GPS.Instance.longitude;
			float vectorDistance;
            Dictionary<string, LabelNode> labelList = LabelMain.Instance.labelList;
            
            
            targetLatitude = labelList[result[currentStep-1]].labelLatitude;
            targetLongitude = labelList[result[currentStep-1]].labelLongitude;
			vectorDistance = Mathf.Sqrt(Mathf.Pow(targetLatitude - latitude, 2) + Mathf.Pow(targetLongitude - longitude, 2));

            // 經度-X, 緯度-Z
            targetPositionX = -1 * (maxDistance * (targetLongitude - longitude)) / vectorDistance;
            targetPositionY = 0f;
            targetPositionZ = -1 * (maxDistance * (targetLatitude - latitude)) / vectorDistance;

            ArrowPointTo.transform.position = new Vector3(targetPositionX, targetPositionY, targetPositionZ);

			GameObject.Find("NaviHint").GetComponent<Text>().text = "目前導航目標：" + Environment.NewLine + result[currentStep-1];
            if(Calc(latitude, longitude, targetLatitude, targetLongitude) < 30 && currentStep-1 > 0) {
                --currentStep;
				GameObject.Find("NaviHint").GetComponent<Text>().text = "目前導航目標：" + result[currentStep-1];
            }
            else if (Calc(latitude, longitude, targetLatitude, targetLongitude) < 30 && currentStep-1 == 0) {
                Arrow.transform.Find("default").gameObject.SetActive(false);
				GameObject.Find("NaviHint").GetComponent<Text>().text = "已抵達終點： " + result[currentStep-1];
				inNavi = false;
				GameObject.Find("arrow").transform.Find("default").gameObject.SetActive(false);
            	GameObject.Find("NaviBackground").GetComponent<RawImage>().enabled = false;
            	GameObject.Find("NaviHint").GetComponent<Text>().enabled = false;
            }

		}
		/* 
		else if(currentStep > 0 && onceAfterStep == true) {
			onceAfterStep = false;
			
			Vector2 face = new Vector2(0,1);
			Vector2 target = new Vector2(GameObject.Find("Target").transform.position.x - GameObject.Find("LabelCamera").transform.position.x, GameObject.Find("Target").transform.position.z - GameObject.Find("LabelCamera").transform.position.z);
			float angle = Vector2.SignedAngle(face, target);
			GameObject.Find("arrow").transform.localEulerAngles = new Vector3(65, -angle, 0);
			
		}
		*/
	}
}
