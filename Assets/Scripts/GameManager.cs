using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int currentStage;
    [SerializeField] private List<StageController> stages;

    private void Start()
    {
        StartStage();
    }

    public void StartStage()
    {
        stages[currentStage].Setup();
    }

    public void FinishStage()
    {
        currentStage++;
    }
}
