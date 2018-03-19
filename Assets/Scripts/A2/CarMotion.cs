using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMotion : MonoBehaviour {
	public GameObject wheel_FL;
	public GameObject wheel_FR;
	public GameObject carBody;
	public float wheelOmega;
	public float wheelDisance;
	public float acceleration;
	public float sec;

	private float wheelAngel;
	private float velocity;
	private bool autoParking = false;
	struct Move {
		public bool forward  ;
		public bool backword ;
		public bool brake    ;
		public bool right    ;
		public bool left     ;
		public bool center   ;
	};
	private Move move;

	// Use this for initialization
	void Start () {
		move = new Move();
		StartCoroutine(defaultParking());
	}
	
	IEnumerator defaultParking() {
		autoParking = true;
		move.forward = true;
		yield return new WaitForSeconds(1.2f);
		move.forward = false;
		move.brake = true;
		yield return new WaitForSeconds(2);
		move.brake = false;
		move.right = true;
		yield return new WaitForSeconds(1);
		move.right = false;
		move.backword = true;
		yield return new WaitForSeconds(sec);
		move.backword = false;
		move.brake = true;
		move.center = true;
		yield return new WaitForSeconds(2);
		move.brake = false;
		move.center = false;
		move.backword = true;
		yield return new WaitForSeconds(.6f);
		move.backword = false;
		move.brake = true;
		yield return new WaitForSeconds(1f);
		move.brake = false;
		autoParking = false;
	}
	// Update is called once per frame
	void OnGUI() {
		GUI.Label(new Rect(10, 10, 150, 25),velocity.ToString());
		GUI.Label(new Rect(10, 30, 150, 25),autoParking.ToString());
	}
	void Update () {
		if (!autoParking) {
			if (Input.GetKeyDown(KeyCode.UpArrow))
				move.forward = true;
			if (Input.GetKeyUp(KeyCode.UpArrow))
				move.forward = false;
			if (Input.GetKeyDown(KeyCode.DownArrow))
				move.backword = true;
			if (Input.GetKeyUp(KeyCode.DownArrow))
				move.backword = false;
			if (Input.GetKeyDown(KeyCode.RightArrow))
				move.right = true;
			if (Input.GetKeyUp(KeyCode.RightArrow))
				move.right = false;
			if (Input.GetKeyDown(KeyCode.LeftArrow))
				move.left = true;
			if (Input.GetKeyUp(KeyCode.LeftArrow))
				move.left = false;
			if (Input.GetKeyDown(KeyCode.Space))
				move.brake = true;
			if (Input.GetKeyUp(KeyCode.Space))
				move.brake = false;
		}

		if (move.forward)
			velocity += acceleration * Time.deltaTime;
		if (move.backword)
			velocity -= acceleration * Time.deltaTime;
		if (move.right)
			wheelAngel -= wheelOmega * Time.deltaTime;
		if (move.left)
			wheelAngel += wheelOmega * Time.deltaTime;
		if (move.brake) {
			if (velocity > acceleration / 2)
				velocity -= acceleration * Time.deltaTime;
			else if (velocity < -acceleration / 2)
				velocity += acceleration * Time.deltaTime;
			else
				velocity = 0;
		}
		if (move.center) {
			if (wheelAngel > 5)
				wheelAngel -= wheelOmega * Time.deltaTime;
			else if (wheelAngel < 5)
				wheelAngel += wheelOmega * Time.deltaTime;
			else
				wheelAngel = 0;
		}

		if(Input.GetKeyDown(KeyCode.V) && !autoParking) {
			autoParking = true;
			StartCoroutine(defaultParking());
			Debug.Log(autoParking);
		}


		wheelAngel = Mathf.Clamp(wheelAngel, -45, 45);
		wheel_FL.transform.localRotation = Quaternion.Euler(0, 0, wheelAngel);
		wheel_FR.transform.localRotation = Quaternion.Euler(0, 0, wheelAngel);
		this.transform.Rotate(
			1 / wheelDisance * Mathf.Tan(wheelAngel * Mathf.PI / 180f) *
			velocity * Vector3.forward * Time.deltaTime * 180f / Mathf.PI);
		this.transform.Translate(Vector3.right * velocity * Time.deltaTime);
	}

	public void stop() {
		velocity = 0;
	}
}
