using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager _instance;

    public static DataManager Instance { get { return _instance; } }

    private string CURRENT_LEVEL = "currentLevel";

    public int CurentLevel { get; set; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
    }

    private void Start()
    {
        CurentLevel = PlayerPrefs.GetInt(CURRENT_LEVEL, 0);
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(CURRENT_LEVEL, CurentLevel);
        PlayerPrefs.Save();
    }
}