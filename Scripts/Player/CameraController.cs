﻿using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [Header("Camera configurations")]
    float sensitivity = 2;
    float smoothing = 1.5f;
    Vector2 velocity;
    Vector2 frameVelocity;

    void Reset() => player = GetComponentInParent<PlayerMovement>().transform;
    void Start() => Cursor.lockState = CursorLockMode.Locked;
    void Update()
    {
        // Get smooth velocity.
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -20, 20);

        // Rotate camera up-down and controller left-right from velocity.
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        player.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
    }
}
