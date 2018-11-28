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
	GameObject root;
	public bool overUI = false;

	void Awake(){
		itemIcons = Resources.LoadAll<Sprite>("Sprites/InventoryIcons");
		heldItemImage = transform.Find("HeldItemImage").gameObject.GetComponent<Image>();
		nullIcon = Resources.Load<Sprite>("Sprites/ui_null_icon");
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
		PointerEventData ped = new PointerEventData(null);
		ped.position = Input.mousePosition;
		List<RaycastResult> results = new List<RaycastResult>();
		gr.Raycast(ped, results);

		return results.Count > 0;
	}

	public void DisplayConstructionPreview(){
		if(true){

		}
	}

}