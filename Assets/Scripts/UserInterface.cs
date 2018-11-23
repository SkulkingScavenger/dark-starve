using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviour {
	int quickSlotCount = 4;
	int inventorySlotCount = 16;
	bool isInventoryShown = false;
	float inputPrevious = 0;
	float input = 0;
	int inventoryGridWidth = 8;
	int slotWidth = 82;


	void Awake(){
		GameObject quickSlotBar = Instantiate(Resources.Load<GameObject>("Prefabs/UI/QuickSlotBar"));
		quickSlotBar.transform.SetParent(transform,false);

		for(int i = 0; i < quickSlotCount; i++){
			GameObject bumbel = Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIInventorySlot"));
			bumbel.transform.SetParent(quickSlotBar.transform,false);
			bumbel.transform.localPosition = new Vector3(i * slotWidth, 0, 0);
		}
	}

	void Update(){

		inputPrevious = input;
		input = Input.GetAxis("Inventory");
		if (input != 0 && inputPrevious == 0){
			ToggleInventory();
		} 
	}

	void ToggleInventory(){
		if (isInventoryShown){
			GameObject inventoryMenu = transform.Find("InventoryMenu(Clone)").gameObject;
			Destroy(inventoryMenu);
			GameObject quickSlotBar = Instantiate(Resources.Load<GameObject>("Prefabs/UI/QuickSlotBar"));
			quickSlotBar.transform.SetParent(transform,false);

			for(int i = 0; i < quickSlotCount; i++){
				GameObject bumbel = Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIInventorySlot"));
				bumbel.transform.SetParent(quickSlotBar.transform,false);
				bumbel.transform.localPosition = new Vector3(i * slotWidth, 0, 0);
			}
		}else{
			GameObject quickSlotBar = transform.Find("QuickSlotBar(Clone)").gameObject;
			Destroy(quickSlotBar);
			GameObject inventoryMenu = Instantiate(Resources.Load<GameObject>("Prefabs/UI/InventoryMenu"));
			inventoryMenu.transform.SetParent(transform,false);

			for(int i = 0; i < inventorySlotCount; i++){
				GameObject bumbel = Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIInventorySlot"));
				bumbel.transform.SetParent(inventoryMenu.transform,false);
				bumbel.transform.localPosition = new Vector3((i%inventoryGridWidth)*slotWidth + 64, (i/inventoryGridWidth)*slotWidth + 64, 0);
			}
		}

		isInventoryShown = !isInventoryShown;
	}
}

