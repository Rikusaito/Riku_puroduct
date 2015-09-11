using UnityEngine;
using System.Collections;

public class PanelGen : MonoBehaviour {

	private const float z = 0.5f;
	/************************************
	-ActivePanel-------------------
	0:NullPanel
	1:Horizontal
	2:Vartical
	3:TopAndRight
	4:TopAndLeft
	5:BottomAndRight
	6:BottomAndLeft
	7:Cross

	-FixPanel-----------------------
	B:BlockPanel
	S:StartPanel
	*************************************/
	public string[,] MapCode = new string[6, 6];
	public string[] MapData = new string[6];
	public GameObject[,] MapObj = new GameObject[6,6];
	public GameObject[] panels = new GameObject[11];
	public Vector3[] panelPos = new Vector3[36];

	private ServantManager SVManager;

	// Use this for initialization
	void Awake () {
		MapData[0] = "BSBBBB";
		MapData[1] = "B7120B";
		MapData[2] = "B5431B";
		MapData[3] = "B2714B";
		MapData[4] = "B1325B";
		MapData[5] = "BBBCBB";

		MapGenerator ();

	}

	void Start () {
		SVManager = GameObject.FindGameObjectWithTag ("ServantManager").GetComponent<ServantManager> ();
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void MapDataSwitch () {
		for (int i = 0; i < 6; i++) {
			for (int j = 0; j < 6; j++) {
				Destroy (MapObj [i, j].gameObject);	
			}
		}
		int SWValue = Random.Range (0, 4);
		switch (SWValue) {
		case 0:
			MapData[0] = "BBBBSB";
			MapData[1] = "B0317B";
			MapData[2] = "B6156B";
			MapData[3] = "B1247B";
			MapData[4] = "B3215B";
			MapData[5] = "BCBBBB";
			break;
		case 1:
			MapData[0] = "BBBSBB";
			MapData[1] = "B0163B";
			MapData[2] = "B1475B";
			MapData[3] = "B7312B";
			MapData[4] = "B5324B";
			MapData[5] = "BBBBCB";
			break;
		case 2:
			MapData[0] = "BBSBBB";
			MapData[1] = "B3670B";
			MapData[2] = "B1621B";
			MapData[3] = "B4375B";
			MapData[4] = "B2135B";
			MapData[5] = "BBCBBB";
			break;
		case 3:
			MapData [0] = "BSBBBB";
			MapData [1] = "B7120B";
			MapData [2] = "B5431B";
			MapData [3] = "B2714B";
			MapData [4] = "B1325B";
			MapData [5] = "BBBCBB";
			break;
		}

		MapGenerator ();
		SVManager.MapLoad ();
	}

	private void MapGenerator () {
		for (int i = 0; i < 6; i++) {
			for (int j = 0; j < 6; j++) {
				MapCode [i, j] = MapData [i].Substring (j, 1);

				switch (MapCode[i,j]) {
				case "B":
					panelPos [i * 6 + j] = new Vector3 (j, i, z);
					MapObj [i, j] = Instantiate (panels [8], new Vector3 (j, i, z), new Quaternion (0, 0, 0, 0)) as GameObject;
					break;
				case "S":
					panelPos [i * 6 + j] = new Vector3 (j, i, z);
					MapObj [i, j] = Instantiate (panels [9], new Vector3 (j, i, z), new Quaternion (0, 0, 0, 0))as GameObject;
					break;
				case "C":
					panelPos [i * 6 + j] = new Vector3 (j, i, z);
					MapObj [i, j] = Instantiate (panels [10], new Vector3 (j, i, 0.8f), new Quaternion (0, 0, 0, 0))as GameObject;
					break;
				case "0":
					panelPos [i * 6 + j] = new Vector3 (j, i, z);
					MapObj [i, j] = Instantiate (panels [0], new Vector3 (j, i, z), new Quaternion (0, 0, 0, 0))as GameObject;
					break;
				case "1":
					panelPos [i * 6 + j] = new Vector3 (j, i, z);
					MapObj [i, j] = Instantiate (panels [1], new Vector3 (j, i, z), new Quaternion (0, 0, 0, 0))as GameObject;
					break;
				case "2":
					panelPos [i * 6 + j] = new Vector3 (j, i, z);
					MapObj [i, j] = Instantiate (panels [2], new Vector3 (j, i, z), new Quaternion (0, 0, 0, 0))as GameObject;
					break;
				case"3":
					panelPos [i * 6 + j] = new Vector3 (j, i, z);
					MapObj [i, j] = Instantiate (panels [3], new Vector3 (j, i, z), new Quaternion (0, 0, 0, 0))as GameObject;
					break;
				case"4":
					panelPos [i * 6 + j] = new Vector3 (j, i, z);
					MapObj [i, j] = Instantiate (panels [4], new Vector3 (j, i, z), new Quaternion (0, 0, 0, 0))as GameObject;
					break;
				case"5":
					panelPos [i * 6 + j] = new Vector3 (j, i, z);
					MapObj [i, j] = Instantiate (panels [5], new Vector3 (j, i, z), new Quaternion (0, 0, 0, 0))as GameObject;
					break;
				case"6":
					panelPos [i * 6 + j] = new Vector3 (j, i, z);
					MapObj [i, j] = Instantiate (panels [6], new Vector3 (j, i, z), new Quaternion (0, 0, 0, 0))as GameObject;
					break;
				case"7":
					panelPos [i * 6 + j] = new Vector3 (j, i, z);
					MapObj [i, j] = Instantiate (panels [7], new Vector3 (j, i, z), new Quaternion (0, 0, 0, 0))as GameObject;
					break;
				default:

					break;
				}
			}
		}

	}

	public void MapObjRenew (float x, float y, GameObject obj){
		MapObj [(int)y, (int)x] = obj;
		for (int i = 0; i < 6; i++) {
			for (int j = 0; j < 6; j++) {
			}
		}
	}

	public GameObject GetMapObj (float x, float y) {
		return MapObj [(int)x, (int)y].gameObject;
	}

	public Vector3[] GetMapPos () {
		Vector3[] MapPos = panelPos;
		return MapPos;
	}
}

