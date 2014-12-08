using UnityEngine;
using System.Collections;
using RAIN.Core;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MouseSelect : MonoBehaviour {

	public BotInfo botInfo;
	[Header("Abilities")]
	public bool muteRoom = false;
	public bool createSound = false;
	public bool createVisual = false;
	public bool patrolRoom = false;
	public Texture2D targetMouse;

	public void toggleMute() {
		muteRoom = !muteRoom;
		toggleMouseCursor(muteRoom);
	}
	public void toggleSound() {
		createSound = !createSound;
		toggleMouseCursor(createSound);
	}
	public void toggleVisual() {
		createVisual = !createVisual;
		toggleMouseCursor(createVisual);
	}
	public void togglePatrol() {
		patrolRoom = !patrolRoom;
		toggleMouseCursor(patrolRoom);
	}

	private void toggleMouseCursor(bool t) {
		if(t)
			Cursor.SetCursor(targetMouse, new Vector2(15,15), CursorMode.Auto);
		else
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}

	void onDisable() {
		toggleMouseCursor(false);
		muteRoom = false;
		createSound = false;
		createVisual = false;
		patrolRoom = false;
	}

	// Update is called once per frame
	void Update () {
		if ( Input.GetMouseButtonDown(0)){
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit, 100.0f)){
				if (hit.transform.tag == "Bot") {
#if UNITY_EDITOR
					UnityEditor.Selection.activeTransform = hit.transform;
#endif
					botInfo.bot = hit.transform.gameObject.GetComponentInChildren<AIRig>().AI;
				} else {
					if(muteRoom) {
						DataHolder.enemyServer.muteRoom(hit.point);
						GetComponent<AudioSource>().Play();
						toggleMute();
					}
					if(createSound) {
						DataHolder.enemyServer.detectSound(hit.point);
						GetComponent<AudioSource>().Play();
						toggleSound();
					}
					if(createVisual) {
						DataHolder.enemyServer.detectVisual(hit.point);
						GetComponent<AudioSource>().Play();
						toggleVisual();
					}
					if(patrolRoom) {
						DataHolder.enemyServer.patrolRoom(hit.point);
						GetComponent<AudioSource>().Play();
						togglePatrol();
					}
				}
			}
		}
	}
}
