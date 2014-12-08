using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoomInfo : MonoBehaviour {
	
	[Range(0,2)]
	public int x = 0;
	[Range(0,2)]
	public int y = 0;

	Text text;
	RoomData roomData;

	// Use this for initialization
	void Start () {
		text = GetComponentInChildren<Text>();
		roomData = DataHolder.enemyServer.getRoomData(x, y);
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "Room Priority\n"+(int)roomData.priority;
	}
}
