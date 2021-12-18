using System.Collections;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;

public class GameManager : MonoBehaviour, ITubeIsSortingCompleted, IGameOver
{
    private static GameManager instance;

    private int numberTubeNeedToBeSorted;

    private bool isGameOver;

    public static GameManager Instance { get => instance; private set => instance = value; }
    public int NumberTubeNeedToBeSorted { get => numberTubeNeedToBeSorted; set => numberTubeNeedToBeSorted = value; }


    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventName.On_Tube_Is_Sorting_Completed, OnTubeIsSortingCompleted);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventName.On_Tube_Is_Sorting_Completed, OnTubeIsSortingCompleted);
    }

    public void OnTubeIsSortingCompleted()
    {
        NumberTubeNeedToBeSorted--;
        if(NumberTubeNeedToBeSorted == 0)
        {
            isGameOver = true;
            OnGameOver();
        }
    }

    public void OnGameOver()
    {
        EventManager.EmitEvent(EventName.On_Game_Over);
        PlayWinEffect();
        AudioManager.Instance.PlayGameWinSound();
    }

    private void PlayWinEffect()
    {
        ParticleSystem confettiVFX = Resources.Load<ParticleSystem>("Confetti_VFX");
        Vector3 spawnPos = new Vector3(0, 7, 0);
        Instantiate(confettiVFX, spawnPos, Quaternion.Euler(90,0,0));
    }
}
