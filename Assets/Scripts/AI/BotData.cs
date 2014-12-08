using UnityEngine;
using System.Collections;
using RAIN.Core;

public class BotData : MonoBehaviour {

	[SerializeField]
	private float _health = 100f;

	public AudioClip dieClip;

	public float health {
		get { return _health; }
		set { _health = value; if(_health<0) Die(); }
	}

	public void Setup(Team team, int teamid) {
		GetComponentInChildren<MeshRenderer>().material.color = team.color;
		Color lazer = team.color;
		lazer = lazer+lazer*Color.grey;
		GetComponentInChildren<LineRenderer>().material.color = lazer;

		AI ai = GetComponentInChildren<AIRig>().AI;
		ai.WorkingMemory.SetItem<int>("team", teamid);
		ai.WorkingMemory.SetItem<float>("health", team.health);
		ai.WorkingMemory.SetItem<float>("runSpeed", team.runSpeed);
		ai.WorkingMemory.SetItem<float>("damage", team.damage);
		_health = team.health;
		ai.Motor.DefaultSpeed = team.speed;

		DataHolder.getBotServer(teamid).RegisterBot(ai);

		BotDestinationSelector bds = new BotDestinationSelector();
		bds.Start(ai);
		bds.Execute(ai);
		bds.Stop(ai);
	}

	public void Die() {
		AI ai = GetComponentInChildren<AIRig>().AI;
		DataHolder.getBotServer(ai.WorkingMemory.GetItem<int>("team")).UnregisterBot(ai);
		ai.IsActive = false;
		//Particle Effect
		//Sound
		Destroy(gameObject, 0.3f);
		GetComponent<AudioSource>().PlayOneShot(dieClip);
	}
}
