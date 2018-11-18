using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

	float speedX = 0;
	float speedY = 0;
	float frictionForceX = 50f;
	float frictionForceY = 50f;
	float spd = 5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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

void KeyboardMovement(){
	float speedInitialX = speedX;
	float speedInitialY = speedY;
	if(Input.GetAxis("Horizontal") != 0){
		speedX = spd*Mathf.Sign(Input.GetAxis("Horizontal") * Time.deltaTime);
	}else{
		speedX -=  Mathf.Sign(speedX) * frictionForceX * Time.deltaTime;
		if(Mathf.Sign(speedX) != Mathf.Sign(speedInitialX)){
			speedX = 0;
		}
	}
	if(Input.GetAxis("Vertical") != 0){
		speedY = spd*Mathf.Sign(Input.GetAxis("Vertical") * Time.deltaTime);
	}else{
		speedY -=  Mathf.Sign(speedY) * frictionForceY * Time.deltaTime;
		if(Mathf.Sign(speedY) != Mathf.Sign(speedInitialY)){
			speedY = 0;
		}
	}
	Vector3 rotation = transform.eulerAngles;
	if(speedX > 0){
		if(speedY < 0){
			rotation.z = 225;
		}else if(speedY > 0){
			rotation.z = 315;
		}else{
			rotation.z = 270;
		}
	}else if(speedX < 0){
		if(speedY < 0){
			rotation.z = 135;
		}else if(speedY > 0){
			rotation.z = 45;
		}else{
			rotation.z = 90;
		}
	}else{
		if(speedY < 0){
			rotation.z = 180;
		}else if(speedY > 0){
			rotation.z = 0;
		}
	}
	//transform.eulerAngles = rotation;
	transform.Rotate(rotation);
	GetComponent<Rigidbody2D>().velocity = new Vector2(speedX,speedY);
}

void Action(){
	if(Input.GetAxis("Fire1") != 0){
		GameObject bumbel = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/bumbel"));
		bumbel.GetComponent<Bumbel>().spd = 5;
		bumbel.transform.eulerAngles = transform.eulerAngles;
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
