using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerConroller : Photon.MonoBehaviour {

    //Weapon Stats
    public int ammo = 0;
    public int range = 0;
    public int damage = 0;
    public float fireRate = 0;
    public bool state = false;
    public string gunName = "";
    public int currentClip = 0;

    //Team Stats
    public int redScore;
    public int blueScore;
    //Player Ui
    private Text matchTimerTxt;
    public float matchTimer = 5f;
    public float teamScoreLimit = 30f;
    private float teamScoreFillamt;
    private Text blueScoreUI;
    private Text redScoreUI;
    private Image redScoreBar;
    private Image blueScoreBar;
    private TextMesh playerNameTxt;
    private Text kdaTxt;
    private Text flagUI;
    private int killScore;
    private int deaths;
    private string nickName;
    //Physics stuff
    public float gravity;
    private float cooldown = 0;
    private float fallspeed;
    public float moveSpeed;
    public float preAimmoveSpeed;
    public float preaimLimit = 0.7f;

    //GameObjects
    public GameObject bulletSpawn;
    public GameObject crossHair;
    public GameObject Particals;
    Animator m_Animator;
    CharacterController controller;
    FxManager fxManager;
    private GunController gunControllScript;

    //Bools
    private bool isGrounded;
    bool hasFlag;

    //Vectors
    Vector3 moveVec = Vector3.zero;

    // Use this for initialization
    void Awake() {
        // Get Team scoreboard objects
        redScoreUI = GameObject.Find("RedScoreTxt").GetComponent<Text>();
        redScoreBar = GameObject.Find("RedScoreBar").GetComponent<Image>();
        blueScoreUI = GameObject.Find("BlueScoreTxt").GetComponent<Text>();
        blueScoreBar = GameObject.Find("BlueScoreBar").GetComponent<Image>();

        teamScoreFillamt = 1 / teamScoreLimit;
        preAimmoveSpeed = moveSpeed / 2;
        flagUI = GameObject.Find("flagUI").GetComponent<Text>();
        //Check if There is a FxManager
        fxManager = FindObjectOfType<FxManager>();
        if (fxManager == null) {
            Debug.LogError("No FxManager");
        }

        //Initialize Components
        matchTimerTxt = GameObject.Find("timerTxt").GetComponent<Text>();
        playerNameTxt = GameObject.Find("playerTxt").GetComponent<TextMesh>();
        m_Animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        gunControllScript = GetComponent<GunController>();
        kdaTxt = GameObject.Find("KdaTxt").GetComponent<Text>();
        if (photonView.isMine)
        {
            PhotonNetwork.player.NickName = PlayerPrefs.GetString("NickName");
        }
        playerNameTxt.text = photonView.owner.NickName;


    }

    //Start of Update Func
    void Update()
    {

        //Set The Player's K/D score
        killScore = PhotonNetwork.player.GetScore();
        deaths = (int)PhotonNetwork.player.CustomProperties["Deaths"];
        kdaTxt.text = killScore + "/" + deaths;
        hasFlag = GetComponent<GunController>().hasFlag;
        FlagTeamUI();
        TeamScoreCalc();
        //If theres no Gun Hide crossHair
        if (gunControllScript.noGun.activeSelf) {
            crossHair.SetActive(false);
        } else {
            crossHair.SetActive(true);
        }

        //CoolDown for shooting Counter
        cooldown -= Time.deltaTime;

        //If its a local player allow Movment And check flag
        if (photonView.isMine) {
            CheckMovement();
            rayTarget();
            TeamHoldsFlag();
        }
    }
    //End of Update Func

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
	// Check if player has flag
	void TeamHoldsFlag() {
		if (gunControllScript.flag.activeSelf) {
			if (PhotonNetwork.player.GetTeam () == PunTeams.Team.blue) {
				PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable(){{"FlagSide",(int)1}});
				Debug.Log ("blueFlag" + (int)PhotonNetwork.room.CustomProperties ["FlagSide"]);
			} else {
				PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable(){{"FlagSide",(int)2}});
				Debug.Log ("redFlag" + (int)PhotonNetwork.room.CustomProperties ["FlagSide"]);
			}
		} 
	}

	//Check to see if a player is Grounded
	void IsGrounded(){
		isGrounded = (Physics.Raycast (transform.position, -transform.up, (controller.height / 3f)));
	
	}
	//If player is not grounded make him fall until he is
	void Fall(){
		if (!isGrounded) {
			fallspeed += gravity * Time.deltaTime;
		} else {
			if (fallspeed > 0)
				fallspeed = 0;
		}
		controller.Move (new Vector3 (0f, -fallspeed)*Time.deltaTime);
	}

    //JoyStick Movment section
    private void CheckMovement(){
        IsGrounded();
        Fall();
        Vector3 lookder = Vector3.zero;
        float horizontalMovment = CrossPlatformInputManager.GetAxis("Horizontal");
        float verticalMovment = CrossPlatformInputManager.GetAxis("Vertical");
		float Hlookdir = CrossPlatformInputManager.GetAxisRaw("Horizontal_2");
		float Vlookdir = CrossPlatformInputManager.GetAxisRaw("Vertical_2");
		lookder = new Vector3(Hlookdir* moveSpeed,0,Vlookdir * moveSpeed);
		moveVec = new Vector3(horizontalMovment * moveSpeed,0,verticalMovment * moveSpeed);
        //If Both joystick are pressed
        if (moveVec != Vector3.zero && lookder != Vector3.zero) {
			moveVec = new Vector3(horizontalMovment * preAimmoveSpeed,0,verticalMovment * preAimmoveSpeed);
            transform.LookAt(transform.position + lookder);
            m_Animator.SetFloat("Forward", Mathf.Abs(horizontalMovment) + Mathf.Abs(verticalMovment), 0.2f, Time.deltaTime);
          
			//Fire in the Direction you facing (PreAim)
			if (Mathf.Abs(Hlookdir) >= preaimLimit || Mathf.Abs(Vlookdir) >= preaimLimit){
                Fire();
            }
        }

		//If only left joystick is pressed
		if (moveVec != Vector3.zero && lookder == Vector3.zero) {
			moveVec = new Vector3(horizontalMovment * moveSpeed,0,verticalMovment * moveSpeed);
			transform.LookAt(transform.position + moveVec);
            m_Animator.SetFloat("Forward", Mathf.Abs(verticalMovment) + Mathf.Abs(horizontalMovment), 0.2f, Time.deltaTime);
        }

        //If only right joystick is pressed
        if (moveVec == Vector3.zero && lookder != Vector3.zero) {
			transform.LookAt(transform.position + lookder);
            //Fire in the Direction you facing
			if(Mathf.Abs(Hlookdir) >= preaimLimit || Mathf.Abs(Vlookdir) >= preaimLimit) {
                Fire();
            }	
		}

		//If nothing is pressed
		if (moveVec == Vector3.zero && lookder == Vector3.zero) {
            m_Animator.SetFloat("Forward", 0, 0, Time.deltaTime);
        }
		controller.Move (moveVec * Time.deltaTime);
	}

	private void rayTarget() {
		
		RaycastHit objectHit;        
		//If we hit something turn the crosshair red
		if (Physics.Raycast (bulletSpawn.transform.position, bulletSpawn.transform.forward, out objectHit, range)) 
		{
			if (objectHit.collider != null) {
				crossHair.GetComponent<SpriteRenderer> ().color = Color.red;
				Vector3 endPoint;
				endPoint = objectHit.point;
				crossHair.transform.position = endPoint;
			}
		} 
		//if we didnt hit something turn it white
		else 
		{
			Vector3 endPoint;
			endPoint = bulletSpawn.transform.position + (bulletSpawn.transform.forward * range);
			crossHair.transform.position = endPoint;
			crossHair.GetComponent<SpriteRenderer> ().color = Color.white;

		}
	}

	private void Fire() {
		//If theres no Gun dont shoot
		if (gunControllScript.noGun.activeSelf){
			return;
		} else{
			if (cooldown > 0 || ammo <= 0){
				return;
			}

			if (gunName != "") {
				m_Animator.SetTrigger("CanShoot"+gunName);
			}

			ammo -= 1;
			//If no ammo while shooting - Reload
			if (ammo <= 0){
				gunControllScript.Reload();
				return;
			}
            RaycastHit objectHit;        
			// Shoot raycast
			if (Physics.Raycast (bulletSpawn.transform.position, bulletSpawn.transform.forward, out objectHit, range)) 
			{
				Health h = objectHit.collider.GetComponent<Health> ();

                if (fxManager != null) 
				{

					Vector3 endPoint;
					endPoint = objectHit.point;
					crossHair.transform.position = endPoint;

					fxManager.GetComponent<PhotonView> ().RPC ("BulletFx", PhotonTargets.All, bulletSpawn.transform.position, endPoint);
                    PhotonNetwork.Instantiate(Particals.name, bulletSpawn.transform.position, bulletSpawn.transform.rotation, 0);

                }
				if (h != null) 
				{
					PhotonPlayer playerHit = objectHit.collider.GetComponent<PhotonView> ().owner;
					if (PhotonNetwork.player.GetTeam () == playerHit.GetTeam()) {
						Debug.Log ("Friendly Fire");
					} else 
					{
                        h.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, damage);
						if(objectHit.collider.gameObject.GetComponent<Health>().canAddScore && objectHit.collider.gameObject.GetComponent<Health>().isKilled)
						{
							PhotonNetwork.player.AddScore(1);
							if (PhotonNetwork.player.GetTeam () == PunTeams.Team.blue && (int)PhotonNetwork.room.CustomProperties ["FlagSide"] == 1) {
								PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable(){{"BlueScore",(int)PhotonNetwork.room.CustomProperties["BlueScore"]+1}});
								Debug.Log ("blue: " + PhotonNetwork.room.CustomProperties ["BlueScore"] + " Red: " + PhotonNetwork.room.CustomProperties ["RedScore"]);
							}else if(PhotonNetwork.player.GetTeam () == PunTeams.Team.red && (int)PhotonNetwork.room.CustomProperties ["FlagSide"] == 2){
								PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable(){{"RedScore",(int)PhotonNetwork.room.CustomProperties["RedScore"]+1}});
								Debug.Log ("blue: " + PhotonNetwork.room.CustomProperties ["BlueScore"] + " Red: " + PhotonNetwork.room.CustomProperties ["RedScore"]);
							}
							objectHit.collider.gameObject.GetComponent<Health>().canAddScore = false;
						}
					}
				}
			} else 
			{
				if (fxManager != null) 
				{
                   
                    PhotonNetwork.Instantiate(Particals.name, bulletSpawn.transform.position, bulletSpawn.transform.rotation,0);
                    Vector3 endPoint;
					endPoint = bulletSpawn.transform.position + (bulletSpawn.transform.forward * range);
					crossHair.transform.position = endPoint;
					fxManager.GetComponent<PhotonView> ().RPC ("BulletFx", PhotonTargets.All, bulletSpawn.transform.position, endPoint);

				}

			}
			cooldown = fireRate;
		}
	}

}
