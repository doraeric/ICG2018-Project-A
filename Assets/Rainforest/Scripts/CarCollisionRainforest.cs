using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollisionRainforest : MonoBehaviour {

	public SpriteRenderer[] carSprites = new SpriteRenderer[3];
	private Color oriColor;

	// Use this for initialization
	void Start () {
		oriColor = carSprites[0].color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ChangeColor (Color c) {
		foreach (var spriterender in carSprites)
		{
				spriterender.color = c;
		}
	}
	void OnTrigerEnter2D (Collider2D other) {
		ChangeColor(Color.yellow);
	}
	void OnTrigerExit2D (Collider2D other) {
		ChangeColor(oriColor);
	}
	void OnCollisionEnter2D (Collision2D collisionInfo) {
		ChangeColor(Color.red);
		this.GetComponent<CarMotionSimulation> ().Stop();
	}
	void OnCollisionExit2D (Collision2D collisionInfo) {
		ChangeColor(oriColor);
	}



}
