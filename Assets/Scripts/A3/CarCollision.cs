using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollision : MonoBehaviour {
	public GameObject car;
	public GameObject wheelFL;
	public GameObject wheelFR;

	SpriteRenderer[] spriteRenderers = new SpriteRenderer[3];
	Color originColor;

	// Use this for initialization
	void Start () {
		spriteRenderers[0] = car.GetComponent<SpriteRenderer>();
		spriteRenderers[1] = wheelFR.GetComponent<SpriteRenderer>();
		spriteRenderers[2] = wheelFL.GetComponent<SpriteRenderer>();
		originColor = spriteRenderers[0].color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void colorChange(Color color) {
		foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
			spriteRenderer.color = color;
		}
	}

	void OnTriggerEnter2D(Collider2D collision) {
		colorChange(Color.yellow);
	}
	void OnTriggerExit2D(Collider2D collision) {
		colorChange(originColor);
	}

	void OnCollisionEnter2D(Collision2D collision) {
		this.GetComponent<CarMotion>().Stop();
		colorChange(Color.red);
	}
	void OnCollisionExit2D(Collision2D collision) {
		colorChange(originColor);
	}
}
