using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloWorld : MonoBehaviour {
	public GameObject trunk;
	public KeyCode key;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(key)) {
			Instantiate(trunk, Random.insideUnitCircle * 3, Quaternion.identity);
		}
	}
}
