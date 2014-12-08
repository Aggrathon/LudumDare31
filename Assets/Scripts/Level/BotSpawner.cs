using UnityEngine;
using System.Collections;

public class BotSpawner : MonoBehaviour {

	public GameObject botPrefab;
	public float distanceToSpawn = 20f;
	public float spawnAreaSize = 4f;

	// Use this for initialization
	void Start () {
		if(botPrefab == null) {
			Debug.LogError("No BOTS!!!!");
			return;
		}

		Data data = DataHolder.getData();
		int eneTeamSize = (int)(data.nrBots*(data.difficulty));
		int plaTeamSize = (int)(data.nrBots*(1-data.difficulty));
		if(eneTeamSize==0) {
			eneTeamSize++;
			plaTeamSize--;
		} 
		else if(eneTeamSize==data.nrBots) {
			eneTeamSize--;
			plaTeamSize++;
		}

		int eneTeam = Random.Range(0,data.teams.Length-1);
		if(eneTeam == data.team) eneTeam = data.teams.Length-1;
		int plaTeam = data.team;

		Transform eneParent = new GameObject("BOTS_TEAM_ENEMY").transform;
		Transform plaParent = new GameObject("BOTS_TEAM_PLAYER").transform;

		bool player = true;

		int pla = 0;
		int ene = 0;

		for(int i = 0; i < data.nrBots; i++) {
			if(player && pla< plaTeamSize) {
				GameObject bot = (GameObject)Instantiate(botPrefab, 
				            transform.position+Vector3.left*distanceToSpawn
				            +Quaternion.AngleAxis(360*pla/plaTeamSize, new Vector3(0,1,0))
				            *Vector3.left*Mathf.Min(spawnAreaSize, plaTeamSize),
				            Quaternion.Euler(Vector3.right));
				bot.GetComponent<BotData>().Setup(data.teams[plaTeam], plaTeam);
				bot.transform.parent = plaParent;
				pla++;
			} else if( ene < eneTeamSize){
				GameObject bot = (GameObject)Instantiate(botPrefab, 
				            transform.position+Vector3.right*distanceToSpawn
				            +Quaternion.AngleAxis(360*ene/eneTeamSize, new Vector3(0,1,0))
				            *Vector3.right*Mathf.Min(spawnAreaSize, eneTeamSize),
				            Quaternion.Euler(Vector3.left));
				bot.GetComponent<BotData>().Setup(data.teams[eneTeam], eneTeam);
				bot.transform.parent = eneParent;
				ene++;
			} else {
				i--;
			}
			player = !player;
		}
	}
}
