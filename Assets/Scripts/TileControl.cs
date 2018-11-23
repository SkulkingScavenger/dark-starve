using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileControl : MonoBehaviour {
	int width = 100;
	int height = 100;
		void Awake(){
		for(int i = 0; i < width; i++){
			for(int j = 0; j < height; j++){
			GameObject bumbel = Instantiate(Resources.Load<GameObject>("Prefabs/Terrain Tile"));
			bumbel.transform.position = new Vector3(i - width/2, j - height/2, 0);
		}
		}
	}

	void Update(){

	}
}