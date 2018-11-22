using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bumbel : NetworkBehaviour {
	float speedX = 0;
	float speedY = 0;
	float frictionForceX = 50f;
	float frictionForceY = 50f;
	public float spd = 5f;
	public float dir = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		speedX = spd*Mathf.Cos((Mathf.PI*dir)/180);
		speedY = spd*Mathf.Sin((Mathf.PI*dir)/180);
		float damping = 1f;//1/((Mathf.Abs(speedY)/spd)+1); //vertical movement is twice as expensive
		speedX = speedX * damping * Time.deltaTime;
		speedY = speedY * damping * Time.deltaTime;

		Vector3 pos = transform.position;
		pos.x += speedX;
		pos.y += speedY;
		transform.position = pos;
	}
}