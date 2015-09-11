using UnityEngine;
using System.Collections;

public class PanelMoveController : MonoBehaviour {

	private const float z = 0.5f;

//	private Vector3[,] panelPos = new Vector3[6, 6];

	public Vector3[] provPanels = new Vector3[36];
	public PanelGen MapGen;
	public GameObject tapObj;
	public GameObject NullPanel;
	public Vector2 movingStartPos;
	public ServantManager SVManager;

	public bool _isMoving = false;
	public float diffPosX;
	public float diffPosY;
	public float totalMoveTime = 0f;
	public float moveDuration = 0.005f;
	// Use this for initialization
	void Start () {
		MapGen = GameObject.FindGameObjectWithTag ("Generator").GetComponent<PanelGen>();
		SVManager = GameObject.FindGameObjectWithTag ("ServantManager").GetComponent<ServantManager> ();
		totalMoveTime = 0f;

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) && _isMoving == false) {

			MovingPanel ();
		}

		if (_isMoving) {
			if (totalMoveTime < moveDuration) {
				totalMoveTime += Time.deltaTime;
				float x = movingStartPos.x + diffPosX * (totalMoveTime / moveDuration);
				float y = movingStartPos.y + diffPosY * (totalMoveTime / moveDuration);
				tapObj.transform.position = new Vector2 (x, y);
			} else {
				_isMoving = false;
				totalMoveTime = 0;
				tapObj.transform.position = new Vector2 (Mathf.Round (tapObj.transform.position.x), Mathf.Round (tapObj.transform.position.y));
				MapGen.MapObjRenew (tapObj.transform.position.x, tapObj.transform.position.y,tapObj);
				NullPanel.transform.position = movingStartPos;
				MapGen.MapObjRenew (NullPanel.transform.position.x, NullPanel.transform.position.y, NullPanel);
				//ここの文字列注意
				SVManager.SendMessage("PanelMove");
			}
		}
	}

	void MovingPanel (){
		Vector3 tap = Camera.main.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x, Input.mousePosition.y, z));
		tapObj = MapGen.GetMapObj (Mathf.Round(tap.y), Mathf.Round(tap.x));
		if (tapObj.tag != "Block" && tapObj.tag != "StartPanel") {
			movingStartPos = tapObj.transform.position;
			GameObject[] objSurroudings = new GameObject[4];
			objSurroudings [0] = MapGen.GetMapObj (tapObj.transform.position.y + 1, tapObj.transform.position.x);
			objSurroudings [1] = MapGen.GetMapObj (tapObj.transform.position.y, tapObj.transform.position.x + 1);
			objSurroudings [2] = MapGen.GetMapObj (tapObj.transform.position.y - 1, tapObj.transform.position.x);
			objSurroudings [3] = MapGen.GetMapObj (tapObj.transform.position.y, tapObj.transform.position.x - 1);
			for (int i = 0; i < 4; i++) {
				if (objSurroudings [i].tag.Equals ("NullPanel")) {
					NullPanel = objSurroudings [i].gameObject;
					diffPosX = NullPanel.transform.position.x - tapObj.transform.position.x;
					diffPosY = NullPanel.transform.position.y - tapObj.transform.position.y;
					_isMoving = true;
				} 
			}
		}
	}
}

