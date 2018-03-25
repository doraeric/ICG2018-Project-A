using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMotionSimulation : MonoBehaviour {

public GameObject wheel_FL;
public GameObject wheel_FR;
public GameObject car;

public float wheelOmega;
private float wheelAngle;
public float wheelDistance;
private float velocity;
public float acceleration;
public float slowDownAcc;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Common();
		goForward();
		goBackward();
		turnLeft();
		turnRight();

		Debug.Log (velocity);
		
		// Car Transform
		wheelAngle = Mathf.Clamp (wheelAngle, -45, 45);
		wheel_FL.transform.localRotation = Quaternion.Euler (0, 0, wheelAngle);
		wheel_FR.transform.localRotation = Quaternion.Euler (0, 0, wheelAngle);

		this.transform.Rotate (1 / wheelDistance * 
			Mathf.Tan (wheelAngle * Mathf.PI / 180f) * 
			velocity * Vector3.forward * Time.deltaTime * 180 / Mathf.PI);
		
		this.transform.Translate (Vector3.right * velocity * Time.deltaTime);
	}

	public void Stop () {
		velocity = 0;
	}

	void goForward () {
		if (Input.GetKey (KeyCode.UpArrow)) {
			velocity += acceleration * Time.deltaTime;
		}
	}
	void goBackward () {
		if (Input.GetKey (KeyCode.DownArrow)) {
			velocity -= acceleration * Time.deltaTime;
		}
	}
	void Common () {
		if (velocity!=0 && !Input.GetKey (KeyCode.UpArrow) && !Input.GetKey (KeyCode.DownArrow)) {
			if (velocity >0) {
				velocity -= slowDownAcc *Time.deltaTime;
				if (velocity < 0){
					velocity = 0;
				}
			} else {
				velocity += slowDownAcc *Time.deltaTime;
				if (velocity > 0){
					velocity = 0;
				}
			}
		}
	}
	void turnLeft () {
		if (Input.GetKey (KeyCode.LeftArrow)) {
			wheelAngle += wheelOmega * Time.deltaTime;
		}
		if (Input.GetKeyUp (KeyCode.LeftArrow)) {
			wheelAngle = 0;
		}
	}
	void turnRight () {
		if (Input.GetKey (KeyCode.RightArrow)) {
			wheelAngle -= wheelOmega * Time.deltaTime;
		}
		if (Input.GetKeyUp (KeyCode.RightArrow)) {
			wheelAngle = 0;
		}
	}
}
