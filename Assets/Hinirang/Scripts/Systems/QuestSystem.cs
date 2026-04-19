using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestSystem : MonoBehaviour
{
    public List<Quest> quests;
    public int activeQuestIndex;
    public static QuestSystem Instance;
    public TaskBG QuestBG;
    public TextMeshProUGUI QuestTitle;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
    }

    public bool CheckActiveObjective(string targetName)
    {
        if (activeQuestIndex < 0 || activeQuestIndex >= quests.Count)
        {
            Debug.LogWarning("No active quest to check.");
            return false;
        }
        Quest activeQuest = quests[activeQuestIndex];
        Debug.Log("Checking objective: " + targetName + " for quest: " + activeQuest.questTitle);
        return activeQuest.CheckActiveObjective(targetName); ;
    }

    public void CheckObjective(string targetName)
    {
        if (activeQuestIndex < 0 || activeQuestIndex >= quests.Count)
        {
            Debug.LogWarning("No active quest to check.");
            return;
        }
        Quest activeQuest = quests[activeQuestIndex];
        if (activeQuest.IsCompleted())
        {
            Debug.Log("Quest count " + activeQuestIndex);
            if (activeQuestIndex >= quests.Count - 1)
            {
                Debug.Log("All quests completed!");
                return;
            }
            activeQuestIndex++;
            Debug.Log("Next quest is: " + quests[activeQuestIndex].questTitle);
        }
    }

    public void UpdateQuestUI()
    {
        if (QuestBG == null)
        {
            Debug.LogError("QuestBG is NOT assigned in the Inspector!");
            return;
        }

        CancelInvoke("HideQuestUI");

        if (activeQuestIndex < 0 || activeQuestIndex >= quests.Count)
        {
            QuestTitle.text = "No active quest";
            QuestBG.gameObject.SetActive(true); 
            Debug.LogWarning("NOTHING");
        }
        else
        {
            Debug.LogWarning("SHOWING");
            Quest activeQuest = quests[activeQuestIndex];
            QuestTitle.text = activeQuest.questObjectives[activeQuest.currentObjectiveIndex].objectiveTitle;

            QuestBG.gameObject.SetActive(true);
            Invoke("HideQuestUI", 3f);
        }
    }


    public void HideQuestUI()
    {
        QuestBG.gameObject.SetActive(false);
    }
}
