using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{
    public List<Quest> quests;
    public int activeQuestIndex;
    public static QuestSystem Instance;

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
}
