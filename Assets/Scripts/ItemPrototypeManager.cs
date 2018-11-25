using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPrototypeManager : MonoBehaviour{
	public List<ItemPrototype> prototypes = new List<ItemPrototype>();
	public static ItemPrototypeManager Instance { get; private set; }

	void Awake (){
		//ensure uniqueness
		if(Instance != null && Instance != this){
			Destroy(gameObject);
		}
		Instance = this;
		DontDestroyOnLoad(transform.gameObject);

		DefinePrototypes();
	}

	void DefinePrototypes(){
		ItemPrototype bumbel;

		bumbel = new ItemPrototype();
		bumbel.name = "Oil";
		bumbel.iconIndex = 0;
		bumbel.maxStackCount = 10;
		bumbel.craftable = true;
		prototypes.Add(bumbel);

		bumbel = new ItemPrototype();
		bumbel.name = "Wood";
		bumbel.iconIndex = 1;
		bumbel.maxStackCount = 10;
		bumbel.craftable = true;
		prototypes.Add(bumbel);

	}
}

