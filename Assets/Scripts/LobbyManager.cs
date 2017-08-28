using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.SceneManagement;
public class LobbyManager : Photon.MonoBehaviour
{
    public Dropdown maxplayersDD;
    private Text waitingTxt;
    private Button SearchForGameBtn;
    [SerializeField] private PunTeams punteams;
    public float respownTimer;
    [SerializeField] private int blueteam;
    [SerializeField] private int redteam;
    private string username;
	private Text Playerlvl;
	private Text playerRank;
    public Text usernameTxt;
	private int maxPlayers;
    public static LobbyManager instance;
    [SerializeField] private bool offlinemode;
    GameObject myplayer;
    int numberPlayers;
    float counter;
    bool isJoined = false;
    bool showTxt = false;
    bool StartGame = false;
    public GameObject startButtons;
	public ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
	public ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable();
    // Use this for initialization
	void Start()
	{
        //GetComponent Section
		Playerlvl		 = GameObject.Find("Playerlvl").GetComponent<Text>();
		playerRank       = GameObject.Find("RankTxt").GetComponent<Text>();
        username         = PlayerPrefs.GetString("PlayerName");
        waitingTxt       = GameObject.Find("waitingTxt").GetComponent<Text>();
        SearchForGameBtn = GameObject.Find("StartGame").GetComponent<Button>();
        //GetComponent Section
        SearchForGameBtn.onClick.AddListener(() => { SearchGame(); });
        Connect();
        waitingTxt.enabled = false;
        SearchForGameBtn.interactable = false;

		//Get User Data From DB
		GetUserData (username);

        //Sync scenes automatically from master client
        PhotonNetwork.automaticallySyncScene = true;

        //Temp Dropdown of max players
    }

    //Temp DropDown hangler


    //What happens when we connet to Master Client
    public  void OnConnectedToMaster()
	{
        //Enable the Search game button Only when we conneted to master
		SearchForGameBtn.interactable = true;
	}

    //Search Game Button
    public void SearchGame()
    {
        OnJoinedLobby();
        showTxt = true;
    }

    void Connect()
    {
        //Connect to Master Client using this Version    
        PhotonNetwork.ConnectUsingSettings("Version 2");
        Debug.Log("Connected");
    }

    //What happens when we Join Lobby
    public virtual void OnJoinedLobby()
    {
        //Try to join a random room
        PhotonNetwork.JoinRandomRoom();
    }

    //What happens if we couldnt find a random room
    void OnPhotonRandomJoinFailed()
    {
        //Create a room instead
        Debug.Log("OnPhotonRandomJoinFailed");
        PhotonNetwork.CreateRoom(null);
    }

    //What happens when we Join a room
    public virtual void OnJoinedRoom()
    {
        //Call RPC to check amount of players in room
        photonView.RPC("WaitForPlayers", PhotonTargets.All);
    }


    // Update is called once per frame
    void Update()
    {
        //Set the Text to be equal to Value from DB

        maxPlayers = maxplayersDD.value;
        //Match search counter
        if (showTxt)
        {
            MatchSearchTimer();
        }
    }

    //What happens when a new player Joins a Room
    public virtual void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        if(PunTeams.PlayersPerTeam[PunTeams.Team.red] == null)
        {
            Debug.Log("No teams yet");
        }
        //Team Count
        redteam = PunTeams.PlayersPerTeam[PunTeams.Team.red].Count;
        blueteam = PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count;
        //Team Count
        if (redteam > blueteam)
            {
                other.SetTeam(PunTeams.Team.blue);
            }
            else if (blueteam > redteam)
            {
                other.SetTeam(PunTeams.Team.red);
            }
            else if (blueteam == redteam)
            {
                other.SetTeam(PunTeams.Team.blue);
            }
            Debug.Log(other.GetTeam());
    }

    //Waiting for players RPC
    [PunRPC]
    void WaitForPlayers()
    {
        //Check to see if the player count is smaller then max players allowed for this room
		if (PhotonNetwork.room.PlayerCount <maxPlayers)
        {
            Debug.Log("waiting for Players");
            Debug.Log("There are " + PhotonNetwork.room.PlayerCount + " Players in Queue");
        }else
            {
		        if (PhotonNetwork.isMasterClient)
		        {
		            PhotonNetwork.player.SetTeam(PunTeams.Team.red);
                    PhotonNetwork.LoadLevel("MainScene");
                    Debug.Log(StartGame);
                }
                   //Set Player Properties
                   properties.Add("Deaths",0);
			       properties.Add("NickName",PlayerPrefs.GetString("NickName").ToString());
			       roomProperties.Add ("RedScore", 0);
			       roomProperties.Add ("BlueScore", 0);
			       roomProperties.Add ("FlagSide", 0);
		           PhotonNetwork.player.SetCustomProperties(properties);
			       PhotonNetwork.room.SetCustomProperties (roomProperties);
                   StartGame = true;
                   punteams.UpdateTeams();
        }
    }

    //Match Search Timer Function
    void MatchSearchTimer()
    {
        string minutes = Mathf.Floor(counter / 60).ToString("00");
        string seconds = (counter % 60).ToString("00");
        counter += Time.deltaTime;
        waitingTxt.enabled = (true);
        waitingTxt.text = "Looking For a Match... " + minutes + ":" + seconds;
    }
		
	private void GetUserData(string username)
	{
		FirebaseDatabase.DefaultInstance
		.GetReference("users").Child(username)
		.GetValueAsync().ContinueWith(task =>{
			if (task.IsFaulted){
				Debug.Log("");
			}
			else if (task.IsCompleted){
				DataSnapshot snapshot = task.Result;
				if (snapshot.Value != null) {
					//Get Values from Database
					PlayerPrefs.SetString("NickName", snapshot.Child("nickname").Value.ToString());
					PlayerPrefs.SetString("Rank", snapshot.Child("rank").Value.ToString());
					PlayerPrefs.SetString("Level", snapshot.Child("level").Value.ToString());
                    Playerlvl.text = "Level: " + PlayerPrefs.GetString("Level");
                    usernameTxt.text =PlayerPrefs.GetString("NickName");

                    playerRank.text = "Rank: " + PlayerPrefs.GetString("Rank");
                }
			}
		});
	}
}






