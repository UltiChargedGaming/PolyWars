using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.ThirdPerson;

public class GunController : Photon.MonoBehaviour {
	//Guns Classes
	WeaponsClasses.SMG1 smgCS;
	WeaponsClasses.AR1 arCS;
    private GameManager gm;
	//Player Prefab
	PlayerConroller player;

	//Actual Gun Prefabs
	public GameObject smg;
	public GameObject pistol;
	public GameObject ar;
	public GameObject flag;
	public GameObject noGun;
	public GameObject mapFlag;

	//Gun Ui Stuff
	private Text gunTypeText;
	private Text guntype;
	private Text ammoTxt;
	private Button reloadBtn;

	public bool hasFlag = false;
    public bool mapflagstate = true;
    //Trigger related Stuff
    private Transform guntoPick;
	public bool onPickup;

	//Animator
	Animator m_Animator;

	//End of Variables

	// Use this for initialization
	void Start () {
		//Initialize Components
		m_Animator 	= GetComponent<Animator>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gunTypeText = GameObject.Find ("GunType").GetComponent<Text> ();
		ammoTxt 	= GameObject.Find ("ammoTxt").GetComponent<Text> ();
        reloadBtn 	= GameObject.Find ("Reload").GetComponent<Button>();
		player 		= gameObject.GetComponent<PlayerConroller> ();

		noGun.SetActive(true);
		FlagState(hasFlag,mapflagstate);

		//Button Listeners
		reloadBtn.onClick.AddListener( () => {Reload();} );  // <-- you assign a method to the button OnClick event here

		//Initialize Weapon Class
		smgCS = new WeaponsClasses.SMG1();
		arCS = new WeaponsClasses.AR1();
	}
	
	// Update is called once per frame
	void Update () {
		//Gun Animations
		m_Animator.SetBool("HasSmg", smgCS.state);
		m_Animator.SetBool("HasAr",arCS.state);
        //Gun Animations
        ammoTxt.text = "Ammo: " +player.ammo.ToString();
		if (onPickup) { 
			switch (guntoPick.tag) {
			//If you picked SMG
			case "SMG": 
				if(smgCS.state == false){
					WeaponDefaultState();
					onPickup = false;
					smgCS.state = true;
					gunTypeText.text = smgCS.gunName;
					player.gunName 	 = smgCS.gunName;
					player.state 	 = smgCS.state;
					player.range 	 = smgCS.range;
					player.damage 	 = smgCS.damage;
					player.fireRate  = smgCS.fireRate;
					player.ammo 	 = smgCS.ammo;
					player.currentClip 	 = smgCS.ammo;
					GetComponent<PhotonView>().RPC("smgState", PhotonTargets.All);
                    }
                    break;
			//If you picked AR
            case "Ar":
				if (arCS.state == false){
					WeaponDefaultState();
					onPickup = false;
					arCS.state = true;
					player.gunName 	 = arCS.gunName;
					gunTypeText.text = arCS.gunName;
					player.range 	 = arCS.range;
					player.damage 	 = arCS.damage;
					player.fireRate  = arCS.fireRate;
					player.ammo 	 = arCS.ammo;
					player.currentClip   = arCS.ammo;

					GetComponent<PhotonView>().RPC("arState", PhotonTargets.All);
                    }
                    break;

			case "Flag":
				if(hasFlag == false)
				{
					hasFlag = true;
                    onPickup = false;
                    gm.photonView.RPC("DestroyFlag", PhotonTargets.All);
                    GetComponent<PhotonView>().RPC("FlagState", PhotonTargets.All, hasFlag, mapflagstate);
				}
				break;
            }
		}
	}

	//On PickUp Object Trigger Event
	public void OnTriggerEnter(Collider hit){
		onPickup = true; // the player entered the trigger
		guntoPick = hit.transform; // save the object transform
    }

	//Player pressed Reload Btn
	public void Reload() {
		StartCoroutine ("AddAmmo");

	}
	//Reset Gunstate
	void WeaponDefaultState(){
		smgCS.state = false;
		arCS.state = false;
	}

	//Choose witch weapon to show on Player
	[PunRPC]
	void smgState(){
		smg.SetActive (true);
		ar.SetActive (false);
		noGun.SetActive (false);
	}
	[PunRPC]
	void arState(){
		smg.SetActive (false);
		ar.SetActive (true);
		noGun.SetActive (false);
	}

	[PunRPC]
	void FlagState(bool myflagstate,bool mapflagstate){
        flag.SetActive (myflagstate);
	}



	//Add ammo to player acording to his Weapon
	[PunRPC]
	IEnumerator AddAmmo() {
		bool reloading = true;
		if (reloading){
			player.ammo = 0;
			m_Animator.SetTrigger("reload" + gunTypeText.text);
			yield return new WaitForSeconds(3f);
			player.ammo = player.currentClip;
			reloading = false;
		}
	}
}
