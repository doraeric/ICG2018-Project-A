using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoPark : MonoBehaviour {
	public GameObject wheel_FL;
	public GameObject wheel_FR;
	public GameObject carBody;
	public float wheelOmega;
	public float wheelDisance;
	public float acceleration;

	private Animator carAnimator;
	private float wheelAngel;
	private float velocity;
	private bool autoParking = false;
	const short FORWARD = 0, BACKWARD = 1, BRAKE = 2,
		RIGHT = 3, LEFT = 4, CENTER = 5;
	bool[] _move = {false, false, false, false, false, false};
	IEnumerator _Move(short[] acts, float time) {
		foreach(short act in acts)
			_move[act] = true;
		yield return new WaitForSeconds(time);
		foreach(short act in acts)
			_move[act] = false;
	}
	public IEnumerator Move(short act, float time) {
		yield return StartCoroutine(_Move(new short[]{act}, time));
	}
	public IEnumerator Move(short[] acts, float time) {
		yield return StartCoroutine(_Move(acts, time));
	}

	// Use this for initialization
	void Start () {
		carAnimator = GetComponent<Animator>();
	}

	IEnumerator defaultParking() {
		autoParking = true;
		yield return Move(FORWARD, 1.25f);
		yield return Move(BRAKE, 2f);
		yield return Move(RIGHT, 1f);
		yield return Move(BACKWARD, 1.45f);
		yield return Move(new short[]{BRAKE, CENTER}, 2f);
		yield return Move(BACKWARD, .6f);
		yield return Move(BRAKE, 1f);
		autoParking = false;
	}
	// Update is called once per frame
	void OnGUI() {
		GUI.Label(new Rect(10, 10, 150, 25),velocity.ToString());
		GUI.Label(new Rect(10, 30, 150, 25),autoParking.ToString());
		GUI.Label(new Rect(10, 50, 150, 25),wheelAngel.ToString());
	}
	void Update () {
		// if (!autoParking) {
		// 	if (Input.GetKeyDown(KeyCode.UpArrow))
		// 		_move[FORWARD] = true;
		// 	if (Input.GetKeyUp(KeyCode.UpArrow))
		// 		_move[FORWARD] = false;
		// 	if (Input.GetKeyDown(KeyCode.DownArrow))
		// 		_move[BACKWARD] = true;
		// 	if (Input.GetKeyUp(KeyCode.DownArrow))
		// 		_move[BACKWARD] = false;
		// 	if (Input.GetKeyDown(KeyCode.RightArrow))
		// 		_move[RIGHT] = true;
		// 	if (Input.GetKeyUp(KeyCode.RightArrow))
		// 		_move[RIGHT] = false;
		// 	if (Input.GetKeyDown(KeyCode.LeftArrow))
		// 		_move[LEFT] = true;
		// 	if (Input.GetKeyUp(KeyCode.LeftArrow))
		// 		_move[LEFT] = false;
		// 	if (Input.GetKeyDown(KeyCode.Space))
		// 		_move[BRAKE] = true;
		// 	if (Input.GetKeyUp(KeyCode.Space))
		// 		_move[BRAKE] = false;
		// 	if (Input.GetKeyDown(KeyCode.C))
		// 		_move[CENTER] = true;
		// 	if (Input.GetKeyUp(KeyCode.C))
		// 		_move[CENTER] = false;
		// }

		if (_move[FORWARD])
			velocity += acceleration * Time.deltaTime;
		if (_move[BACKWARD])
			velocity -= acceleration * Time.deltaTime;
		if (_move[RIGHT])
			wheelAngel -= wheelOmega * Time.deltaTime;
		if (_move[LEFT])
			wheelAngel += wheelOmega * Time.deltaTime;
		if (_move[BRAKE]) {
			if (velocity > acceleration / 2)
				velocity -= acceleration * Time.deltaTime;
			else if (velocity < -acceleration / 2)
				velocity += acceleration * Time.deltaTime;
			else
				velocity = 0;
		}
		if (_move[CENTER]) {
			if (wheelAngel > 5)
				wheelAngel -= wheelOmega * Time.deltaTime;
			else if (wheelAngel < -5)
				wheelAngel += wheelOmega * Time.deltaTime;
			else
				wheelAngel = 0;
		}

		if(Input.GetKeyDown(KeyCode.R)) {
			transform.position = new Vector3(-6, 0.8f, 0);
			transform.rotation = new Quaternion(0, 0, 0, 1);
		}
		if(Input.GetKeyDown(KeyCode.V) && !autoParking) {
			autoParking = true;
			StartCoroutine(defaultParking());
			autoParking = false;
		}

		if(wheelAngel < 0) {
			carAnimator.SetBool("turnLeft", false);
			carAnimator.SetBool("turnRight", true);
		} else if(wheelAngel > 0) {
			carAnimator.SetBool("turnLeft", true);
			carAnimator.SetBool("turnRight", false);
		} else {
			carAnimator.SetBool("turnLeft", false);
			carAnimator.SetBool("turnRight", false);
		}

		wheelAngel = Mathf.Clamp(wheelAngel, -45, 45);
		wheel_FL.transform.localRotation = Quaternion.Euler(0, 0, wheelAngel);
		wheel_FR.transform.localRotation = Quaternion.Euler(0, 0, wheelAngel);
		transform.Rotate(
			1 / wheelDisance * Mathf.Tan(wheelAngel * Mathf.PI / 180f) *
			velocity * Vector3.forward * Time.deltaTime * 180f / Mathf.PI);
		transform.Translate(Vector3.right * velocity * Time.deltaTime);
	}

	public void Stop() {
		velocity = 0;
	}
}
