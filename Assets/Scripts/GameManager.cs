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
        StartNextStage();
    }

    public void StartNextStage()
    {
        DisableAllEnvironments();
        stages[currentStage].Setup();
        environments[currentStage].SetActive(true);
    }

    public void FinishStage()
    {
        currentStage++;
        UI_Manager.Instance.TurnOnVictoryPanel();
    }

    private void DisableAllEnvironments()
    {
        foreach(GameObject gameObject in environments)
        {
            gameObject.SetActive(false);
        }
    }
}
