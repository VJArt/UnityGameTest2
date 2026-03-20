using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [SerializeField] public float speed = 15f;
    [SerializeField] public float acceleration = 100f; // Increased for snappier start
    [Header("Air Physics")]
    [SerializeField] public float airControl = 0.2f; // How much friction applies in air
    [SerializeField] public float jumpHeight = 4f;
    [SerializeField] private float coyoteTime = 0.15f;
    [Header("Environment")]
    [SerializeField] public float gravity = -30f;
    [SerializeField] public float groundFriction = 10f; // Tune this down (try 10-20)
    [SerializeField] public float slideFriction = 2f;

    private CharacterController cc;
    private Vector3 verticalVelocity; // Renamed for clarity
    private Vector3 horizontalVelocity;
    private float coyoteCounter;

    void Awake() => cc = GetComponent<CharacterController>();

    void Update()
    {
        // 1. GET INPUT
        Vector3 inputDirection = (transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical")).normalized;

        // 2. APPLY FRICTION (The "Tax")
        // Instead of stopping when keys are released, we always bleed speed.
        float currentFriction = cc.isGrounded ? groundFriction : slideFriction;

        // This math reduces velocity magnitude by a percentage over time
        if (horizontalVelocity.magnitude > 0)
        {
            horizontalVelocity -= horizontalVelocity * currentFriction * Time.deltaTime;
        }

        // 3. APPLY ACCELERATION (The "Engine")
        if (inputDirection.magnitude > 0.1f)
        {
            // MoveTowards allows us to reach 'speed' instantly without CAPING momentum.
            // If horizontalVelocity is 50, MoveTowards(50, 15) won't do anything 
            // unless you change direction.
            horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, inputDirection * speed, acceleration * Time.deltaTime);
        }

        // 4. VERTICAL LOGIC (Coyote Time & Gravity)
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

        // 5. FINAL EXECUTION
        cc.Move((horizontalVelocity + verticalVelocity) * Time.deltaTime);
    }
}