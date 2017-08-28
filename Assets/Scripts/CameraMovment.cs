using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovment : MonoBehaviour {

	Camera camera;
	Transform cameraMov;
	public float smooth;

	private Vector3 velocity = Vector3.zero;
	// Use this for initialization
	void Start () {
		camera = GameObject.FindObjectOfType<Camera> ();

	}

	// Update is called once per frame
	void Update () {
		Vector3 pos = new Vector3 ();
		pos.x = transform.position.x;
		pos.y = transform.position.y;
		pos.z = transform.position.z;
		camera.transform.position = Vector3.SmoothDamp (camera.transform.position, pos, ref velocity, smooth);

	}
}
