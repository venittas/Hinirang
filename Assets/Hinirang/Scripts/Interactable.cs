using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    GameObject indicator = null;
    public Rigidbody2D rb;
    public Animator animator;
    public void Start()
    {
        indicator = transform.Find("Indicator")?.gameObject;
        if(indicator != null)
        {
            indicator.SetActive(false);
        }
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Debug.Log("RB: " + rb);
        Debug.Log("Animator: " + animator);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowIndicator()
    {
        indicator.SetActive(true);
    }

    public void HideIndicator()
    {
        indicator.SetActive(false);
    }

    public abstract void Interact(string eventName);
}
