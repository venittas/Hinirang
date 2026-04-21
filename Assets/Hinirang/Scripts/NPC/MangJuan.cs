using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;

public class MangJuan : NPCScript
{
    public static MangJuan Instance;
    [SerializeField] private Boat Boat;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        base.Start();
    }

    public override void CheckEventTriggerName(string eventName)
    {
        string newEventName = eventName;
        if (eventName == "IntroHelper1")
        {
            Player.Instance.eventNameTrigger = "AlingNena1Quest1";
        }
    }

    public void Scene3()
    {
        StartCoroutine(Scene3Routine());
    }
    public IEnumerator MoveOne()
    {
        yield return StartCoroutine(Move(0));
    }

    public IEnumerator Scene3Routine()
    {
        yield return StartCoroutine(Move(1));
        yield return StartCoroutine(Move(2));
        yield return StartCoroutine(Move(3));
        StopMovement();
        Debug.Log("Scene 3");
        yield return StartCoroutine(MoveWithBoat());
        
    }

    public IEnumerator MoveWithBoat()
    {
        Boat.MoveBoat();
        yield return StartCoroutine(MoveNoAnim(4));
    }

    public void DisableBoat()
    {
        if (Boat != null)
        {
            Boat.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
            //Debug.Log(SceneSystem.Instance.currentPlayerLocation);
        if (SceneSystem.Instance.currentPlayerLocation != SceneSystem.SceneIndex.Island)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
