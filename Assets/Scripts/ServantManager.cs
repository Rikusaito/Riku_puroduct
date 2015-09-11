using UnityEngine;
using System.Collections;


public class ServantManager : MonoBehaviour {

	private const float z = 0f;
	//for servant object
	public GameObject servant;
	//for servantSelect array
	public GameObject[] Servants = new GameObject[6];
	//for startPosition object
	private GameObject startPanel;
	public Vector2 startPos;
	//for servant parent object
	public GameObject mapObj;
	public GameObject frontObj;
	private PanelGen panelGen;
	//for enemy param script
	private CastleParam castleParam;

	//for servant select
	private ServantSpawn servantSpawn;
	private bool _isServantDecide = false;


	//for servantAnimation 
	private Animator servantAnim;
	//for servant appearTimer
	private float timer = 0;
	//for servant restart
	private float restartTimer = 0;

	//for moveFlag
	public bool _startPhase_1 = true;
	public bool _startPhase_2 = false;
	public bool _isMovingPhase_1 = false;
	public bool _isMovingPhase_2 = false;
	public bool _isPanelCenterMoving = false;
	public bool _isNextPanelMoving = false;
	public bool _isEndMoving = false;
	//for move variable
	private float totalMoveTime = 0;
	private float moveDuration = 2f;
	public float diffPosX;
	public float diffPosY;

	//liveFlag
	public bool _isLiving = true;
	//maploadFlag
	public bool _isLoading = false;

	//for routeJudg variable
	public int startGate;
	public int endGate = 0;
	//for goal variable
	private float xPos = 0;
	private float yPos = 1.0f;

	// Use this for initialization
	void Start () {
		startPanel = GameObject.FindGameObjectWithTag ("StartPanel");
		servantSpawn = GameObject.FindGameObjectWithTag ("ServantSpawn").GetComponent<ServantSpawn>();
		panelGen = GameObject.FindGameObjectWithTag ("Generator").GetComponent<PanelGen>();
		castleParam = GameObject.FindGameObjectWithTag ("CastleParam").GetComponent<CastleParam> ();
		startGate = 2;
	}
	
	// Update is called once per frame
	void Update () {
		if (_isLiving) {
			restartTimer = 0;
			if (_startPhase_1) {
				if (timer <= 2) {
					timer += Time.deltaTime;
				} else {
					_isServantDecide = ServantSelect ();
					if (_isServantDecide) {
						timer = 0;
						Debug.Log ("kitayo-");
						servant.transform.position = startPanel.transform.position;
						servant.transform.parent = startPanel.transform;
						startPos = startPanel.transform.position;
						mapObj = panelGen.GetMapObj (servant.transform.position.y + yPos, servant.transform.position.x);
						frontObj = mapObj;
						_startPhase_1 = false;
						_startPhase_2 = true;
					} else {
						return;
					}
				}
			} 

			if (_startPhase_2) {
				if (frontObj != mapObj) {
					mapObj = frontObj;
				}
				if (totalMoveTime < moveDuration) {
					totalMoveTime += Time.deltaTime; 
					float x = startPos.x + 0 * (totalMoveTime / moveDuration);
					float y = startPos.y + 0.5f * (totalMoveTime / moveDuration);
					servant.transform.position = new Vector2 (x, y);
				} else {
					mapObj = panelGen.GetMapObj (servant.transform.position.y + yPos, servant.transform.position.x);
					_startPhase_2 = false;
					totalMoveTime = 0;
					if (mapObj.tag != "Block") {
						if (mapObj.tag == "NullPanel") {
							ServantDie ();
						}
						if (mapObj.GetComponent<Panel> ().route [2]) {
							endGate = 0;
							_isMovingPhase_1 = true;
							_isPanelCenterMoving = true;
						} else {
							Debug.Log ("死んだ");
							ServantDie ();
							_isLiving = false;
						}
					} else {
						Debug.Log ("死んだ_D");
						ServantDie ();
						_isLiving = false;
					}
				}
			}

			if (_isMovingPhase_1) {
				if (_isPanelCenterMoving) {
//					startGate = endGate;
					ServantMoveFirst ();
					_isPanelCenterMoving = false;
				}

				if (totalMoveTime < moveDuration) {
					totalMoveTime += Time.deltaTime;
					float x = startPos.x + diffPosX * (totalMoveTime / moveDuration);
					float y = startPos.y + diffPosY * (totalMoveTime / moveDuration);
					servant.transform.position = new Vector2 (x, y);
				} else {
					servant.transform.position = new Vector2 (Mathf.Round (servant.transform.position.x), Mathf.Round (servant.transform.position.y));
					_isMovingPhase_1 = false;
					totalMoveTime = 0;
					frontObj = mapObj;
					startPos = servant.transform.position;
					_isMovingPhase_2 = true;
					_isNextPanelMoving = true;
				}
			}

			if (_isMovingPhase_2) {
				if (frontObj != mapObj) {
					mapObj = frontObj;
				}
				if (_isNextPanelMoving) {
					ServantMoveSecond ();
					_isNextPanelMoving = false;
				}
				if (totalMoveTime < moveDuration) {
					totalMoveTime += Time.deltaTime;
					float x = startPos.x + diffPosX * (totalMoveTime / moveDuration);
					float y = startPos.y + diffPosY * (totalMoveTime / moveDuration);
					servant.transform.position = new Vector2 (x, y);
				} else {
					if (mapObj.tag != ("NullPanel") || mapObj.tag != ("Block")) {
						totalMoveTime = 0;
						_isMovingPhase_1 = true;
						_isPanelCenterMoving = true;
						mapObj = panelGen.GetMapObj (Mathf.Floor(servant.transform.position.y) + yPos, Mathf.Floor(servant.transform.position.x) + xPos);						
						_isMovingPhase_2 = false;
					} else {
						Debug.Log ("死んだ_C");
						ServantDie ();
						_isLiving = false;
					}
				} 
			}
			if (_isEndMoving) {
				_isMovingPhase_1 = false;
				_isMovingPhase_2 = false;
				_isNextPanelMoving = false;
				_isPanelCenterMoving = false;
				if (totalMoveTime < moveDuration) {
					totalMoveTime += Time.deltaTime;
					float x = startPos.x + diffPosX * (totalMoveTime / moveDuration);
					float y = startPos.y + diffPosY * (totalMoveTime / moveDuration);
					servant.transform.position = new Vector2 (x, y);
				} else {
					Debug.Log ("城についた");
					_isEndMoving = false;
					ServantDie ();
					_isLiving = false;
				}
			}
		} else {
			if (restartTimer > 2f) {
				_isMovingPhase_1 = false;
				panelGen.MapDataSwitch ();
				restartTimer = 0;
				yPos = 1.0f;
				startGate = 2;
				_isLiving = true;
				_startPhase_1 = true;
				_isLoading = false;
			
			} else {
				restartTimer += Time.deltaTime;

			}
		}
	}

