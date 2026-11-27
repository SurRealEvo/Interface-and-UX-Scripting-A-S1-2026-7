
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 12f;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference parryAction;

    [Header("Parry Settings")]
    [SerializeField] private float parryBounceForce = 14f;

    private ParryableObject nearbyParryObject;

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private bool isGrounded;
    private PlayerParryFeedback parryFeedback;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        parryFeedback = GetComponent<PlayerParryFeedback>();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        parryAction.action.Enable();
        parryAction.action.performed += OnParry;

        jumpAction.action.performed += OnJump;
    }

    private void OnDisable()
    {
        jumpAction.action.performed -= OnJump;
        parryAction.action.performed -= OnParry;
        parryAction.action.Disable();

        moveAction.action.Disable();
        jumpAction.action.Disable();
    }

    private void Update()
    {
        movementInput = moveAction.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(
            movementInput.x * moveSpeed,
            rb.linearVelocity.y
        );
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

            isGrounded = false;
        }
    }

    private void OnParry(InputAction.CallbackContext context)
    {
        bool successful = false;

        if (nearbyParryObject != null)
        {
            successful = nearbyParryObject.TryParry(
                rb, parryBounceForce
            );
        }

        parryFeedback.PlayParryEffect(successful);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        ParryableObject parryObject = other.GetComponent<ParryableObject>();

        if (parryObject != null)
        {
            nearbyParryObject = parryObject;
            parryObject.SetPlayerInRange(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ParryableObject parryObject = other.GetComponent<ParryableObject>();

        if (parryObject != null && nearbyParryObject == parryObject)
        {
            nearbyParryObject = null;
            parryObject.SetPlayerInRange(false);
            Debug.Log("Parry object left range!");
        }
    }
}


