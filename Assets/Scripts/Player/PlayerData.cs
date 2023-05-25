using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData : MonoBehaviour
{
    [SerializeField] private string username;
    [SerializeField] private int experience;
    [SerializeField] private List<string> friendsList;

    public void AddFriend(string newFriend)
    {
        friendsList.Add(newFriend);
    }
    public void RemoveFriend(string oldFriend)
    {
        friendsList.Remove(oldFriend);
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
   
}