	void ServantMoveFirst (){
		if (mapObj.tag.Equals("NullPanel") || mapObj.tag.Equals("Block")) {
			Debug.Log ("死んだ_B");
			ServantDie ();
			_isLiving = false;
		} else {
			if (mapObj.tag.Equals ("Castle")) {
				_isEndMoving = true;
				castleParam.CastleParametor (servant);
			} else {
				if (mapObj.GetComponent<Panel> ().route [startGate]) {
					if (mapObj.tag.Equals ("Cross") || mapObj.tag.Equals ("Tate") || mapObj.tag.Equals ("Yoko")) {
						if (startGate < 2) {
							endGate = startGate + 2;
						} else {
							endGate = startGate - 2;
						}
					} else if (mapObj.tag.Equals ("Turn")) {
						for (int i = 0; i < 4; i++) {
							if (startGate != i && mapObj.GetComponent<Panel> ().route [i]) {
								endGate = i;
							}
						}		
					}

				} else {
					Debug.Log ("死んだ_A");
					ServantDie ();
					_isLiving = false;
				}
			}
			servant.transform.parent = mapObj.transform;
			startPos = servant.transform.position;
			diffPosX = 0;
			diffPosY = 0;
			diffPosX = mapObj.transform.position.x - startPos.x;
			diffPosY = mapObj.transform.position.y - startPos.y;
		}
	}

	void ServantMoveSecond (){
		diffPosX = 0;
		diffPosY = 0;
		if (endGate == 0) {
			xPos = 0;
			yPos = 1; 
			diffPosX = 0;
			diffPosY = 0.5f;
			servantAnim.SetBool ("_isFrontMove", true);
			servantAnim.SetBool ("_isLeftMove", false);
			servantAnim.SetBool ("_isRightMove", false);
			servantAnim.SetBool ("_isBackMove", false);
		} else if (endGate == 1) {
			xPos = 1;
			yPos = 0;
			diffPosX = 0.5f;
			diffPosY = 0;
			servantAnim.SetBool ("_isFrontMove", false);
			servantAnim.SetBool ("_isLeftMove", false);
			servantAnim.SetBool ("_isRightMove", true);
			servantAnim.SetBool ("_isBackMove", false);
		} else if (endGate == 2) {
			xPos = 0;
			yPos = 0;
			diffPosX = 0;
			diffPosY = -0.5f;
			servantAnim.SetBool ("_isFrontMove", false);
			servantAnim.SetBool ("_isLeftMove", false);
			servantAnim.SetBool ("_isRightMove", false);
			servantAnim.SetBool ("_isBackMove", true);
		} else if (endGate == 3) {
			xPos = 0;
			yPos = 0;
			diffPosX = -0.5f;
			diffPosY = 0;
			servantAnim.SetBool ("_isFrontMove", false);
			servantAnim.SetBool ("_isLeftMove", true);
			servantAnim.SetBool ("_isRightMove", false);
			servantAnim.SetBool ("_isBackMove", false);
		}
		if (mapObj.tag.Equals ("Turn")) {
			if (endGate < 2) {
				startGate = endGate + 2;
			} else {
				startGate = endGate - 2;
			}		
		}
	}

	bool ServantSelect (){
		bool _isServantSelect = false;
		int switchValue = Random.Range (0, 6);
		startPanel = GameObject.FindGameObjectWithTag ("StartPanel");
		servant = servantSpawn.GetServant ();
		if (servant != null) {
			if (switchValue < 3) {
				Debug.Log ("slime");
				moveDuration = 2f;
			} else if (switchValue > 2 && switchValue < 5) {
				Debug.Log ("bone");
				moveDuration = 1.5f;
			} else if (switchValue == 5) {
				Debug.Log ("dragon");
				moveDuration = 1f;
			}
			servantAnim = servant.GetComponent<Animator> ();
			_isServantSelect = true;
		}
		return _isServantSelect;
	}

	void PanelMove () {
		if (mapObj != null ) {
			frontObj = panelGen.GetMapObj (servant.transform.position.y + yPos, servant.transform.position.x + xPos);
		}
	}

	public void MapLoad () {
		_isLoading = true;
	}

	void ServantDie () {
		_isServantDecide = false;
		Destroy (servant.gameObject);
	}
}


//public class SampleSub {
//
//	public delegate void onComplete();
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//}

