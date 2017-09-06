using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Photon.MonoBehaviour {
    
	private Text matchTimerTxt;
	public float matchTimer = 5f;
	//Team Stats
	public int redScore;
	public int blueScore;
	private Text blueScoreUI;
	private Text redScoreUI;
	private Image redScoreBar;
	private Image blueScoreBar;
	public float teamScoreLimit = 30f;
	private float teamScoreFillamt;

	private Text flagUI;
	public GameObject mapFlag;
    public Transform mapFlagSpot;
    GameObject currentFlag;
    private void Start() {
		if (PhotonNetwork.isMasterClient) {
			photonView.RPC ("CreateFlag", PhotonTargets.All);
		}

		// Get UI Objects
		flagUI = GameObject.Find("flagUI").GetComponent<Text>();
		matchTimerTxt = GameObject.Find("timerTxt").GetComponent<Text>();
		redScoreUI = GameObject.Find("RedScoreTxt").GetComponent<Text>();
		redScoreBar = GameObject.Find("RedScoreBar").GetComponent<Image>();
		blueScoreUI = GameObject.Find("BlueScoreTxt").GetComponent<Text>();
		blueScoreBar = GameObject.Find("BlueScoreBar").GetComponent<Image>();
		teamScoreFillamt = 1 / teamScoreLimit;
    }

    private void Update() {
		FlagTeamUI();
		TeamScoreCalc();
    }
    [PunRPC]
    void DestroyFlag() {
        Destroy(this.currentFlag);
    }

    [PunRPC]
    void CreateFlag() {
        Debug.Log("Create");
        currentFlag = (GameObject)Instantiate(mapFlag, mapFlagSpot.position, Quaternion.identity);
    }

	void TeamScoreCalc() {
		MatchTimer();
		redScore = (int)PhotonNetwork.room.CustomProperties["RedScore"];
		blueScore = (int)PhotonNetwork.room.CustomProperties["BlueScore"];

		redScoreUI.text = redScore.ToString();
		redScoreBar.fillAmount = (float)redScore * teamScoreFillamt;
		blueScoreUI.text = blueScore.ToString();
		blueScoreBar.fillAmount = (float)blueScore * teamScoreFillamt;

		//Check to see if Timer is 0 and its not a draw
		//if timer is 0 and its a draw wait for the next kill
		if(matchTimer <= 0 && redScore != blueScore){
			if (blueScore > redScore)
			{
				//Blue team reached the kills limit = they won
				Debug.Log("Blue Wins/End match/Add Rank...");
			}
			if (redScore > blueScore)
			{
				//Red team reached the kills limit = they won
				Debug.Log("Red Wins/End match/Add Rank...");
			}
		}


		if(blueScore == teamScoreLimit){
			//Blue team reached the kills limit = they won
			Debug.Log("Blue Wins/End match/Add Rank...");
		}
		if (redScore == teamScoreLimit){
			//Blue team reached the kills limit = they won
			Debug.Log("Red Wins/End match/Add Rank...");
		}

	}
	//Match Timer
	void MatchTimer()
	{
		if(matchTimer >= 0){
			string minutes = Mathf.Floor(matchTimer / 60).ToString("00");
			string seconds = (matchTimer % 60).ToString("00");
			matchTimer -= Time.deltaTime;
			matchTimerTxt.text = minutes + ":" + seconds;
		}
	}

	//Team Flag Checker
	void FlagTeamUI(){
		if ((int)PhotonNetwork.room.CustomProperties["FlagSide"] == 0) {
			flagUI.text = "Capture The FLAG!";
		} else if ((int)PhotonNetwork.room.CustomProperties ["FlagSide"] == 1) {
			flagUI.text = "BlueTeam holds the FLAG";
		} else if ((int)PhotonNetwork.room.CustomProperties ["FlagSide"] == 2) {
			flagUI.text = "RedTeam holds the FLAG";
		}
	}
}
