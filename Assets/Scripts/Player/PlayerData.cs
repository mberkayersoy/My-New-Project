using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class PlayerData : MonoBehaviour
{
    [SerializeField] private string username;
    [SerializeField] private int experience;
    [SerializeField] private int winCount;
    [SerializeField] private List<string> requestList;
    [SerializeField] private List<string> friendsList;

    public void AddFriend(string newFriend)
    {
        friendsList.Add(newFriend);
        GameObject row = Instantiate(NetworkUIManager.Instance.friendRowPrefab, NetworkUIManager.Instance.friendContent);
        row.GetComponentInChildren<TextMeshProUGUI>().text = newFriend;
    }

    public void RemoveFriend(string oldFriend)
    {
        friendsList.Remove(oldFriend);
    }

    public List<string> GetFriendList()
    {
        return friendsList;
    }

    // Login yapilinca calisiyor.
    public void SetFriendList(List<string> friendData)
    {
        friendsList = friendData;
        SetFriendsRow();
    }

    public void SetFriendsRow()
    {
        foreach (string friend in friendsList)
        {
            GameObject row = Instantiate(NetworkUIManager.Instance.friendRowPrefab, NetworkUIManager.Instance.friendContent);
            row.GetComponentInChildren<TextMeshProUGUI>().text = friend;
        }
    }

    public void AddRequest(string newRequest)
    {
        requestList.Add(newRequest);
    }

    public void RemoveRequest(string oldRequest)
    {
        requestList.Remove(oldRequest);
    }

    // Login yapilinca calisiyor.
    public void SetRequestList(List<string> requestData)
    {
        requestList = requestData;
        SetRequestRows();
    }

    public void SetRequestRows()
    {
        foreach (string request in requestList)
        {
            GameObject row = Instantiate(NetworkUIManager.Instance.requestRowPrefab, NetworkUIManager.Instance.requestContent);
            row.GetComponentInChildren<TextMeshProUGUI>().text = request;
        }
    }

    public List<string> GetRequestList()
    {
        return requestList;
    }

    public void SetExperience(int experiencePoint)
    {
        experience += experiencePoint;
    }

    public int GetExperience()
    {
        return experience;
    }

    public string GetUsername()
    {
        return username;
    }

    public void SetUsername(string usernameData)
    {
        username = usernameData;
    }

    public void SetWinCount(int winData)
    {
        winCount = winData; 
    }

    public void AddWin()
    {
        winCount++;
    }

}
