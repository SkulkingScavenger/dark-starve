using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnderCursorDisplay : MonoBehaviour {
	public Item heldItem = null;
	Image heldItemImage;
	Sprite[] itemIcons;
	Sprite nullIcon;
	Camera cam = null;
	GameObject root;
	public bool overUI = false;

	void Awake(){
		itemIcons = Resources.LoadAll<Sprite>("Sprites/InventoryIcons");
		heldItemImage = transform.Find("HeldItemImage").gameObject.GetComponent<Image>();
		nullIcon = Resources.Load<Sprite>("Sprites/ui_null_icon");
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		root = GameObject.FindGameObjectWithTag("Canvas");
	}

	void Update(){
		transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
		transform.SetAsLastSibling();
		if(heldItem != null){
			heldItemImage.sprite = itemIcons[heldItem.Prototype().iconIndex];
			transform.Find("HeldItemImage").localPosition = new Vector3(0,0,0);
		}else{
			heldItemImage.sprite = nullIcon;
			transform.Find("HeldItemImage").localPosition = new Vector3(999,999,0);
		}
		overUI = CheckUICollision();
	}

	bool CheckUICollision(){
		GraphicRaycaster gr = root.GetComponent<GraphicRaycaster>();
		//Create the PointerEventData with null for the EventSystem
		PointerEventData ped = new PointerEventData(null);
		//Set required parameters, in this case, mouse position
		ped.position = Input.mousePosition;
		//Create list to receive all results
		List<RaycastResult> results = new List<RaycastResult>();
		//Raycast it
		gr.Raycast(ped, results);

		return results.Count > 0;
	}



}