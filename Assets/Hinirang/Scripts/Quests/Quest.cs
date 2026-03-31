using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Quest
{
    public List<QuestObjective> questObjectives;
    public string questTitle;
    public string questDescription;
    public QuestState questState;
    public int currentObjectiveIndex = 0;

    public bool IsCompleted()
    {
        foreach (var objective in questObjectives)
        {
            if (objective.questState != QuestState.Completed)
            {
                return false;
            }
        }
        return true;
    }

    public void CheckObjective(string targetName)
    {
        if (currentObjectiveIndex < 0 || currentObjectiveIndex >= questObjectives.Count)
        {
            Debug.LogWarning("No active objective to check.");
            return;
        }
        QuestObjective currentObjective = questObjectives[currentObjectiveIndex];
        if (currentObjective.targetName == targetName)
        {
            currentObjective.UpdateProgress(targetName);
            if (currentObjective.IsComplete())
            {
                currentObjectiveIndex++;
                if (currentObjectiveIndex >= questObjectives.Count)
                {
                    questState = QuestState.Completed;
                    Debug.Log($"Quest '{questTitle}' completed!");
                }
                else
                {
                    questState = QuestState.InProgress;
                    Debug.Log($"Objective '{currentObjective.objectiveTitle}' completed! Next objective: '{questObjectives[currentObjectiveIndex].objectiveTitle}'");
                }
            }
        }
    }
}
