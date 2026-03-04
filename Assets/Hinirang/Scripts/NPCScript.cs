using UnityEngine;

public class NPCScript : Interactable
{
    [SerializeField] private DialogueLine[] dialogueLines;
    void Start()
    {
        base.Start();
    }

    public override void Interact()
    {
        if(dialogueLines != null && dialogueLines.Length > 0)
        {
            DialogueSystem.Instance.StartDialogue(dialogueLines);
        }
    }
}
