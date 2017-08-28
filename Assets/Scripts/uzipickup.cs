using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uzipickup : MonoBehaviour {

	public GameObject uziprefab;

	public void UziPickUp()
	{
		uziprefab.SetActive (true);

		Destroy (this.gameObject);
	}
}
