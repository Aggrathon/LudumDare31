using UnityEngine;
using System.Collections;

public class KeyBoardInput : MonoBehaviour {
	
	public Vector2 cameraBoundsTopRight = new Vector2(10,10);
	public Vector2 cameraBoundsBottomLeft = new Vector2(-10,-10);
	private float cameraMoveSpeed = 10f;
	private float cameraZoomSpeed = 5f;
	[Header("Game Objects")]
	public GameObject pauseGraphic;
	public GameObject roomInfo;

	private float lastFrameTime = 0;

	[SerializeField]
	private GameObject mm;

	void Awake() {
		mm = GameObject.FindObjectOfType<MainMenu>().gameObject;
	}
	void Start() {
		cameraMoveSpeed = DataHolder.getData().cameraMoveSpeed;
		cameraZoomSpeed = DataHolder.getData().cameraZoomSpeed;
	}


	void Update () {
		float td = (Time.deltaTime==0? Time.realtimeSinceStartup-lastFrameTime:Time.deltaTime);

		if(Input.GetButtonUp("Cancel"))
			mm.SetActive(true);
		if(Input.GetButtonUp("Pause")) {
			togglePause();
		}
		if(Input.GetButtonUp("roomInfo"))
			toggleRoomInfo();
		if(Input.GetButtonUp("createSound"))
			gameObject.GetComponent<MouseSelect>().toggleSound();
		if(Input.GetButtonUp("createSight"))
			gameObject.GetComponent<MouseSelect>().toggleVisual();
		if(Input.GetButtonUp("muteRoom"))
			gameObject.GetComponent<MouseSelect>().toggleMute();
		if(Input.GetButtonUp("resetPatrol"))
			gameObject.GetComponent<MouseSelect>().togglePatrol();

		float zoom = Input.GetAxis("Zoom");
		if(zoom !=0) {
			Camera cam = Camera.main;
			cam.fieldOfView = Mathf.Clamp(cam.fieldOfView-zoom*cameraZoomSpeed, 10, 110);
		}

		float hor = Input.GetAxisRaw("Horizontal")*cameraMoveSpeed*td;
		float ver = Input.GetAxisRaw("Vertical")*cameraMoveSpeed*td;
		if(hor != 0 || ver != 0) {
			Transform cam = Camera.main.transform;
			cam.position = new Vector3(
				Mathf.Clamp(hor+cam.position.x, cameraBoundsBottomLeft.x, cameraBoundsTopRight.x),
				cam.position.y, 
				Mathf.Clamp(ver+cam.position.z, cameraBoundsBottomLeft.y, cameraBoundsTopRight.y));
		}


		lastFrameTime = Time.realtimeSinceStartup;
	}

	public void togglePause() {
		Time.timeScale = Time.timeScale==0? 1:0;
		if(pauseGraphic) 
			pauseGraphic.SetActive(Time.timeScale==0);
	}
	
	public void toggleRoomInfo() {
		roomInfo.SetActive(!roomInfo.activeSelf);
	}
	
}
