using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour {
    //Login Variables
    public Text loginuserTxt;
    public Text loginpassTxt;
    private string loginusernameTxt;
    private string loginpasswordTxt;
    //Login Variables

    public GameObject menuBtns;
    public GameObject registerUi;
    public GameObject loginUi;
    public Text emailTxt;
    public Text passTxt;
    public Text status;
	public Text nickName;

    private string usernameTxt;
    private string passwordTxt;
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
    DatabaseReference reference;
    // Use this for initialization
    public class User
    {
        public string username;
        public string password;
		public string nickname;
		public int rank;
		public int level;
        public User()
        {
        }

		public User(string username, string password,string nickname,int rank,int level)
        {
            this.username = username;
            this.password = password;
			this.nickname = nickname;
			this.rank = rank;
			this.level = level;

    	}
    }
    void Start () {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://operation-polywars.firebaseio.com/");
        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }
	
	// Update is called once per frame
	void Update () {
    }

    public void RegisterOption() {
        registerUi.SetActive(true);
        menuBtns.SetActive(false);
    }
    public void RegBtn() {
		writeNewUser(emailTxt.text, passTxt.text,nickName.text,0,1);
    }
    public void LoginBtn() {
		LoginUser(loginuserTxt.text, loginpassTxt.text);
    }
    public void LoginOption(){
        loginUi.SetActive(true);
        menuBtns.SetActive(false);
    }

    private void LoginUser(string username, string password) {
        FirebaseDatabase.DefaultInstance
        .GetReference("users").Child(username)
        .GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                Debug.Log("");
            }
            else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
            	if (snapshot.Value != null) {
                    if(snapshot.Child("password").Value.ToString() == password) {
                        PlayerPrefs.SetString("PlayerName", username);
                        PhotonNetwork.LoadLevel("MainMenu");
					}else {
                    	Debug.Log("Wrong Password");
                    }
                }
            }
        });
    }

	private void writeNewUser(string username, string password,string nickname,int rank,int level) {
        FirebaseDatabase.DefaultInstance
        .GetReference("users").Child(username)
        .GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                // Handle the error...
            }
            else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Value != null) {
                    status.color = Color.red;
                    status.text = "User Exists Pick Another User Name";
                }
                else {
						User user = new User(username, password,nickname,rank,level);
                    string json = JsonUtility.ToJson(user);

                    reference.Child("users").Child(username).SetRawJsonValueAsync(json).ContinueWith(regTask => {
                        if (regTask.IsCanceled) {
                            Debug.LogError("UpdateUserProfileAsync was canceled.");
                            return;
                        }
                        if (regTask.IsFaulted) {
                            Debug.LogError("UpdateUserProfileAsync encountered an error: " + regTask.Exception);
                            return;
                        }
                        if (regTask.IsCompleted) {
                            Debug.Log("User Registered");
                            PlayerPrefs.SetString("PlayerName", username);
							PlayerPrefs.SetString("Rank", rank.ToString());
							PlayerPrefs.SetString("NickName", nickname);
                            PhotonNetwork.LoadLevel("MainMenu");
                        }
                    }); ;
                }
            }
     });
    }
}
