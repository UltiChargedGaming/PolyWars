using Firebase;
using Firebase.Unity.Editor;
using UnityEngine;
using Firebase.Database;

public class FireBaseCOntroller : MonoBehaviour {

	public class User{
		public string username;
		public string email;

		public User(){
		}

		public User(string username,string email)
		{
			this.username = username;
			this.email = email;
		}
	}
	// Use this for initialization
//	void Start() {
    // Set this before calling into the realtime database.
//		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://operation-polywars.firebaseio.com/");
//		username = "Valeri";
//		email = "valerish42@icloud.com";
//		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
//		User user = new User(username, email);
//    	string json = JsonUtility.ToJson(user);
//		reference.Child("users").Child("2").SetRawJsonValueAsync(json);

 // }


}