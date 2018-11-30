using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

	float speedX = 0;
	float speedY = 0;
	float frictionForceX = 200f;
	float frictionForceY = 200f;
	float baseSpeed = 200f;
	float faceDirection = 0.0f;
	float movementDirection = 0.0f;

	float chargeTimeRaw;
	public Camera mainCamera;
	public Inventory backpack = new Inventory(12);
	public Inventory quickSlot = new Inventory(6);
	public Inventory weaponSlot = new Inventory(1);
	public Inventory lightSlot = new Inventory(1);
	public Inventory trinketSlot = new Inventory(4);
	public UnderCursorDisplay cursorHandler;
	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		mainCamera.GetComponent<MainCamera>().player = gameObject;
		transform.position = new Vector3(transform.position.x,transform.position.y,-1);

		Item bumbel;
		bumbel = new Item();
		bumbel.itemId = 0;
		backpack.Add(bumbel);

		bumbel = new Item();
		bumbel.itemId = 1;
		backpack.Add(bumbel);

		bumbel = new Item();
		bumbel.itemId = 2;
		backpack.Add(bumbel,2);

		GameObject.FindGameObjectWithTag("Canvas").GetComponent<UserInterface>().Init(this);
		cursorHandler = GameObject.FindGameObjectWithTag("UnderCursorDisplay").GetComponent<UnderCursorDisplay>();
	}
	
	// Update is called once per framed
	void Update () {
		SetFaceDirection();
		// if (Input.GetAxis("Horizontal")!=0){
		// 	float x = transform.position.x;
		// 	x += 0.5f * Input.GetAxis("Horizontal");
		// 	transform.position=new Vector3(x, transform.position.y, 0);
		// }
		// if (Input.GetAxis("Vertical")!=0){
		// 	float y = transform.position.y;
		// 	y += 0.5f * Input.GetAxis("Vertical");
		// 	transform.position=new Vector3(transform.position.x, y, 0);
		// }
		// float speedX = Input.GetAxis("Horizontal");
		// float speedY = Input.GetAxis("Vertical");
		// GetComponent<Rigidbody2D>().velocity = new Vector2(speedX, speedY);
		if(!isLocalPlayer){return;}
		KeyboardMovement();
		Action();
	}
	
// 	void MouseMovement(){
// 	float spd = 100f;
// 	float speedInitialX = speedX;
// 	float speedInitialY = speedY;
// 	if(Input.GetAxis("MouseRight") != 0){
// 		Vector3 mouseCoordinates = Camera.main.ScreenToWorldPoint(Input.mousePosition);
// 		Vector2 start = new Vector2(transform.position.x,transform.position.y);
// 		Vector2 end = new Vector2(mouseCoordinates.x,mouseCoordinates.y);
// 		float d = Vector2.Distance(start,end);
		

// 		if(d > 0.1){
// 			float angle = Mathf.Asin((end.y-start.y)/d);
// 			if (start.y <= end.y && start.x <= end.x){
// 				//nothing
// 			}else if (start.y <= end.y && start.x > end.x){
// 				angle = Mathf.PI - angle; 
// 			}else if (start.y > end.y && start.x > end.x){
// 				angle = Mathf.PI - angle; 
// 			}else if (start.y > end.y && start.x <= end.x){
// 				angle = 2*Mathf.PI + angle; 
// 			}

// 			speedX = spd*Mathf.Cos(angle);
// 			speedY = spd*Mathf.Sin(angle);
// 			float damping = 1/((Mathf.Abs(speedY)/spd)+1); //vertical movement is twice as expensive
// 			speedX = speedX * damping * Time.deltaTime;
// 			speedY = speedY * damping * Time.deltaTime;
// 		}
// 	}else{
// 		speedX -=  Mathf.Sign(speedX) * frictionForceX * Time.deltaTime;
// 		if(Mathf.Sign(speedX) != Mathf.Sign(speedInitialX)){
// 			speedX = 0;
// 		}
// 		speedY -=  Mathf.Sign(speedY) * frictionForceY * Time.deltaTime;
// 		if(Mathf.Sign(speedY) != Mathf.Sign(speedInitialY)){
// 			speedY = 0;
// 		}
// 	}
// 	if(speedX!=0){
// 		Vector3 theScale = transform.localScale;
// 		theScale.x = Mathf.Sign(speedX);
// 		transform.localScale = theScale;
// 	}
// 	GetComponent<Rigidbody2D>().velocity = new Vector2(speedX,speedY);
// }

