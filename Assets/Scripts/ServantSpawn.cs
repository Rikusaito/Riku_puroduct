using UnityEngine;
using System.Collections;

public class ServantSpawn : MonoBehaviour {

	private const float z = -0.5f;

	public GameObject[] servants = new GameObject[3];
	private GameObject servant;
	private GameObject startPanel;
	private bool _isServantSelect = true;
	private int servantCount;

	// Use this for initialization
	void Start () {
		startPanel = GameObject.FindGameObjectWithTag ("StartPanel");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Mouse1) && _isServantSelect == true){
			AppaerServant ();
			_isServantSelect = false;
		}
	}

	private Vector3 MousePos () {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x, Input.mousePosition.y, z));
		Vector3 position = new Vector3 (Mathf.RoundToInt (mousePos.x), Mathf.RoundToInt (mousePos.y), z);
		return position;
	}

	void ServantSelect (float xPos){
		if (xPos == 2.0f) {
			servant = Instantiate(servants[0], new Vector3(startPanel.transform.position.x, startPanel.transform.position.y,z), new Quaternion(0,0,0,0))as GameObject;
		} else if (xPos == 3.0f){
			servant = Instantiate(servants[1], new Vector3(startPanel.transform.position.x, startPanel.transform.position.y,z), new Quaternion(0,0,0,0))as GameObject;
		} else if (xPos == 4.0f){
			servant = Instantiate(servants[2], new Vector3(startPanel.transform.position.x, startPanel.transform.position.y,z), new Quaternion(0,0,0,0))as GameObject;
		}
		servantCount += 1;
	}

	void AppaerServant(){
		Vector3 tap = MousePos ();
		Debug.Log ("tapPosition = " + tap);
		if (servantCount < 1) {
			ServantSelect (tap.x);
		}
	}

	void servantVanish (){
		_isServantSelect = true;
	}

	public GameObject GetServant (){
		if (_isServantSelect == true) {
			return servant;
		} else {
			return null;
		}
	}
}
