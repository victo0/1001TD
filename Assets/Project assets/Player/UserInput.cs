using UnityEngine;
using System.Collections;
using TD;

public class UserInput : MonoBehaviour {
	public Player player;
	void Start () {
		player = transform.root.GetComponent< Player > ();
	}

	void Update () {
		if (player.human) { // J'ai franchement besoin d'expliquer cà ?
			MoveCamera ();
			RotateCamera ();
			MouseActivity();
			TestClick();
		}
	}
	private void MoveCamera() {
		float xpos = Input.mousePosition.x;
		float ypos = Input.mousePosition.y;
		Vector3 movement = new Vector3(0,0,0);
		
		bool mouseScroll = false;
		
		//horizontal camera movement
		if(xpos >= 0 && xpos < ResourceManager.ScrollWidth) {
			movement.x -= ResourceManager.ScrollSpeed;
			player.hud.SetCursorState(CursorState.PanLeft);
			mouseScroll = true;
		} else if(xpos <= Screen.width && xpos > Screen.width - ResourceManager.ScrollWidth) {
			movement.x += ResourceManager.ScrollSpeed;
			player.hud.SetCursorState(CursorState.PanRight);
			mouseScroll = true;
		}
		
		//vertical camera movement
		if(ypos >= 0 && ypos < ResourceManager.ScrollWidth) {
			movement.z -= ResourceManager.ScrollSpeed;
			player.hud.SetCursorState(CursorState.PanDown);
			mouseScroll = true;
		} else if(ypos <= Screen.height && ypos > Screen.height - ResourceManager.ScrollWidth) {
			movement.z += ResourceManager.ScrollSpeed;
			player.hud.SetCursorState(CursorState.PanUp);
			mouseScroll = true;
		}
		
		//make sure movement is in the direction the camera is pointing
		//but ignore the vertical tilt of the camera to get sensible scrolling
		movement = Camera.main.transform.TransformDirection(movement);
		movement.y = 0;
		
		//away from ground movement
		movement.y -= ResourceManager.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel");
		
		//calculate desired camera position based on received input
		Vector3 origin = Camera.main.transform.position;
		Vector3 destination = origin;
		destination.x += movement.x;
		destination.y += movement.y;
		destination.z += movement.z;
		
		//limit away from ground movement to be between a minimum and maximum distance
		if(destination.y > ResourceManager.MaxCameraHeight) {
			destination.y = ResourceManager.MaxCameraHeight;
		} else if(destination.y < ResourceManager.MinCameraHeight) {
			destination.y = ResourceManager.MinCameraHeight;
		}
		
		//if a change in position is detected perform the necessary update
		if(destination != origin) {
			Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.ScrollSpeed);
		}
		if(!mouseScroll) {
			player.hud.SetCursorState(CursorState.Select);
		}
	}
	void RotateCamera() {
		Vector3 origin = Camera.main.transform.eulerAngles;
		Vector3 destination = origin;
			if((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetMouseButton(1)) {
			destination.x -= Input.GetAxis("Mouse Y") * ResourceManager.RotateAmount;
			destination.y += Input.GetAxis("Mouse X") * ResourceManager.RotateAmount;
		}
			if(destination != origin) {
			Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.RotateSpeed);
		}
	}
	private void  MouseActivity () {
		if (Input.GetMouseButtonDown (0)) {
				LeftMouseClick ();
		} else if (Input.GetMouseButtonDown (1)) {
				RightMouseClick ();
		}
		MouseHover ();
	}
	private void LeftMouseClick() {
		if(player.hud.MouseInBounds()) {
			if (player.IsFindingBuildingLocation()) {
				if (player.CanPlaceBuilding()) player.StartConstruction();
			} else {
				GameObject hitObject = WorkManager.FindHitObject(Input.mousePosition);
				Vector3 hitPoint = WorkManager.FindHitPoint(Input.mousePosition);
				if(hitObject && hitPoint != ResourceManager.InvalidPosition) {
					if(player.SelectedObject) player.SelectedObject.MouseClick(hitObject, hitPoint, player);
					else if(hitObject.tag!="Ground") {
						WorldObject worldObject = hitObject.transform.parent.GetComponent< WorldObject >();
						if(worldObject) {
							//we already know the player has no selected object
							player.SelectedObject = worldObject;
							worldObject.SetSelection(true, player.hud.GetPlayingArea());
						}
					}
				}
			}
		}
	}
	public void RightMouseClick() {
		if(player.hud.MouseInBounds() && !Input.GetKey(KeyCode.LeftAlt) && player.SelectedObject) {
			if(player.IsFindingBuildingLocation()) {
				player.CancelBuildingPlacement();
			} else {
				player.SelectedObject.SetSelection(false, player.hud.GetPlayingArea());
				player.SelectedObject = null;
			}
		}
	}
	private void MouseHover() {
		if(player.hud.MouseInBounds()) {
			if(player.IsFindingBuildingLocation()) {
				player.FindBuildingLocation();
			} else {
				GameObject hoverObject = WorkManager.FindHitObject(Input.mousePosition);
				if(hoverObject) {
					if (hoverObject.tag == "Ennemi") { 
					}
					if(player.SelectedObject) player.SelectedObject.SetHoverState(hoverObject);
					else if(hoverObject.tag != "Ground") {
						Player owner = hoverObject.transform.root.GetComponent< Player >();
						if(owner) {
							Unit unit = hoverObject.transform.parent.GetComponent< Unit >();
							Building building = hoverObject.transform.parent.GetComponent< Building >();
							if(owner.username == player.username && (unit || building)) {
								player.hud.SetCursorState(CursorState.Select);
							}
						}
					}
				}
			}
		}
	}
	private void TestClick () {
		if (Input.GetKeyDown (KeyCode.T)) {
			Debug.Log ("Test Ultime" );
		}
	}
}