using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler {
	public int index = 0;
	public UserInterface root = null;
	public Inventory targetInventory = null;
	GameObject img;

	void Awake(){
		img = transform.Find("Image").gameObject;
	}

	void Update(){
		if(targetInventory.Get(index) != null){
			ItemPrototype proto = targetInventory.Get(index).Prototype();
			img.GetComponent<Image>().sprite = root.itemIcons[proto.iconIndex];
		}else{
			img.GetComponent<Image>().sprite = root.nullIcon;
		}
	}

	public void OnPointerClick(PointerEventData eventData){
		UnderCursorDisplay cursorHandler = root.underCursorDisplay.GetComponent<UnderCursorDisplay>();
		if (eventData.button == PointerEventData.InputButton.Left){
			if(targetInventory.Get(index) != null){
				if(cursorHandler.heldItem == null){
					cursorHandler.heldItem = targetInventory.Get(index);
					targetInventory.Add(null,index);
				}else{
					cursorHandler.heldItem = targetInventory.Add(cursorHandler.heldItem,index);
				}
			}else{
				if(cursorHandler.heldItem != null){
					cursorHandler.heldItem = targetInventory.Add(cursorHandler.heldItem,index);
				}
			}
		}else if (eventData.button == PointerEventData.InputButton.Right){
			if(cursorHandler.heldItem != null){
				if(root.isInventoryShown){
					root.ToggleInventory();
				}
			}else{
				if(root.isInventoryShown){
					root.ToggleInventory();
				}
				if(targetInventory.Get(index) != null){
					targetInventory.Get(index).Use();
				}
			}
		}
	}
}