using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {
	
	public GameObject[] spawnRooms;
	public GameObject[] enterRooms;
	public GameObject[] cornerRooms;
	public GameObject[] hallwayRooms;
	public GameObject[] middleRooms;

	public float roomScale = 10f;

	void Awake() {
		Transform level = new GameObject("LEVEL").transform;
		//middle
		((GameObject)Instantiate( middleRooms[Random.Range(0,middleRooms.Length)],
			transform.position, Quaternion.identity))
			.transform.parent = level;
		//enter
		((GameObject)Instantiate(enterRooms[Random.Range(0,enterRooms.Length)],
		    transform.position+Vector3.left*roomScale, Quaternion.identity))
			.transform.parent = level;
		((GameObject)Instantiate(enterRooms[Random.Range(0,enterRooms.Length)],
		     transform.position+Vector3.right*roomScale, Quaternion.Euler(new Vector3(0,180,0))))
			.transform.parent = level;
		//spawn
		((GameObject)Instantiate(spawnRooms[Random.Range(0,spawnRooms.Length)], 
         	transform.position+Vector3.left*roomScale*2, Quaternion.identity))
			.transform.parent = level;
		((GameObject)Instantiate(spawnRooms[Random.Range(0,spawnRooms.Length)], 
		    transform.position+Vector3.right*roomScale*2,Quaternion.Euler(new Vector3(0,180,0))))
			.transform.parent = level;
		//hallways
		((GameObject)Instantiate(hallwayRooms[Random.Range(0,hallwayRooms.Length)],
		 	transform.position+Vector3.forward*roomScale, Quaternion.identity))
		 	.transform.parent = level;
		((GameObject)Instantiate(hallwayRooms[Random.Range(0,hallwayRooms.Length)],
		    transform.position+Vector3.back*roomScale, 
		    Quaternion.Euler(new Vector3(0,180,0))))
			.transform.parent = level;
		//corners
		((GameObject)Instantiate(cornerRooms[Random.Range(0,cornerRooms.Length)], 
		    transform.position+Vector3.left*roomScale+Vector3.forward*roomScale, 
		    Quaternion.identity))
			.transform.parent = level;
		((GameObject)Instantiate(cornerRooms[Random.Range(0,cornerRooms.Length)], 
		    transform.position+Vector3.left*roomScale+Vector3.back*roomScale, 
		    Quaternion.Euler(new Vector3(0,270,0))))
			.transform.parent = level;
		((GameObject)Instantiate(cornerRooms[Random.Range(0,cornerRooms.Length)], 
		    transform.position+Vector3.right*roomScale+Vector3.back*roomScale, 
		    Quaternion.Euler(new Vector3(0,180,0))))
			.transform.parent = level;
		((GameObject)Instantiate(cornerRooms[Random.Range(0,cornerRooms.Length)], 
		    transform.position+Vector3.right*roomScale+Vector3.forward*roomScale, 
		    Quaternion.Euler(new Vector3(0,90,0))))
			.transform.parent = level;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
