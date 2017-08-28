using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRound : Photon.MonoBehaviour {

	public float timer;
	public float roundCooldown;
	public int roundLimit = 5;
	float timeLeft;
	float roundCd;
	private Text timerUI;
	private Text cooldownUI;
	private Text cdStringUI;
	private Text roundUI;
	private int round;
	public MapPhoton currentState;

	// Use this for initialization
	void Start () {
		round = 1;
		timeLeft = timer;
		roundCd = roundCooldown;
		timerUI = GameObject.Find ("timer").GetComponent<Text> ();
		roundUI = GameObject.Find ("round").GetComponent<Text> ();
		cooldownUI = GameObject.Find ("cd").GetComponent<Text> ();
		cdStringUI = GameObject.Find ("cooldown").GetComponent<Text> ();
		currentState = GameObject.Find("PhotonNetwork").GetComponent<MapPhoton> ();
		cooldownUI.enabled = false;
		cdStringUI.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		//GameRoundTimer ();
	}

	public virtual void GameRoundTimer()
	{
		if (round > roundLimit) {
			PhotonNetwork.LoadLevel("MainMenu");
		}
		// If time gt 0 keep counting and show to UI
		if (timeLeft < 0) {
			// ReSpawn players and start cooldown count.

			if (photonView.isMine) 
			{
				photonView.RPC ("DestroyPlayers", PhotonTargets.All);
			}
			roundCd -= Time.deltaTime;
			cooldownUI.text = ((int)roundCd).ToString();
			// Show cooldown UI
			cooldownUI.enabled = true;
			cdStringUI.enabled = true;
		} else {
			timeLeft -= Time.deltaTime;
			timerUI.text = ((int)timeLeft).ToString();
		}

		// If cooldown ends, add round count and remove cooldown UI
		if ( roundCd < 0) {
			cdStringUI.enabled = false;
			cooldownUI.enabled = false;
			if (photonView.isMine){
				photonView.RPC ("RespawnPlayers", PhotonTargets.All);

			}
			// Reset timers
			roundCd = roundCooldown;
			timeLeft = timer;
			round++;
			roundUI.text = round.ToString();
		}
	}

	[PunRPC]
	void DestroyPlayers() {
		PhotonNetwork.Destroy (GameObject.FindGameObjectWithTag ("Player"));
	}

	[PunRPC]
	void RespawnPlayers() {
		currentState.RespawnPlayer ();
	}
}
