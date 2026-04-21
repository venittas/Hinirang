using UnityEngine;

public class Boat : MonoBehaviour
{
    private static Rigidbody2D rb;
    public static Boat Instance;

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
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void MoveBoat()
    {
        rb.linearVelocity = Vector3.left * 5f;
        Invoke("StopBoat", 5f);
    }

    private void StopBoat()
    {
        rb.linearVelocity = Vector3.zero;
    }
    public void DisableBoat()
    {
        gameObject.SetActive(false);
    }
}
