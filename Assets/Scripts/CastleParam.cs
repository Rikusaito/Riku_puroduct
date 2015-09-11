using UnityEngine;
using System.Collections;

public class CastleParam : MonoBehaviour {

	public int HP = 3;
	public int ASPD = 4;

	public bool _isCastleBreaking = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		if (_isCastleBreaking) {
//			
//		}
	}

	public void CastleParametor (GameObject obj){
		if (obj.name.Equals ("Slime")) {
			if (HP <= 0) {
				_isCastleBreaking = true;
			}
			HP -= 1;
		}
	}
}