void SetFaceDirection(){
	Camera cam = Camera.main;//GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	Vector3 mouseCoordinates = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
	Vector2 start = new Vector2(transform.position.x,transform.position.y);
	Vector2 end = new Vector2(mouseCoordinates.x,mouseCoordinates.y);
	float d = Vector2.Distance(start,end);

	if(d > 0.1){
		float angle = Mathf.Asin((end.y-start.y)/d);
		if (start.y <= end.y && start.x <= end.x){
			//nothing
		}else if (start.y <= end.y && start.x > end.x){
			angle = Mathf.PI - angle; 
		}else if (start.y > end.y && start.x > end.x){
			angle = Mathf.PI - angle; 
		}else if (start.y > end.y && start.x <= end.x){
			angle = 2*Mathf.PI + angle; 
		}

	faceDirection = angle/(Mathf.PI/180);
	transform.eulerAngles = new Vector3(0,0,faceDirection);
	//transform.Rotate(rotation);
	}
}

void KeyboardMovement(){
	float inputX = Input.GetAxis("Horizontal");
	float inputY = Input.GetAxis("Vertical");
	
	if(inputX > 0){
		if(inputY < 0){
			movementDirection = 315;
		}else if(inputY > 0){
			movementDirection = 45;
		}else{
			movementDirection = 0;
		}
	}else if(inputX < 0){
		if(inputY < 0){
			movementDirection = 225;
		}else if(inputY > 0){
			movementDirection = 135;
		}else{
			movementDirection = 180;
		}
	}else{
		if(inputY < 0){
			movementDirection = 270;
		}else if(inputY > 0){
			movementDirection = 90;
		}
	}

	float xspd,yspd = baseSpeed;
	float angle = movementDirection*(Mathf.PI/180);
	xspd = baseSpeed*Mathf.Cos(angle);
	yspd = baseSpeed*Mathf.Sin(angle);

	if(inputX != 0){
		speedX = xspd * Time.deltaTime;
	}else{
		if(Mathf.Abs(speedX) - (frictionForceX * Time.deltaTime) < 0){
			speedX = 0;
		}else{
			speedX = Mathf.Sign(speedX)*(Mathf.Abs(speedX) - (frictionForceX * Time.deltaTime));
		}
	}
	if(inputY != 0){
		speedY = yspd * Time.deltaTime;
	}else{
		if(Mathf.Abs(speedY) - (frictionForceY * Time.deltaTime) < 0){
			speedY = 0;
		}else{
			speedY = Mathf.Sign(speedY)*(Mathf.Abs(speedY) - (frictionForceY * Time.deltaTime));
		}
	}
	
	GetComponent<Rigidbody2D>().velocity = new Vector2(speedX,speedY);
}

void Action(){
	if(Input.GetAxis("Fire1") != 0){
		if(cursorHandler.structure != null){
			GameObject bumbel = Instantiate(Resources.Load<GameObject>("Prefabs/firepit"));
			Vector3 mouseCoordinates = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));
			mouseCoordinates.x = Mathf.Round(mouseCoordinates.x*2)/2f;
			mouseCoordinates.y = Mathf.Round(mouseCoordinates.y*2)/2f;
			bumbel.transform.position = new Vector3(mouseCoordinates.x, mouseCoordinates.y,-0.5f);
			cursorHandler.structure = null;
		}else if(!cursorHandler.overUI){
			if(cursorHandler.heldItem[0] != null){
				
			}
			GameObject bumbel = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/bumbel"));
			bumbel.GetComponent<Bumbel>().spd = 5;
			bumbel.GetComponent<Bumbel>().dir = faceDirection;
			bumbel.transform.position = transform.position;
		}
	}
	if(Input.GetAxis("Fire2") != 0){
		if(cursorHandler.structure != null){
			cursorHandler.structure = null;

		}else if(cursorHandler.heldItem[0] != null){
			cursorHandler.heldItem[0].UseFromInventory(this,cursorHandler.heldItem,0);
		}
	}
}

void OnTriggerEnter2D(Collider2D other){
		// if(other.gameObject.GetComponent("InteractiveInstallation") != null){
		// 	contactedInstallation = other.gameObject;
		// }
		// if(other.gameObject.GetComponent("DoorInstallation") != null){
		// 	contactedDoor = other.gameObject;
		// 	StartCoroutine(contactedDoor.GetComponent<DoorInstallation>().Traverse(this));
		// }
	}

	void OnTriggerExit2D(Collider2D other){
		// if(other.gameObject.GetComponent("InteractiveInstallation") != null && contactedInstallation != null){
		// 	if(other.gameObject.GetInstanceID() == contactedInstallation.gameObject.GetInstanceID()){
		// 		contactedInstallation = null;
		// 	}
		// }
		// if(other.gameObject.GetComponent("DoorInstallation") != null && contactedDoor != null){
		// 	if(other.gameObject.GetInstanceID() == contactedDoor.gameObject.GetInstanceID()){
		// 		contactedDoor = null;
		// 	}
		// }
	}


}
