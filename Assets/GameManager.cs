using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Photon.MonoBehaviour {
    public GameObject mapFlag;
    public Transform mapFlagSpot;
    GameObject currentFlag;
    private void Start()
    {
        if(PhotonNetwork.isMasterClient)
        photonView.RPC("CreateFlag", PhotonTargets.All);
    }

    private void Update()
    {
        
    }

    [PunRPC]
    void DestroyFlag()
    {
        Destroy(this.currentFlag);
    }

    [PunRPC]
    void CreateFlag()
    {
        Debug.Log("Create");
        currentFlag = (GameObject)Instantiate(mapFlag, mapFlagSpot.position, Quaternion.identity);
    }
}
