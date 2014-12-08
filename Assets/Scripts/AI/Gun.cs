using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	private LineRenderer lr;
	private float cooldownLeft = 0f;
	public float gunCooldown = 1f;
	public float fxLength = 0.75f;
	public AudioSource audioSrc;

	// Use this for initialization
	void Start () {
		lr = GetComponent<LineRenderer>();
	}

	void FixedUpdate() {
		cooldownLeft -= Time.fixedDeltaTime;
	}

	public void Shoot(Vector3 target) {
		if(cooldownLeft<0) {
			lr.enabled = true;
			lr.SetPosition(0, transform.position);
			lr.SetPosition(1, target);
			StartCoroutine(disableFx());
			audioSrc.Play();
			audioSrc.pitch = Random.Range(.7f,1.05f);
		}
	}

	private IEnumerator disableFx() {
		yield return new WaitForSeconds(fxLength);
		lr.enabled = false;
	}
}
