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

    public void CheckObjective(string targetName)
    {
        if (activeQuestIndex < 0 || activeQuestIndex >= quests.Count)
        {
            Debug.LogWarning("No active quest to check.");
            return;
        }
        Quest activeQuest = quests[activeQuestIndex];
        activeQuest.CheckObjective(targetName);
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
        QuestBG.gameObject.SetActive(true);
        if (activeQuestIndex < 0 || activeQuestIndex >= quests.Count)
        {
            QuestTitle.text = "No active quest";
            return;
        }
        else
        {
            Quest activeQuest = quests[activeQuestIndex];
            QuestTitle.text = activeQuest.questTitle;
            Invoke("HideQuestUI", 3f);
        }
    }

    public void HideQuestUI()
    {
        QuestBG.gameObject.SetActive(false);
    }
}
