using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    [Header("Slow Movement")]
    [Tooltip("Movement to slow down when crouched.")]
    [SerializeField] PlayerMovement movement;
    float crouchSpeed = 2f;

    [Header("Low Head")]
    [Tooltip("Head to lower when crouched.")]
    public Transform headToLower;
    [HideInInspector] public float? defaultHeadYLocalPosition;
    [HideInInspector] public float crouchYHeadPosition = 1;
    
    [Tooltip("Collider to lower when crouched.")]
    [HideInInspector] public CapsuleCollider colliderToLower;
    [HideInInspector] public float? defaultColliderHeight;

    [HideInInspector] public bool IsCrouched { get; private set; }
    public event System.Action CrouchStart, CrouchEnd;

    void Reset()
    {
        movement = GetComponentInParent<PlayerMovement>();
        headToLower = movement.GetComponentInChildren<Camera>().transform;
        colliderToLower = movement.GetComponentInChildren<CapsuleCollider>();
    }

    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.C))
        {
            if (headToLower)
            {
                if (!defaultHeadYLocalPosition.HasValue)
                    defaultHeadYLocalPosition = headToLower.localPosition.y;

                headToLower.localPosition = new Vector3(headToLower.localPosition.x, crouchYHeadPosition, headToLower.localPosition.z);
            }

            if (colliderToLower)
            {
                if (!defaultColliderHeight.HasValue)
                    defaultColliderHeight = colliderToLower.height;
   
                float loweringAmount;
                if(defaultHeadYLocalPosition.HasValue)
                    loweringAmount = defaultHeadYLocalPosition.Value - crouchYHeadPosition;
                else
                    loweringAmount = defaultColliderHeight.Value * .5f;

                colliderToLower.height = Mathf.Max(defaultColliderHeight.Value - loweringAmount, 0);
                colliderToLower.center = Vector3.up * colliderToLower.height * .5f;
            }

            if (!IsCrouched)
            {
                IsCrouched = true;
                SetSpeedOverrideActive(true);
                CrouchStart?.Invoke();
            }
        }
        else
        {
            if (IsCrouched)
            {
                if (headToLower)
                    headToLower.localPosition = new Vector3(headToLower.localPosition.x, defaultHeadYLocalPosition.Value, headToLower.localPosition.z);

                if (colliderToLower)
                {
                    colliderToLower.height = defaultColliderHeight.Value;
                    colliderToLower.center = Vector3.up * colliderToLower.height * .5f;
                }

                IsCrouched = false;
                SetSpeedOverrideActive(false);
                CrouchEnd?.Invoke();
            }
        }
    }


    #region Speed override.
    void SetSpeedOverrideActive(bool state)
    {
        if(!movement)
            return;

        if (state)        
        {
            if (!movement.speedOverrides.Contains(SpeedOverride))
                movement.speedOverrides.Add(SpeedOverride);
        }
        else
        {
            if (movement.speedOverrides.Contains(SpeedOverride))
                movement.speedOverrides.Remove(SpeedOverride);
        }
    }

    float SpeedOverride() => crouchSpeed;
    #endregion
}
