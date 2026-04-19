using UnityEngine;

public class Boat : MonoBehaviour
{
    private static Rigidbody2D rb;

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
}
