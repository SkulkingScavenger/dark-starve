using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

	GameObject player = null;

	// Use this for initialization
	void Awake () {
		player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (player != null) {
			Vector3 v = new Vector3(player.transform.position.x, player.transform.position.y, -10);
			transform.position = v;
		}
	}
}
