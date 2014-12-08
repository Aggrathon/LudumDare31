using UnityEngine;
using System.Collections;

public class DisableOnLoad : MonoBehaviour {

	void Awake() {
		gameObject.SetActive(false);
	}
}
