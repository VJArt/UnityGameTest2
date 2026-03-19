using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [SerializeField] public float speed = 5f;
    [SerializeField] public float acceleration = 15f;
    [Header("")]
    [SerializeField] public float airControl = 0.5f;
    [SerializeField] public float jumpHeight = 2f;
    [SerializeField] private float coyoteTime = 0.15f;
    [Header("")]
    [SerializeField] public float gravity = -10f;
    [SerializeField] public float friction = 10f;
    [Header("")]
    [SerializeField] public float slideFriction = 2f;
    [SerializeField] public float slideSpeed = 20f;

    private CharacterController cc;
    private Vector3 velocity;
    private Vector3 horizontalVelocity;
    private float coyoteCounter;
    private bool isSliding = false;

    void Awake()
    {
       cc=GetComponent<CharacterController>(); 
    }

    void Update()
    {
        Vector3 inputDirection = (transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical"));

        float control = cc.isGrounded ? 1f : airControl;

        horizontalVelocity += inputDirection * acceleration * control * Time.deltaTime;

        if (cc.isGrounded)
        {
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }
        if (horizontalVelocity.magnitude > speed)
        {
            horizontalVelocity = horizontalVelocity.normalized * speed;
        }
        if (cc.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        if (Input.GetButton("Jump") && coyoteCounter > 0)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            coyoteCounter = 0f;
        }
        if (cc.isGrounded && inputDirection.magnitude <0.1f)
        {
            float drag = Mathf.Max(horizontalVelocity.magnitude-friction*Time.deltaTime, 0f);   
            horizontalVelocity = horizontalVelocity.normalized * drag;  
        }

        velocity.y += gravity * Time.deltaTime;
        cc.Move((horizontalVelocity+velocity) *Time.deltaTime);
    }
}
