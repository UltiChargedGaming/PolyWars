using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDistruct : Photon.MonoBehaviour {

	public float selfDistructTime = 1f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		selfDistructTime -= Time.deltaTime;

		if (selfDistructTime <= 0) {
				Destroy (this.gameObject);
			}
		}

}
