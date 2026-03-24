using TMPro;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [SerializeField] public float speed = 15f;
    [SerializeField] public float acceleration = 100f; 
    [Header("Air Physics")]
    [SerializeField] public float airControl = 0.2f;
    [SerializeField] public float jumpHeight = 4f;
    [SerializeField] private float coyoteTime = 0.15f;
    [Header("Environment")]
    [SerializeField] public float gravity = -30f;
    [SerializeField] public float groundFriction = 10f; 
    [SerializeField] public float slideFriction = 2f;
    [Header("Debug")]
    [SerializeField] public TextMeshProUGUI speedDebug;

    private CharacterController cc;
    private Vector3 verticalVelocity; 
    private Vector3 horizontalVelocity;
    private float coyoteCounter;

    void Awake() => cc = GetComponent<CharacterController>();

    void Update()
    {
        Vector3 inputDirection = (transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical")).normalized;


        if (cc.isGrounded)
        {
            if (horizontalVelocity.magnitude > 0)
            {
                horizontalVelocity -= horizontalVelocity * groundFriction * Time.deltaTime;
            }
        }

        if (inputDirection.magnitude > 0.1f)
        {
            float currentSpeedInDirection = Vector3.Dot(horizontalVelocity, inputDirection);
            float addSpeed = speed - currentSpeedInDirection;

            if (addSpeed > 0)
            {
                float accelAmount = acceleration * Time.deltaTime;

                if (!cc.isGrounded)
                {
                    accelAmount *= airControl;
                }

                accelAmount = Mathf.Min(accelAmount, addSpeed);
                horizontalVelocity += inputDirection * accelAmount;
            }
        }

        if (cc.isGrounded)
        {
            coyoteCounter = coyoteTime;
            if (verticalVelocity.y < 0) verticalVelocity.y = -2f;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        if (Input.GetButton("Jump") && coyoteCounter > 0)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            coyoteCounter = 0f;
        }
        verticalVelocity.y += gravity * Time.deltaTime;

        cc.Move((horizontalVelocity + verticalVelocity) * Time.deltaTime);
        int displaySpeed = Mathf.RoundToInt(horizontalVelocity.magnitude);
        speedDebug.text = displaySpeed.ToString();
    }
}