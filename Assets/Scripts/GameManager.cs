using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int currentStage;
    [SerializeField] private List<StageController> stages;
    [SerializeField] private List<GameObject> environments;

    [SerializeField] private Player player;

    private void Start()
    {
        MusicPlayer.Instance.Play(0);
        StartNextStage();
    }

    public void StartNextStage()
    {
        if(currentStage != 0)
        {
            MusicPlayer.Instance.FadePlay(currentStage);
        }

        DisableAllEnvironments();
        stages[currentStage].Setup();
        environments[currentStage].SetActive(true);
    }

    public void FinishStage()
    {
        if(currentStage < stages.Count)
        {
            currentStage++;
            UI_Manager.Instance.TurnOnVictoryPanel();
        }
        else
        {
            UI_Manager.Instance.WinGame();
        }
    }

    private void DisableAllEnvironments()
    {
        foreach(GameObject gameObject in environments)
        {
            gameObject.SetActive(false);
        }
    }
}
