using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork : Photon.MonoBehaviour {
	public GameObject crossHair;
    public GameObject CrossLine;
	private Vector3 realPosition = Vector3.zero;
	private Quaternion realRotation = Quaternion.identity;
	// Use this for initialization
	void Awake () {
        if(photonView.isMine)
        {

        }
	}
	
	// Update is called once per frame
	void Update () {
		if (photonView.isMine) {

			crossHair.SetActive(true);
			CrossLine.SetActive(true);

        } else {
			crossHair.SetActive (false);
			transform.position = Vector3.Lerp (transform.position, realPosition, 0.1f);
			transform.rotation = Quaternion.Lerp (transform.rotation, realRotation, 0.1f);

		}
	}

	public void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info)
	{
		if (stream.isWriting) {
			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);
        }
        else {
			realPosition = (Vector3)stream.ReceiveNext();
			realRotation = (Quaternion)stream.ReceiveNext();

        }

    }
}
