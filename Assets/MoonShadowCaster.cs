using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonShadowCaster : MonoBehaviour {
	public CelestialBody earth;

	void Start () {
		earth = GameObject.Find("Humble Abode").GetComponent<CelestialBody>();
	}

	void LateUpdate () {
		if (earth) {
			transform.LookAt(earth.transform.position);
		}
	}
}