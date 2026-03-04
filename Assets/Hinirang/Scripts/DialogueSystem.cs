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
    }
    public void StartDialogue(DialogueLine[] dialogueLines)
    {
        dialoguePanel.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(TypeDialogue(dialogueLines));
    }

    private IEnumerator TypeDialogue(DialogueLine[] dialogueLines)
    {
        yield return null; //parang scanner.nextLine() sa java, para maghintay ng frame bago magstart si dialogue
        //hahahahha hula-hula nalang
        foreach (DialogueLine line in dialogueLines)
        {
            yield return null; 
            characterNameUI.text = line.characterName;
            lineUI.text = "";
            bool skipped = false;

            foreach (char letter in line.line)
            {
                float timer = 0f;
                while (timer < typingSpeed)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        lineUI.text = line.line;
                        skipped = true;
                        break;
                    }
                    timer += Time.deltaTime;
                    yield return null;
                }

                if (skipped) break;

                lineUI.text += letter;
                PlayTextBlip();
            }

            if (skipped)
            {
                yield return null;
            }
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        }

        dialoguePanel.gameObject.SetActive(false);
        Player.Instance.currentState = Player.PlayerState.Idle;
    }

    private void PlayTextBlip()
    {
        if (textBlipSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(textBlipSFX);
        }
    }

}
