using UnityEngine;
using System.Collections;

public class KeepDataPersistant : MonoBehaviour {

	public GameObject dataHolderPrefab;

	void Awake() {
		if(dataHolderPrefab == null) {
			Debug.LogError("No Dataholder setup!");
			return;
		}
		GameObject dataHolder = GameObject.FindWithTag("DATA");
		if(dataHolder == null) {
			dataHolder = (GameObject)Instantiate(dataHolderPrefab);
			dataHolder.name = "DATA";
		}
		dataHolder.transform.parent = transform.parent;
	}
}
