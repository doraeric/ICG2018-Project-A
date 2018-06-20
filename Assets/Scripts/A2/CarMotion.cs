using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMotion : MonoBehaviour {
#if UNITY_EDITOR
	const bool DEBUG = true;
#else
	const bool DEBUG = false;
#endif

	public GameObject wheel_FL;
	public GameObject wheel_FR;
	public GameObject carBody;
	public float wheelOmega;
	public float wheelDisance;
	public float acceleration;
	public float GetSpeed() { return velocity; }
	public bool lockInput = false;
	public float slowDownAcc;

	private Animator carAnimator;
	private float wheelAngle;
	private float velocity;
	private float _acceleration;
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
		UIManager.Instance.ShowPanel("HelpPanel");
		_acceleration = acceleration;
		StartCoroutine(CloseHelp());
	}
	IEnumerator CloseHelp() {
		yield return new WaitForSeconds(5.0f);
		UIManager.Instance.ClosePanel("HelpPanel");
	}

	IEnumerator defaultParking() {
		lockInput = true;
		yield return Move(FORWARD, 1.25f);
		yield return Move(BRAKE, 2f);
		yield return Move(RIGHT, 1f);
		yield return Move(BACKWARD, 1.45f);
		yield return Move(new short[]{BRAKE, CENTER}, 2f);
		yield return Move(BACKWARD, .6f);
		yield return Move(BRAKE, 1f);
		lockInput = false;
	}
	// Update is called once per frame
	void OnGUI() {
		if (DEBUG) {
			GUI.Label(new Rect(10, 10, 150, 25),velocity.ToString());
			GUI.Label(new Rect(10, 30, 150, 25),lockInput.ToString());
			GUI.Label(new Rect(10, 50, 150, 25),wheelAngle.ToString());
		}
	}
	void Common () {
		if (!_move[FORWARD] && !_move[BACKWARD]) {
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
		if (!_move[RIGHT] && !_move[LEFT]) {
			if (wheelAngle > 5)
				wheelAngle -= wheelOmega * Time.deltaTime;
			else if (wheelAngle < -5)
				wheelAngle += wheelOmega * Time.deltaTime;
			else
				wheelAngle = 0;
		}
	}
	void ResetMove() {
		_move[FORWARD] = false;
		_move[BACKWARD] = false;
		_move[BRAKE] = false;
		_move[RIGHT] = false;
		_move[LEFT] = false;
		_move[CENTER] = false;
		lockInput = false;
	}
	public void ResetCar() {
		ResetMove();
		transform.position = new Vector3(-6, 0.8f, 0);
		transform.rotation = new Quaternion(0, 0, 0, 1);
		velocity = 0;
		wheelAngle = 0;
		acceleration = _acceleration;
	}
	void Update () {
		if (!lockInput)
			Common();
		if (DEBUG && Input.GetKey(KeyCode.P)) {
			EasterEgg egg = GetComponent<EasterEgg>();
			egg.FindEgg();
		}
		if (!lockInput && Input.GetKeyDown(KeyCode.R)) {
			ResetCar();
		}

		if (!lockInput && Input.GetKeyDown(KeyCode.Z)) {
			StartCoroutine(defaultParking());
		}

		if (!lockInput) {
			if (Input.GetKeyDown(KeyCode.UpArrow))
				_move[FORWARD] = true;
			if (Input.GetKeyUp(KeyCode.UpArrow))
				_move[FORWARD] = false;
			if (Input.GetKeyDown(KeyCode.DownArrow))
				_move[BACKWARD] = true;
			if (Input.GetKeyUp(KeyCode.DownArrow))
				_move[BACKWARD] = false;
			if (Input.GetKeyDown(KeyCode.RightArrow))
				_move[RIGHT] = true;
			if (Input.GetKeyUp(KeyCode.RightArrow))
				_move[RIGHT] = false;
			if (Input.GetKeyDown(KeyCode.LeftArrow))
				_move[LEFT] = true;
			if (Input.GetKeyUp(KeyCode.LeftArrow))
				_move[LEFT] = false;
			if (Input.GetKeyDown(KeyCode.Space))
				_move[BRAKE] = true;
			if (Input.GetKeyUp(KeyCode.Space))
				_move[BRAKE] = false;
			if (Input.GetKeyDown(KeyCode.C))
				_move[CENTER] = true;
			if (Input.GetKeyUp(KeyCode.C))
				_move[CENTER] = false;
		}

		if (_move[FORWARD])
			velocity += acceleration * Time.deltaTime;
		if (_move[BACKWARD])
			velocity -= acceleration * Time.deltaTime;
		if (_move[RIGHT])
			wheelAngle -= wheelOmega * Time.deltaTime;
		if (_move[LEFT])
			wheelAngle += wheelOmega * Time.deltaTime;
		if (_move[BRAKE]) {
			if (velocity > acceleration / 2)
				velocity -= acceleration * Time.deltaTime;
			else if (velocity < -acceleration / 2)
				velocity += acceleration * Time.deltaTime;
			else
				velocity = 0;
		}
		if (_move[CENTER]) {
			if (wheelAngle > 5)
				wheelAngle -= wheelOmega * Time.deltaTime;
			else if (wheelAngle < -5)
				wheelAngle += wheelOmega * Time.deltaTime;
			else
				wheelAngle = 0;
		}

		if(wheelAngle < 0) {
			carAnimator.SetBool("turnLeft", false);
			carAnimator.SetBool("turnRight", true);
		} else if(wheelAngle > 0) {
			carAnimator.SetBool("turnLeft", true);
			carAnimator.SetBool("turnRight", false);
		} else {
			carAnimator.SetBool("turnLeft", false);
			carAnimator.SetBool("turnRight", false);
		}

		wheelAngle = Mathf.Clamp(wheelAngle, -45, 45);
		wheel_FL.transform.localRotation = Quaternion.Euler(0, 0, wheelAngle);
		wheel_FR.transform.localRotation = Quaternion.Euler(0, 0, wheelAngle);
		transform.Rotate(
			1 / wheelDisance * Mathf.Tan(wheelAngle * Mathf.PI / 180f) *
			velocity * Vector3.forward * Time.deltaTime * 180f / Mathf.PI);
		transform.Translate(Vector3.right * velocity * Time.deltaTime);
	}

	public void Stop() {
		velocity = 0;
	}
}
