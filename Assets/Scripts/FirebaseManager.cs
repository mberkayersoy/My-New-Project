using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using System;
using System.Collections;
using Firebase.Firestore;
using System.Collections.Generic;
using Firebase.Extensions;
using System.Linq;

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

    private void Start()
    {
        
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

    // Login yapan kullan�c�n�n b�t�n bilgileri firebase'den �ekip PlayerData s�n�f�nda tutuyorum.
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

                    // Firebase'de olan her �eyi �ekelim.
                    string username = data["username"].ToString();
                    string experience = data["experience"].ToString(); //ekranda g�stermek i�in stringe �evirdim.
                    string winCount = data["winCount"].ToString(); //ekranda g�stermek i�in stringe �evirdim
                                                                   
                    List<object> requestListData = data["requestList"] as List<object>;
                    List<string> stringRequestList;

                    if (requestListData != null)
                    {
                        stringRequestList = requestListData.Cast<string>().ToList();
                        playerData.SetRequestList(stringRequestList);
                    }
                    else
                    {
                        stringRequestList = new List<string>();
                        playerData.SetRequestList(stringRequestList);
                    }

                    List<object> friendListData = data["friendList"] as List<object>;
                    List<string> stringFriendList;

                    if (friendListData != null)
                    {
                        stringFriendList = friendListData.Cast<string>().ToList();
                        playerData.SetFriendList(stringFriendList);
                    }
                    else
                    {
                        stringFriendList = new List<string>();
                        playerData.SetFriendList(stringFriendList);
                    }

                    // Firebase'den �ekilen her �eyi PlayerData'ya yazal�m.
                    playerData.SetUsername(username);
                    playerData.SetExperience(int.Parse(experience));
                    playerData.SetWinCount(int.Parse(winCount));


                    // InfoPanel
                    usernameText.text = "Username: " + username;
                    experienceText.text = "Experience: " + experience;
                    NetworkUIManager.Instance.OnLoginButtonClicked(username);
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

        // Kullan�c� ad�n�n kullan�mda olup olmad���n� kontrol et
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

        // Kullan�c�y� kaydet
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
            // Firestore'da kullan�c� bilgilerini kaydet
            var newUser = new Dictionary<string, object>
        {
            { "username", _username },
            { "email" , _email},
            { "password", _password},
            { "experience", 0 },
            { "winCount", 0 },
            { "friendList", new List<string>()},
            { "requestList", new List<string>()},
            // Di�er kullan�c� bilgilerini burada ekleyebilirsiniz
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
        // PlayerData s�n�f�ndan verileri al
        PlayerData playerData = GetComponent<PlayerData>();
        string username = playerData.GetUsername();
        int experience = playerData.GetExperience();
        // Di�er verileri al

        // Firestore �zerindeki belgeyi g�ncellemek i�in bir dictionary olu�tur
        Dictionary<string, object> updatedData = new Dictionary<string, object>
        {
            { "username", username },
            { "experience", experience },
            // Di�er verileri ekle
        };

        // G�ncelleme i�lemini ger�ekle�tir
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

    public async void SendRequest(string receiver)
    {
        // PlayerData s�n�f�ndan gerekli verileri al
        PlayerData playerData = GetComponent<PlayerData>();
        string sender = playerData.GetUsername();
        List<string> senderFriendList = playerData.GetFriendList();
        List<string> senderRequestList = playerData.GetRequestList();

        if (senderFriendList != null)
        {
            if (senderFriendList.Contains(receiver))
            {
                NetworkUIManager.Instance.feedbackText.text = "You are already friends with " + receiver;
                return;
            }
        }

        if (senderRequestList != null )
        {
            if (senderRequestList.Contains(receiver))
            {
                NetworkUIManager.Instance.feedbackText.text = receiver + " is already on your request list";
                return;
            }
        }

        if (receiver == sender)
        {
            NetworkUIManager.Instance.feedbackText.text = "You cannot send requests to yourself";
            return;
        }

        var querySnapshot = await firestore.Collection("users")
            .WhereEqualTo("username", receiver)
            .GetSnapshotAsync();

        if (querySnapshot == null)
        {
            Debug.Log("null: " + receiver);
            NetworkUIManager.Instance.feedbackText.text = "Receiver not found: " + receiver;
            return;
        }

        if (querySnapshot != null)
        {
            Debug.Log("null degil: " + receiver);
            foreach (var document in querySnapshot.Documents)
            {
                var data = document.ToDictionary();

                if (data.ContainsKey("username"))
                {
                    List<object> receiverRequestList = data["requestList"] as List<object>;
                    List<string> stringReceiverRequestList;

                    if (receiverRequestList != null)
                    {
                        stringReceiverRequestList = receiverRequestList.Cast<string>().ToList();
                    }
                    else
                    {
                        stringReceiverRequestList = new List<string>();
                    }

                    if (stringReceiverRequestList != null)
                    {
                        if (receiverRequestList.Contains(sender))
                        {
                            NetworkUIManager.Instance.feedbackText.text = "You are already on " + receiver + "'s request list";
                            return;
                        }
                    }

                    receiverRequestList.Add(sender);

                    await document.Reference.UpdateAsync("requestList", receiverRequestList);

                    // �stek g�nderildi mesaj�n� g�ster
                    NetworkUIManager.Instance.feedbackText.text = "Request sent to " + receiver;
                }
            }
        }
        else
        {
            NetworkUIManager.Instance.feedbackText.text = "There is no such that user named " + receiver;
        }
    }

    public async void AcceptRequestAsync(string senderUsername)
    {
        PlayerData playerData = GetComponent<PlayerData>();
        string receiverUsername = playerData.GetUsername();

        // Request'in kabul edildi�i taraf�n bilgilerini al
        var acceptingUserQuery = await firestore.Collection("users").WhereEqualTo("username", receiverUsername).GetSnapshotAsync();
        var acceptingUserDocument = acceptingUserQuery.Documents.FirstOrDefault();

        if (acceptingUserDocument != null)
        {
            var acceptingUserData = acceptingUserDocument.ToDictionary();

            List<object> requestListData = acceptingUserData["requestList"] as List<object>;
            List<string> stringAcceptingUserRequestList;

            if (requestListData != null)
            {
                stringAcceptingUserRequestList = requestListData.Cast<string>().ToList();
            }
            else
            {
                stringAcceptingUserRequestList = new List<string>();
            }

            // Request'in kabul edildi�i taraf�n requestList'inden ilgili requesti kald�r
            stringAcceptingUserRequestList.Remove(senderUsername);
            playerData.RemoveRequest(senderUsername);

            List<object> friendListData = acceptingUserData["friendList"] as List<object>;
            List<string> stringAcceptingUserFriendList;

            if (friendListData != null)
            {
                stringAcceptingUserFriendList = friendListData.Cast<string>().ToList();
            }
            else
            {
                stringAcceptingUserFriendList = new List<string>();
            }

            // Kabul eden kullan�c�n�n friendList'ine request atan ki�iyi ekle
            stringAcceptingUserFriendList.Add(senderUsername);
            playerData.AddFriend(senderUsername);

            // Firebase Firestore'da g�ncelleme i�lemini yap
            await acceptingUserDocument.Reference.UpdateAsync(new Dictionary<string, object>
            {
                { "requestList", stringAcceptingUserRequestList },
                { "friendList", stringAcceptingUserFriendList }
            });

            // Request atan kullan�c�n�n bilgilerini al
            var requestSenderQuery = await firestore.Collection("users").WhereEqualTo("username", senderUsername).GetSnapshotAsync();
            var requestSenderDocument = requestSenderQuery.Documents.FirstOrDefault();

            if (requestSenderDocument != null)
            {
                var requestSenderData = requestSenderDocument.ToDictionary();

                List<object> senderFriendListData = requestSenderData["friendList"] as List<object>;
                List<string> stringaSenderUserFriendList;

                if (friendListData != null)
                {
                    stringaSenderUserFriendList = senderFriendListData.Cast<string>().ToList();
                }
                else
                {
                    stringaSenderUserFriendList = new List<string>();
                }

                // Request atan kullan�c�n�n friendList'ine kabul eden kullan�c�y� ekle
                stringaSenderUserFriendList.Add(receiverUsername);

                // Firebase Firestore'da g�ncelleme i�lemini yap
                await requestSenderDocument.Reference.UpdateAsync(new Dictionary<string, object>
                {
                    { "friendList", stringaSenderUserFriendList }
                });
            }
        }

    }
    public async void RejectRequestAsync(string rejectingUsername)
    {
        PlayerData playerData = GetComponent<PlayerData>();
        string receiverUsername = playerData.GetUsername();

        // Request'in kabul edildi�i taraf�n bilgilerini al
        var receiverUserQuery = await firestore.Collection("users").WhereEqualTo("username", receiverUsername).GetSnapshotAsync();
        var receiverUserDocument = receiverUserQuery.Documents.FirstOrDefault();

        if (receiverUserDocument != null)
        {
            var receiverUserData = receiverUserDocument.ToDictionary();

            List<object> requestListData = receiverUserData["requestList"] as List<object>;
            List<string> stringReceiverUserRequestList;

            if (requestListData != null)
            {
                stringReceiverUserRequestList = requestListData.Cast<string>().ToList();
            }
            else
            {
                stringReceiverUserRequestList = new List<string>();
            }

            // Request'in kabul edildi�i taraf�n requestList'inden ilgili requesti kald�r
            stringReceiverUserRequestList.Remove(rejectingUsername);
            playerData.RemoveRequest(rejectingUsername);

            // Firebase Firestore'da g�ncelleme i�lemini yap
            await receiverUserDocument.Reference.UpdateAsync(new Dictionary<string, object>
            {
                { "requestList", stringReceiverUserRequestList },
            });
        }
    }

    public async void RemoveFriendAsync(string removedFriend)
    {
        PlayerData playerData = GetComponent<PlayerData>();
        string localUsername = playerData.GetUsername();

        // Arkada� listesinden oyuncu silen ki�inin bilgilerini al
        var localUserQuery = await firestore.Collection("users").WhereEqualTo("username", localUsername).GetSnapshotAsync();
        var localUserDocument = localUserQuery.Documents.FirstOrDefault();

        if (localUserDocument != null)
        {
            var localUserData = localUserDocument.ToDictionary();

            List<object> friendListData = localUserData["friendList"] as List<object>;
            List<string> stringLocalUserFriendList;

            if (friendListData != null)
            {
                stringLocalUserFriendList = friendListData.Cast<string>().ToList();
            }
            else
            {
                stringLocalUserFriendList = new List<string>();
            }

            // Arkada� kald�ran ki�inin friendList'inden ilgili arkada�� kald�r
            stringLocalUserFriendList.Remove(removedFriend);
            playerData.RemoveFriend(removedFriend);

            // Firebase Firestore'da g�ncelleme i�lemini yap
            await localUserDocument.Reference.UpdateAsync(new Dictionary<string, object>
            {
                { "friendList", stringLocalUserFriendList },
            });

            // Kald�r�lan ki�inin bilgileri
            var removedUserrQuery = await firestore.Collection("users").WhereEqualTo("username", removedFriend).GetSnapshotAsync();
            var removedUserDocument = removedUserrQuery.Documents.FirstOrDefault();

            if (removedUserDocument != null)
            {
                var removedUserData = removedUserDocument.ToDictionary();

                List<object> removedUserFriendListData = removedUserData["friendList"] as List<object>;
                List<string> stringRemovedUserFriendList;

                if (friendListData != null)
                {
                    stringRemovedUserFriendList = removedUserFriendListData.Cast<string>().ToList();
                }
                else
                {
                    stringRemovedUserFriendList = new List<string>();
                }

                // removedUser ki�isinin friendList'inden kald�ran ki�iyi sil
                stringRemovedUserFriendList.Remove(localUsername);

                // Firebase Firestore'da g�ncelleme i�lemi yap
                await removedUserDocument.Reference.UpdateAsync(new Dictionary<string, object>
                {
                    { "friendList", stringRemovedUserFriendList }
                });
            }
        }
    }
}