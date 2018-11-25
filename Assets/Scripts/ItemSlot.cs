using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler {
	public int index = 0;
	public UserInterface root = null;
	GameObject img;

	void Awake(){
		img = transform.Find("Image").gameObject;
	}

	void Update(){
		if(root.playerInventory.Get(index) != null){
			ItemPrototype proto = root.playerInventory.Get(index).Prototype();
			img.GetComponent<Image>().sprite = root.itemIcons[proto.iconIndex];
		}else{
			img.GetComponent<Image>().sprite = root.nullIcon;
		}
	}

	public void OnPointerClick(PointerEventData eventData){
		if (eventData.button == PointerEventData.InputButton.Left){
			UnderCursorDisplay cursorHandler = root.underCursorDisplay.GetComponent<UnderCursorDisplay>();
			if(root.playerInventory.Get(index) != null){
				if(cursorHandler.heldItem == null){
					cursorHandler.heldItem = root.playerInventory.Get(index);
					root.playerInventory.Add(null,index);
				}else{
					cursorHandler.heldItem = root.playerInventory.Add(cursorHandler.heldItem,index);
				}
			}else{
				if(cursorHandler.heldItem != null){
					cursorHandler.heldItem = root.playerInventory.Add(cursorHandler.heldItem,index);
				}
			}
		}else if (eventData.button == PointerEventData.InputButton.Right){
			Debug.Log("Right click");
		}
	}
}