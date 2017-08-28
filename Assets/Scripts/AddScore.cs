using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AddScore : Photon.MonoBehaviour {

    Text kdaTxt;
	// Use this for initialization
	void Start () {
        kdaTxt = GameObject.Find("KdaTxt").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void AddKillScore()
    {
        float killScore = PhotonNetwork.player.GetScore();
        kdaTxt.text = killScore + "/D";
        PhotonNetwork.player.AddScore(1);
    }
}
