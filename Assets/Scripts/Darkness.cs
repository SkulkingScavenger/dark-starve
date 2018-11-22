using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darkness : MonoBehaviour {
	SpriteRenderer baseSprite;
	SpriteRenderer resultSprite;
	void Awake(){
		baseSprite = transform.Find("BaseSprite").GetComponent<SpriteRenderer>();
		resultSprite = transform.Find("ResultSprite").GetComponent<SpriteRenderer>();

		resultSprite.sortingLayerName = "Characters";
		//resultSprite.sprite.texture = Resources.Load<Texture2D>("darkness_mat");
	}

	void Update(){

	}
}