using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    float walkSpeed = 6f;

    [Header("Running")]
    [HideInInspector] public bool canRun = true;
    [HideInInspector] public bool IsRunning { get; private set; }
    float runSpeed = 8f;

    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        IsRunning = canRun && Input.GetKey(KeyCode.LeftShift);

        float targetMovingSpeed = IsRunning ? runSpeed : walkSpeed;
        if (speedOverrides.Count > 0)
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();

        Vector2 targetVelocity =new Vector2( Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);
        rb.velocity = transform.rotation * new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.y);
    }
}