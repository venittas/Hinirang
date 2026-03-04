using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    GameObject indicator = null;
    public void Start()
    {
        indicator = transform.Find("Indicator")?.gameObject;
        if(indicator != null)
        {
            indicator.SetActive(false);
        }
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

    public abstract void Interact();
}
