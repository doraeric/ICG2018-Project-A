using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane1Behaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy(this.gameObject, 5f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.D)) {
			Destroy(this.gameObject);
		}
	}

	void OnGUI(){
		GUI.Label(new Rect(10, 10, 150, 25), "Destroy this");
		if (GUI.Button(new Rect(10, 35, 150, 25), "click me")) {
			Destroy(this.gameObject);
		}
	}
}
