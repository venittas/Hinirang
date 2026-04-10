using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance;

    [SerializeField] private Image dialoguePanel;
    [SerializeField] private TextMeshProUGUI characterNameUI;
    [SerializeField] private TextMeshProUGUI lineUI;
    [SerializeField] private float typingSpeed = 0.1f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip textBlipSFX;
    [SerializeField] private string InteractingTarget;
    private bool skipRequested = false;
    private bool isTyping = false;

    private void Start()
    {
        GameManager.Instance.IntroHelper1Finished += Instance_IntroHelper1Finished; ;
    }

    private void Instance_IntroHelper1Finished(object sender, System.EventArgs e)
    {
        GameManager.Instance.IntroHelper2();
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Button dialoguUIButton = dialoguePanel.GetComponent<Button>();
        if (dialoguUIButton != null)
        {
            dialoguUIButton.onClick.AddListener(OnDialogueClicked);
        }
    }
    public void StartDialogue(DialogueLine[] dialogueLines, string eventName)
    {
        Player.Instance.currentState = Player.PlayerState.Interacting;
        dialoguePanel.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(TypeDialogue(dialogueLines, eventName));
        Debug.Log("Tapos na");
    }

    private IEnumerator TypeDialogue(DialogueLine[] dialogueLines, string eventName)
    {
        yield return null; //parang scanner.nextLine() sa java, para maghintay ng frame bago magstart si dialogue
        //hahahahha hula-hula nalang
        foreach (DialogueLine line in dialogueLines)
        {
            characterNameUI.text = line.characterName;
            lineUI.text = "";
            skipRequested = false;
            isTyping = true;

            foreach (char letter in line.line)
            {
                if (skipRequested)
                {
                    lineUI.text = line.line;
                    break;
                }

                lineUI.text += letter;
                PlayTextBlip();
                yield return new WaitForSeconds(typingSpeed);
            }

            isTyping = false;
            skipRequested = false;

            // Wait for click instead of key
            yield return new WaitUntil(() => skipRequested);
        }

        dialoguePanel.gameObject.SetActive(false);
        Player.Instance.currentState = Player.PlayerState.Moving;

        yield return new WaitForSeconds(1f);
        if (QuestSystem.Instance.CheckActiveObjective(InteractingTarget))
        {
            QuestSystem.Instance.UpdateQuestUI();
        }
        EventChecker(eventName);
    }

    public void EventChecker(string eventName)
    {
        if (eventName == "IntroHelper1")
        {
            // Call a public method on GameManager to raise the event, since you cannot invoke it directly here.
            GameManager.Instance.IntroHelper2();
        }
    }
    

    private void PlayTextBlip()
    {
        if (textBlipSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(textBlipSFX);
        }
    }

    public void OnDialogueClicked()
    {
        skipRequested = true;
    }

    public void SetInteractingTarget(string targetName)
    {
        InteractingTarget = targetName;
    }

}
