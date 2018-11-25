using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {
	int quickSlotCount = 6;
	int packSlotCount = 12;
	int equipSlotCount = 4;
	bool isInventoryShown = false;
	float inputPrevious = 0;
	float input = 0;
	int inventoryGridWidth = 6;
	int slotWidth = 60; 
	GameObject inventoryOverlay;
	public GameObject underCursorDisplay;
	public Camera mainCamera;

	public Inventory playerInventory = null;
	public Inventory externalInventory = null;

	public Sprite[] itemIcons;
	public Sprite nullIcon;

	Vector3 quickSlotPosition;
	Vector3 packPosition;
	Vector3 weaponSlotPosition;
	Vector3 lightSlotPosition;
	Vector3 externalInventoryPanelPosition;


	void Awake(){
		itemIcons = Resources.LoadAll<Sprite>("Sprites/InventoryIcons");
		nullIcon = Resources.Load<Sprite>("Sprites/ui_null_icon");
		underCursorDisplay = transform.Find("UnderCursorDisplay").gameObject;

		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		float leftBound = -1*mainCamera.pixelWidth/2;
		Debug.Log(leftBound);
		quickSlotPosition = new Vector3(leftBound+32,-176, 0);
		packPosition = new Vector3(leftBound+32,-96, 0);
		weaponSlotPosition = new Vector3(leftBound+32,48, 0);
		lightSlotPosition = new Vector3(leftBound+32+60,48, 0);
		externalInventoryPanelPosition = new Vector3(32,-96, 0);

		GameObject quickSlotBar = Instantiate(Resources.Load<GameObject>("Prefabs/UI/InventoryPanel"));
		quickSlotBar.transform.SetParent(transform,false);
		quickSlotBar.transform.localPosition = quickSlotPosition;
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
			Destroy(inventoryOverlay);
		}else{
			inventoryOverlay = Instantiate(Resources.Load<GameObject>("Prefabs/UI/InventoryPanel"));
			inventoryOverlay.transform.SetParent(transform,false);
			inventoryOverlay.transform.localPosition = new Vector3(0,0,0);

			//Create Pack Panel
			GameObject packMenu = Instantiate(Resources.Load<GameObject>("Prefabs/UI/InventoryPanel"));
			packMenu.transform.SetParent(inventoryOverlay.transform,false);
			packMenu.transform.localPosition = packPosition;
			for(int i = 0; i < packSlotCount; i++){
				GameObject bumbel = Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIPackSlot"));
				bumbel.transform.SetParent(packMenu.transform,false);
				bumbel.transform.localPosition = new Vector3((i%inventoryGridWidth)*slotWidth, (i/inventoryGridWidth)*slotWidth, 0);
				bumbel.GetComponent<ItemSlot>().root = this;
				bumbel.GetComponent<ItemSlot>().index = i;
				

			}

			//Create Weapon Slot
			GameObject weaponSlot = Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIWeaponSlot"));
			weaponSlot.transform.SetParent(inventoryOverlay.transform,false);
			weaponSlot.transform.localPosition = weaponSlotPosition;

			//Create Light Slot
			GameObject lightSlot = Instantiate(Resources.Load<GameObject>("Prefabs/UI/UILightSlot"));
			lightSlot.transform.SetParent(inventoryOverlay.transform,false);
			lightSlot.transform.localPosition = lightSlotPosition;

			if(externalInventory != null){
				GameObject externalInventoryPanel = Instantiate(Resources.Load<GameObject>("Prefabs/UI/InventoryPanel"));
				externalInventoryPanel.transform.SetParent(inventoryOverlay.transform,false);
				externalInventoryPanel.transform.localPosition = externalInventoryPanelPosition;
				for(int i = 0; i < externalInventory.maxSize; i++){
					GameObject bumbel = Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIPackSlot"));
					bumbel.transform.SetParent(externalInventoryPanel.transform,false);
					bumbel.transform.localPosition = new Vector3((i%inventoryGridWidth)*slotWidth, (i/inventoryGridWidth)*slotWidth, 0);
				}
			}
		}
		isInventoryShown = !isInventoryShown;
	}


	void PackSlotClick(int index){

		Debug.Log(index);
	}
}
