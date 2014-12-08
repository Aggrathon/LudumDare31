using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Data : ScriptableObject {

#if UNITY_EDITOR
	[MenuItem("Assets/Create/Data")]
	public static void CreateAsset () {
		ScriptableObjectUtility.CreateAsset<Data> ();
	}
#endif

	public Data() {
		_showMainMenuOnLoad = true;
	}

	void OnDestroy() {
		_showMainMenuOnLoad = false;
	}
	
	private bool _showMainMenuOnLoad = true;
	public bool showMainMenuOnLoad { get { return _showMainMenuOnLoad; } set { _showMainMenuOnLoad = value; } }

	[Header("Game Options")]
	[Range(0f, 1f)]
	public float difficulty = 0.5f;
	[Range(2, 32)]
	public int nrBots = 16;

	[SerializeField]
	private int _team;
	public int team {
		get { return _team; } 
		set { _team = Mathf.Clamp(value, 0, teams.Length-1); } 
	}

	[Header("Control Options")]
	public float cameraMoveSpeed = 10f;
	public float cameraZoomSpeed = 5f;

	[Header("Teams")]
	public Team[] teams;

	[Header("BotServer Options")]
	public float patrolPriority = 0.5f;
	public float sightPriority = 3.5f;
	public float soundPriority = 2f;
	[Space(3)]
	public float botsInRoomBonus = 40f;
	public float botsInRoomCapasity = 6f;
	public float adjacentRoomBonus = 20f;
	[Space(3)]
	public float distancePriority = 10f;
	public float timeCap = 30f;
	public float randomness = 60f;

	[Header("Bot Options")]
	public float wanderThreshold = 120f;


}

[System.Serializable]
public class Team  {
	public Color color;
	public string name;
	public float health;
	public float damage;
	public float speed;
	public float runSpeed;

	public Team(Color col, string name) {
		color = col;
		this.name = name;
		health = 100f;
		damage = 50f;
		speed = 2f;
		runSpeed = 3f;
	}

	public Team (Color color, string name, float health, float damage, float speed, float runSpeed)
	{
		this.color = color;
		this.name = name;
		this.health = health;
		this.damage = damage;
		this.speed = speed;
		this.runSpeed = runSpeed;
	}
	
}
