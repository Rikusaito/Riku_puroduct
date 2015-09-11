using UnityEngine;
using System.Collections;

public class PlayerParam : MonoBehaviour {

	public int HP;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GetDamage (){
		if (HP > 0) {
			HP -= 1;
		} else {
			
		}
	}

	void GameOver (){
		Debug.Log ("gameover");
	}
}
