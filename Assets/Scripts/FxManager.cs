using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxManager : MonoBehaviour {
	public GameObject smgBulletFxPrefab;
	public GameObject dustHitFxPrefab;
  
    LineRenderer bulletTrace;
	[PunRPC]
	void BulletFx(Vector3 startPos,Vector3 endPos) {
		Instantiate (smgBulletFxPrefab, startPos, Quaternion.Euler(startPos));
    }
}
