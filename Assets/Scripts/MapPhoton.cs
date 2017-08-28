using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPhoton : Photon.MonoBehaviour {

    [SerializeField] private GameObject bluePlayer;
    [SerializeField] private GameObject redPlayer;
    public float respownTimer;
    [SerializeField] private int blueteam;
    [SerializeField] private int redteam;
    [SerializeField] private PunTeams punteams;
    [SerializeField] private GameObject lobbycamera;
	[SerializeField] private Transform[] RedSpawn;
	[SerializeField] private Transform[] BlueSpawn;
    [SerializeField] private bool offlinemode;
    GameObject myplayer;
    int numberPlayers;
    float counter;
    // Use this for initialization
    void Start () {
        PlayerSpawn();
        Debug.Log("You are on team " + PhotonNetwork.player.GetTeam());
        Debug.Log("Red Count: " + PunTeams.PlayersPerTeam[PunTeams.Team.red].Count + " Blue Count: " + PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count);
    }

    // Update is called once per frame
    void Update () {
	}

    public virtual void PlayerSpawn()
    {
        if (PhotonNetwork.player.GetTeam() == PunTeams.Team.blue)
        {
			myplayer = (GameObject)PhotonNetwork.Instantiate(bluePlayer.name, BlueSpawn[Random.Range(0, BlueSpawn.Length)].transform.position, BlueSpawn[Random.Range(0, BlueSpawn.Length)].transform.rotation, 0);
            myplayer.GetComponent<CameraMovment>().enabled = true;
            myplayer.GetComponent<PlayerConroller>().enabled = true;
            myplayer.GetComponent<GunController>().enabled = true;

        }
        else
        {
			myplayer = (GameObject)PhotonNetwork.Instantiate(redPlayer.name, RedSpawn[Random.Range(0, RedSpawn.Length)].transform.position, RedSpawn[Random.Range(0, RedSpawn.Length)].transform.rotation, 0);
            myplayer.GetComponent<CameraMovment>().enabled = true;
            myplayer.GetComponent<PlayerConroller>().enabled = true;
            myplayer.GetComponent<GunController>().enabled = true;
            PhotonNetwork.player.SetTeam(PunTeams.Team.red);
        }

    }
    public void RespawnPlayer()
    {
        PlayerSpawn();
    }
}
