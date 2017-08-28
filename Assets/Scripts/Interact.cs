using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider hit) {
		if (hit.gameObject.tag == "SMG") 
		{
			Debug.Log ("PickUp");
			hit.gameObject.GetComponent<uzipickup> ().UziPickUp ();
		}
	}
}
