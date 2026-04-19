using UnityEngine;
[System.Serializable]
public class QuestObjective
{
    public QuestType questType;
    public QuestState questState;
    public string objectiveTitle;
    public string objectiveDescription;
    public string targetName;  // for Kill and Talk quests
                               // this is the name of the target NPC or enemy
                               // for Collect quests, this is the name of the item to collect
    public int targetAmount;   // for Collect quests, this is the amount of items to collec
                               // for Kill quests, this is the amount of enemies to kill
    public int currentAmount;  // for Collect quests, this is the current amount of items collected
                               // for Kill quests, this is the current amount of enemies killed

    // for Talk quests, targetAmount should be 1
    // and currentAmount should be 0 or 1, indicating whether the player has talked to the target NPC or not

    public void UpdateProgress(string targetName)
    {

        if (questState == QuestState.Completed)
        {
             return;
        }
        currentAmount ++;
        if (currentAmount >= targetAmount)
        {
            currentAmount = targetAmount;
            Debug.Log($"Objective '{objectiveTitle}' completed!");
            questState = QuestState.Completed;
            return;
        }
        else
        {
            questState = QuestState.InProgress;
            return;
        }
    }
    public bool IsComplete()
    {
        return questState == QuestState.Completed;
    }

}
