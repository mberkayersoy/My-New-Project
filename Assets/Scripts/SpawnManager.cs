using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    [SerializeField] GameObject[] Bases;
    public Vector3 SpawnPlayer(int teamNumber)
    {
        return Bases[teamNumber].transform.position;
    }
}
