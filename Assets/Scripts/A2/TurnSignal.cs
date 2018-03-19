using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSignal : MonoBehaviour {
	private Animator carAnimator;

	// Use this for initialization
	void Start () {
		carAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Z)) {
			carAnimator.SetBool("turnLeft", true);
			carAnimator.SetBool("turnRight", false);
		}
		if (Input.GetKeyDown(KeyCode.X)) {
			carAnimator.SetBool("turnLeft", false);
			carAnimator.SetBool("turnRight", true);
		}
		if (Input.GetKeyDown(KeyCode.C)) {
			carAnimator.SetBool("turnLeft", false);
			carAnimator.SetBool("turnRight", false);
		}
	}
}
