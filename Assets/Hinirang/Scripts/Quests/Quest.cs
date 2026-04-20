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

    public bool CheckActiveObjective(string targetName)
    {
        QuestObjective currentObjective = questObjectives[currentObjectiveIndex];
        if (currentObjective.targetName == targetName)
        {
            CheckObjective(targetName);
            return true;
        }
        else
        {
            return false;
        }
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
                Debug.Log("EVENT: "+Player.Instance.eventNameTrigger);
                if (Player.Instance.eventNameTrigger == "Day1Tiyanak" && currentObjective.targetName == "Tiyanak") 
                {
                    Debug.Log("StartDay3 triggered by: " + targetName);
                    GameManager.Instance.StartDay3();
                }else if (Player.Instance.eventNameTrigger == "Day3Tiyanak" && currentObjective.targetName == "Tiyanak")
                {
                    Debug.Log("EndDay3 triggered by: " + targetName);
                    Player.Instance.currentState = Player.PlayerState.Interacting;
                    GameManager.Instance.TeleportPlayer(13.56f, 3.94f);
                    GameManager.Instance.MoveDialogueToDay7();
                    Player.Instance.eventNameTrigger = "Day7";
                    MangEnko.Instance.enabled = false;
                    GameManager.Instance.Day7Intro();
                }
                if ((currentObjectiveIndex + 1) >= questObjectives.Count)
                {
                    questState = QuestState.Completed;
                    Debug.Log($"Quest '{questTitle}' completed!");
                    QuestSystem.Instance.CheckObjective(targetName);
                    return;
                }
                currentObjectiveIndex++;
                QuestSystem.Instance.UpdateQuestUI();
                questState = QuestState.InProgress;
                Debug.Log($"Objective completed! Next: '{questObjectives[currentObjectiveIndex].objectiveTitle}'");
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
