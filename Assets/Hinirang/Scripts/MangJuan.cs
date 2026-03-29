using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;

public class MangJuan : NPCScript
{

    [SerializeField] private Boat Boat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
        Scene3();
    }

    private void Scene3()
    {
        StartCoroutine(Scene3Routine());
    }

    private IEnumerator Scene3Routine()
    {
        yield return StartCoroutine(Move(0));
        yield return StartCoroutine(Move(1));
        yield return StartCoroutine(Move(2));
        StopMovement();
        Debug.Log("Scene 3");
        yield return StartCoroutine(MoveWithBoat());
        
    }

    private IEnumerator MoveWithBoat()
    {
        Boat.MoveBoat();
        yield return StartCoroutine(MoveNoAnim(3));
    }

}
