using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public GameObject quitButton;
	public Slider difficultySlider;
	public Slider numberOfBotsSlider;
	public Text numberOfBotsText;
	public GameObject[] teamButtons;
	[Space(5)]
	public GameObject gameUI;

	void Awake() {
		gameObject.SetActive(DataHolder.getData().showMainMenuOnLoad);
	}

	void Start () {
		Data data = DataHolder.getData();
		#if UNITY_WEBPLAYER
		if(quitButton != null)
			quitButton.SetActive(false);
		#endif
		
		if(difficultySlider != null)
			difficultySlider.value = data.difficulty;
		if(numberOfBotsSlider != null) {
			numberOfBotsSlider.value = data.nrBots;
			numberOfBotsText.text = "Bots: "+data.nrBots;
		}

		for(int i = 0; i < teamButtons.Length; i++) {
			Text t = teamButtons[i].GetComponentInChildren<Text>();
			if (t != null) {
				Color c = data.teams[i].color;
				t.text = "<color=#"+string.Format("{0:X2}{1:X2}{2:X2}",(int)(c.r*255), (int)(c.g*255), (int)(c.b*255))
					+">"+data.teams[i].name+"</color>";
			}
		}
		teamButtons[data.team].GetComponent<Button>().interactable = false;

	}
	
	public void ExitApplication() {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		DataHolder.getData().showMainMenuOnLoad = false;
		#else
		Application.Quit();
		#endif
	}
	
	
	public void RestartLevel() {
		Application.LoadLevel(Application.loadedLevel);
		DataHolder.getData().showMainMenuOnLoad = false;
	}

	public void ChangeDifficulty(float diff) {
		DataHolder.getData().difficulty = diff;
	}
	public void ChangeNrOfBots(float bots) {
		DataHolder.getData().nrBots = (int)bots;
		numberOfBotsText.text = "Bots: "+(int)bots;
	}

	public void SelectTeam (int team) {
		teamButtons[DataHolder.getData().team].GetComponent<Button>().interactable = true;
		teamButtons[team].GetComponent<Button>().interactable = false;
		DataHolder.getData().team = team;
	}

	void OnEnable () { gameUI.SetActive(false); }
	void OnDisable () { gameUI.SetActive(true); }
}
