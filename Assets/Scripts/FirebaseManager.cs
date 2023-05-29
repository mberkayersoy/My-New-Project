using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using System;
using System.Collections;
using Firebase.Firestore;
using System.Collections.Generic;
using Firebase.Extensions;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;

    [Header("Firebase")]
    private FirebaseAuth auth;
    private FirebaseUser user;
    private FirebaseFirestore firestore;
    [Space(5f)]

    [Header("Login References")]
    [SerializeField] public TMP_InputField loginEmail;
    [SerializeField] public TMP_InputField loginPassword;
    [SerializeField] public TMP_Text loginOutputText;
    [Space(5f)]

    [Header("Register References")]
    [SerializeField] public TMP_InputField registerUsername;
    [SerializeField] public TMP_InputField registerEmail;
    [SerializeField] public TMP_InputField registerPassword;
    [SerializeField] public TMP_InputField registerConfirmPassword;
    [SerializeField] public TMP_Text registerOutputText;

    [Header("Menu UI")]
    [SerializeField] public TextMeshProUGUI usernameText;
    [SerializeField] public TextMeshProUGUI experienceText;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
            Instance = this;
        }

        if (!Application.isEditor)
        {
            ClearOutputs();
            if (auth != null)
            {
                auth.SignOut();
                Debug.Log("Sign out!");
            }
        }


        ClearOutputs();
        if (auth != null)
        {
            auth.SignOut();
            Debug.Log("Sign out!");
        }

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(checkDependancyTask =>
        {
            var dependencyStatus = checkDependancyTask.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {

                InitializeFirebase();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }
    void OnDestroy()
    {
        if (auth != null)
        {
            auth.StateChanged -= AuthStateChanged;
            auth.SignOut();
            auth = null;
        }

        if (user != null)
            user.DeleteAsync();
    }
    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);

        firestore = FirebaseFirestore.DefaultInstance;
    }

    private void AuthStateChanged(object sender, EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out");
            }

            user = auth.CurrentUser;

            if (signedIn)
            {

                Debug.Log($"Signed In: {user.DisplayName}");
            }

        }
    }

    public void ClearOutputs()
    {
        loginOutputText.text = "";
        registerOutputText.text = "";
        if (!Application.isEditor)
        {
            loginOutputText.text = "";
            registerOutputText.text = "";
        }
    }

    public void LoginButton()
    {
        StartCoroutine(LoginLogic(loginEmail.text, loginPassword.text));
    }


    public void RegisterButton()
    {
        StartCoroutine(RegisterLogic(registerUsername.text, registerEmail.text, registerPassword.text, registerConfirmPassword.text));
    }

    private IEnumerator LoginLogic(string _email, string _password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            FirebaseException firebaseException = (FirebaseException)loginTask.Exception.GetBaseException();
            AuthError error = (AuthError)firebaseException.ErrorCode;
            string output = "Unknown Error, Please Try Again";

            switch (error)
            {
                case AuthError.MissingEmail:
                    output = "Please Enter Your Email";
                    break;
                case AuthError.MissingPassword:
                    output = "Please Enter Your Password";
                    break;
                case AuthError.InvalidEmail:
                    output = "InvalidEmail";
                    break;
                case AuthError.WrongPassword:
                    output = "Incorrect Password";
                    break;
                case AuthError.UserNotFound:
                    output = "Account Does Not Exist";
                    break;
            }
            loginOutputText.text = output;
            loginOutputText.color = Color.red;
        }
        else
        {

            loginOutputText.text = "Login successfully";
            loginOutputText.color = Color.green;
            GetDatasFromFirestore(loginEmail.text);

            //PlayerInfo.Instance.setPlayerName(user.DisplayName);
            //LoginPanelController.Instance.LoginSuccess();
            Invoke("ClearOutputs", 0.5f);
        }
    }

    // Login yapýlýrken bütün bilgileri firebase'den çekip PlayerData sýnýfýnda tutuyorum.
    private async void GetDatasFromFirestore(string userEmail)
    {
        var querySnapshot = await firestore.Collection("users")
            .WhereEqualTo("email", userEmail)
            .GetSnapshotAsync();

        if (querySnapshot != null)
        {
            foreach (var document in querySnapshot.Documents)
            {
                var data = document.ToDictionary();
                if (data.ContainsKey("username"))
                {
                    // FirebaseManager'a playerData compenenti ekleyelim.
                    PlayerData playerData = gameObject.AddComponent<PlayerData>();

                    // Firebase'de olan her þeyi çekelim.
                    string username = data["username"].ToString();
                    string experience = data["experience"].ToString(); //ekranda göstermek için stringe çevirdim.
                    string winCount = data["winCount"].ToString(); //ekranda göstermek için stringe çevirdim.
                    List<string> friendList = (List<string>)data["friendList"];
                    List<string> requestList = (List<string>)data["requestList"];

                    // Firebase'den çekilen her þeyi PlayerData'ya yazalým.
                    playerData.SetUsername(username);
                    playerData.SetExperience(int.Parse(experience));
                    playerData.SetWinCount(int.Parse(winCount));
                    playerData.SetFriendList(friendList);
                    playerData.SetRequestList(requestList);

                    // InfoPanel
                    usernameText.text = "Username: " + username;
                    experienceText.text = "Experience: " + experience;
                    //NetworkUIManager.Instance.OnLoginButtonClicked(username);
                }   
                else
                {
                    loginOutputText.text = "Username field not found";
                    loginOutputText.color = Color.red;

                }
            }
        }
    }

    private IEnumerator RegisterLogic(string _username, string _email, string _password, string _confirmPassword)
    {
        if (_password != _confirmPassword)
        {
            registerOutputText.text = "Passwords do not match!";
            yield break;
        }

        // Kullanýcý adýnýn kullanýmda olup olmadýðýný kontrol et
        var checkUsernameTask = firestore.Collection("users")
            .WhereEqualTo("username", _username)
            .GetSnapshotAsync();
        yield return new WaitUntil(() => checkUsernameTask.IsCompleted);

        if (checkUsernameTask.Exception != null)
        {
            Debug.LogError("Error checking username: " + checkUsernameTask.Exception);
            registerOutputText.text = "An error occurred. Please try again.";
            yield break;
        }

        if (checkUsernameTask.Result.Count > 0)
        {
            registerOutputText.text = "Username already exists!";
            yield break;
        }

        // Kullanýcýyý kaydet
        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            FirebaseException exception = (FirebaseException)registerTask.Exception.GetBaseException();
            AuthError errorCode = (AuthError)exception.ErrorCode;

            string message = "Registration failed. Error: " + errorCode.ToString();
            Debug.LogError(message);
            registerOutputText.text = message;
        }
        else
        {
            // Firestore'da kullanýcý bilgilerini kaydet
            var newUser = new Dictionary<string, object>
        {
            { "username", _username },
            { "email" , _email},
            { "password", _password},
            { "experience", 0 },
            { "winCount", 0 },
            { "friendList", null},
            { "requestList", null},
            // Diðer kullanýcý bilgilerini burada ekleyebilirsiniz
        };

            var saveUserTask = firestore.Collection("users").Document(auth.CurrentUser.UserId).SetAsync(newUser);
            yield return new WaitUntil(() => saveUserTask.IsCompleted);

            if (saveUserTask.Exception != null)
            {
                Debug.LogError("Error saving user data: " + saveUserTask.Exception);
                registerOutputText.text = "An error occurred during registration. Please try again.";
                yield break;
            }

            Debug.Log("Registration successful!");
            registerOutputText.text = "Registration successful!";
        }
    }

    public void UpdatePlayerExperience()
    {
        PlayerData playerData = GetComponent<PlayerData>();
        // PlayerData sýnýfýndan verileri alýn
        string username = playerData.GetUsername();
        int experience = playerData.GetExperience();
        // Diðer verileri alýn

        // Firestore üzerindeki belgeyi güncellemek için bir dictionary oluþturun
        Dictionary<string, object> updatedData = new Dictionary<string, object>
        {
            { "username", username },
            { "experience", experience },
            // Diðer verileri ekle
        };

        // Güncelleme iþlemini gerçekleþtirin
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        db.Collection("users").Document(auth.CurrentUser.UserId).UpdateAsync(updatedData)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Error updating player data: " + task.Exception);
                }
                else
                {
                    Debug.Log("Player data updated successfully");
                    experienceText.text = "Experience: " + experience;
                }
            });
    }

    public void SendRequest(string receiver)
    {
        PlayerData playerData = GetComponent<PlayerData>();
        // PlayerData sýnýfýndan verileri alýn
        string sender = playerData.GetUsername();
        List<string> friendList = playerData.GetFriendList();
        List<string> requestList = playerData.GetFriendList();
        // Diðer verileri alýn

        if (friendList.Contains(receiver))
        {
            NetworkUIManager.Instance.feedbackText.text = "You are already friends with " + receiver;
        }
        else if(requestList.Contains(receiver))
        {
            NetworkUIManager.Instance.feedbackText.text = receiver + " is already on your request list";
        }

        // Firestore üzerindeki belgeyi güncellemek için bir dictionary oluþturun
        Dictionary<string, object> updatedData = new Dictionary<string, object>
        {
            { "username", sender },
            { "friendList", friendList },
            // Diðer verileri ekle
        };

        // Güncelleme iþlemini gerçekleþtirin
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        db.Collection("users").Document(auth.CurrentUser.UserId).UpdateAsync(updatedData)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Error updating player data: " + task.Exception);
                }
                else
                {
                    Debug.Log("Player data updated successfully");
                    //experienceText.text = "Experience: " + experience;
                }
            });
    }

    //public async void UpdatePlayerExperience(string username)
    //{
    //    var querySnapshot = await firestore.Collection("users")
    //        .WhereEqualTo("username", username)
    //        .GetSnapshotAsync();

    //    if (querySnapshot != null)
    //    {
    //        foreach (var document in querySnapshot.Documents)
    //        {
    //            var data = document.ToDictionary();
    //            if (data.ContainsKey("username"))
    //            {
    //                string experience = data["experience"].ToString();
    //                PlayerData playerData = GetComponent<PlayerData>();
    //                playerData.SetExperience(int.Parse(experience));
    //                experienceText.text = "Experience: " + playerData.GetExperience().ToString();
    //            }
    //        }
    //    }
    //}
}