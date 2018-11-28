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

	StructurePrototype structure = null;
	StructurePrototype structurePrevious = null;
	GameObject constructionPreview = null;
	int constructionPreviewCellWidth = 64;

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


		if(structure != structurePrevious){
			DisplayConstructionPreview();
		}
		structurePrevious = structure;
		if(constructionPreview != null){
			constructionPreview.transform.position = new Vector3(transform.position.x,transform.position.y,-8);
		}
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
		if(constructionPreview != null){
			Destroy(constructionPreview);
		}
		constructionPreview = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ConstructionPreview"));
		constructionPreview.GetComponent<SpriteRenderer>().sprite = structure.sprite;
		GameObject gridCell = constructionPreview.transform.Find("GridCell").gameObject;
		GameObject bumbel;
		for(int i=0;i<structure.size;i++){
			for(int j=0;j<structure.size;j++){
				bumbel = Instantiate(gridCell);
				bumbel.transform.SetParent(constructionPreview.transform);
				bumbel.transform.localPosition = new Vector3((Decimal.Divide(structure.size-1,2) + i)*constructionPreviewCellWidth, (Decimal.Divide(structure.size-1,2) + j)*constructionPreviewCellWidth,0);
			}
		}
		Destroy(gridCell);
	}

}