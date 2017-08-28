using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {
    private GameManager gm;
	public bool isKilled = false;
    private Animator anim;
	private Text healthPoints;
	public float hitPoints = 100f;
	public float currentHitPoints;
	public Image healthBar;
	public int score;
    bool isDead;
    public int deathScore;
	public bool canAddScore =true;
    public Transform flagPos;
	private GunController gunControllScript;
	// Use this for initialization
	void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        flagPos = GameObject.Find("FlagSpot").GetComponent<Transform>();
		gunControllScript = GetComponent<GunController> ();
        isKilled = false ;
        anim = GetComponent<Animator>();
        currentHitPoints = hitPoints;
        healthPoints = GameObject.Find ("HealthTxt").GetComponent<Text> ();
		healthBar.fillAmount = currentHitPoints/100f;

	}
	void Update() {
        if (isKilled == true) {
            GetComponent<PlayerConroller>().enabled = false;
        }
	}

	public float CalculateHealth(){
		return currentHitPoints / hitPoints;
	}

	// Update is called once per frame
	[PunRPC]
	public void TakeDamage (int amt) {
		currentHitPoints -= amt;
		healthBar.fillAmount = currentHitPoints/100f;
		if (GetComponent<PhotonView> ().isMine) {
			healthPoints.text = "Health: " + currentHitPoints;
		}
		if (currentHitPoints <= 0){
			isKilled = true;

            anim.SetBool("isDead", true);
            StartCoroutine("Die");
        }
	}

    IEnumerator Die(){ 
        yield return new WaitForSeconds(3f);
        if (GetComponent<PhotonView>().isMine) {
            if (gunControllScript.flag.activeSelf)
            {
                PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "FlagSide", (int)0 } });
                gm.photonView.RPC("CreateFlag", PhotonTargets.All);
            }
            PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable(){{"Deaths",(int)PhotonNetwork.player.CustomProperties["Deaths"]+1}});
			Debug.Log("Deaths:" + PhotonNetwork.player.CustomProperties["Deaths"]);
            GameObject.FindObjectOfType<MapPhoton>().RespawnPlayer();
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
