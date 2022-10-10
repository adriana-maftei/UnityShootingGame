using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    Rigidbody rb;
    float jumpStrength = 1.5f;
    public event System.Action Jumped;

    [SerializeField, Tooltip("Prevents jumping when the transform is in mid-air")]
    GroundCheck groundCheck;

    void Reset() => groundCheck = GetComponentInChildren<GroundCheck>();
    void Awake() => rb = GetComponent<Rigidbody>();

    void LateUpdate()
    {
        if (Input.GetButtonDown("Jump") && (!groundCheck || groundCheck.isGrounded))
        {
            rb.AddForce(Vector3.up * 100 * jumpStrength);
            Jumped?.Invoke();
        }
    }
}
