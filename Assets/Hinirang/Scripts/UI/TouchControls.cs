using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchControls : MonoBehaviour
{
    public Button UpButton;
    public Button DownButton;
    public Button LeftButton;
    public Button RightButton;
    public Button InteractButton;
    public Button AttackButton;


    void Start()
    {
        // Up Button
        AddEventTrigger(UpButton, EventTriggerType.PointerDown, (e) => Player.Instance.MoveUp());
        AddEventTrigger(UpButton, EventTriggerType.PointerUp, (e) => Player.Instance.StopMove());

        // Down Button
        AddEventTrigger(DownButton, EventTriggerType.PointerDown, (e) => Player.Instance.MoveDown());
        AddEventTrigger(DownButton, EventTriggerType.PointerUp, (e) => Player.Instance.StopMove());

        // Left Button
        AddEventTrigger(LeftButton, EventTriggerType.PointerDown, (e) => Player.Instance.MoveLeft());
        AddEventTrigger(LeftButton, EventTriggerType.PointerUp, (e) => Player.Instance.StopMove());

        // Right Button
        AddEventTrigger(RightButton, EventTriggerType.PointerDown, (e) => Player.Instance.MoveRight());
        AddEventTrigger(RightButton, EventTriggerType.PointerUp, (e) => Player.Instance.StopMove());

        InteractButton.onClick.AddListener(() => Player.Instance.Interact());
        AttackButton.onClick.AddListener(() => Player.Instance.Attack());
    }
    
    //tldr: helper method para pag hold ng mga button
    private void AddEventTrigger(Button button, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }
        
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }
}