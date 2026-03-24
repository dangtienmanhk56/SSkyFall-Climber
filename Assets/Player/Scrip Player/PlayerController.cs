using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 5f;

    [Header("Jump")]
    public float jumpForce = 7f;
    public float maxChargeTime = 1.5f;

    private float chargeTime;
    private bool isCharging = false;
    private bool isGrounded;

    private Rigidbody2D rb;
    private Animator anim;

    private float moveInput;
    private int facingDirection = 1; // 1 = phải, -1 = trái

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Jump();

        // cập nhật animation speed (Idle/Run)
        anim.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x));
    }

    void Move()
    {
        moveInput = 0;

        // 👉 Nếu đang gồng → KHÓA di chuyển
        if (isCharging)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        // 👉 Nếu đang bay → cũng không điều khiển
        if (!isGrounded) return;

        if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1;
            Flip(-1);
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1;
            Flip(1);
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isCharging = true;
            chargeTime = 0;

            anim.SetBool("isJumping", true);
        }

        if (Input.GetKey(KeyCode.Space) && isCharging)
        {
            chargeTime += Time.deltaTime;
            chargeTime = Mathf.Clamp(chargeTime, 0, maxChargeTime);
        }

        if (Input.GetKeyUp(KeyCode.Space) && isCharging)
        {
            float power = chargeTime / maxChargeTime;

            float jumpY = jumpForce * power;
            float jumpX = facingDirection * jumpForce * power;

            rb.linearVelocity = new Vector2(0, 0);
            rb.AddForce(new Vector2(jumpX * 100, jumpY * 200));

            isCharging = false;
            isGrounded = false;
        }
    }

    void Flip(int direction)
    {
        facingDirection = direction;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetBool("isJumping", false);
        }
    }
}