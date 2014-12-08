using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using RAIN.Core;

public class BotInfo : MonoBehaviour {

	public Text status;
	public GameObject panel;
	private AI _bot;
	public AI bot { 
		get { return _bot; } 
		set { 
			_bot = value; 
			panel.SetActive(value != null); 
		} 
	}

	// Use this for initialization
	void Start () {
		if(_bot == null) {
			panel.SetActive(false);
		}		
	}
	
	// Update is called once per frame
	void Update () {
		if(_bot != null && _bot.Body != null) {
			Vector3 target = _bot.WorkingMemory.GetItem<Vector3>("moveTarget");
			status.text = bot.WorkingMemory.GetItem<string>("status")+"\n"
				+(int)_bot.Body.GetComponent<BotData>().health+"\n"
					+(int)_bot.WorkingMemory.GetItem<float>("damage")+"\n"
					+(int)_bot.Body.transform.position.x+", "
					+(int)_bot.Body.transform.position.z+"\n"
					+(int)target.x+", "+(int)target.z;
		}
	}
}
