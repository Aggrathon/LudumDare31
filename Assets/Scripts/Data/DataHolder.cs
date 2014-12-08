using UnityEngine;
using System.Collections;

public class DataHolder : MonoBehaviour {

	private static Data data;

	private static BotServer _playerServer;
	public static BotServer playerServer { get { return _playerServer; } }
	private static BotServer _enemyServer;
	public static BotServer enemyServer { get { return _enemyServer; } }

	public GameObject gameWonScreen;
	public GameObject gameLostScreen;

	[SerializeField]
	private Data dataObject = null;

	void Awake() {
		if(data != null)
			dataObject = data;
		else {
			data = (Data)Instantiate(dataObject);
		}
		_playerServer = new BotServer(data);
		_enemyServer = new BotServer(data);

		gameWonScreen.SetActive(false);
		gameLostScreen.SetActive(false);
	}

	void Update() {
		_playerServer.Update (Time.deltaTime);
		_enemyServer.Update (Time.deltaTime);
		
		if(_playerServer.defeated) {
			gameLostScreen.SetActive(true);
			_playerServer.defeated = false;
		}
		else if(_enemyServer.defeated) {
			gameWonScreen.SetActive(true);
			_enemyServer.defeated = false;
		}
	}

	public static Data getData() { return data; }

	public static BotServer getBotServer(int teamid) { return teamid == data.team ? _playerServer : _enemyServer; }
	
}
