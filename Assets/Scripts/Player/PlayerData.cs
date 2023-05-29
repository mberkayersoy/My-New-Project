using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public void RemoveFriend(string oldFriend)
    {
        friendsList.Remove(oldFriend);
    }

    public List<string> GetFriendList()
    {
        return friendsList;
    }

    public void SetFriendList(List<string> friendData)
    {
         friendsList = friendData;
    }

    public void AddRequest(string newRequest)
    {
        requestList.Add(newRequest);
    }

    public void RemoveRequest(string oldRequest)
    {
        requestList.Remove(oldRequest);
    }

    public void SetRequestList(List<string> requestData)
    {
        requestList = requestData;
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
