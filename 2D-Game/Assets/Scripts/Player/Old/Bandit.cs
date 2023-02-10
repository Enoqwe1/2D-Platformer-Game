using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Bandit : MonoBehaviour
{
    #region Private Variables
    private Quiz quiz;
    private PlayerStats PS;
    private Animator anim;
    private Rigidbody2D rb;
    [HideInInspector]
    public float lastDash = -100f;
    [SerializeField]
    private Vector2 knockbackSpeed;
    private Vector2
        ledgePosBot,
        ledgePos1,
        ledgePos2;
    private static Bandit instance;
    [SerializeField]
    private float knockbackDuration;
    private float
        movementInputDirection,
        jumpTimer,
        turnTimer,
        wallJumpTimer,
        dashTimeLeft,
        lastImageXpos,
        knockbackStartTime;
    private bool
        isWalking,
        isFacingRight = true,
        isGrounded,
        isTouchingWall,
        canNormalJump,
        canWallJump,
        isWallSliding,
        isAttemtingtoJump,
        checkJumpMultiplier,
        canMove,
        canFlip,
        hasWallJumped,
        isTounchingLedge,
        canClimbLedge = false,
        ledgeDetected,
        isDashing,
        knockback;

    private int
        amountOfJumpsLeft,
        facingDirection = 1,
        lastWallJumpDirection;
    #endregion


    #region Public Variables
    public Slider slider;
    public ParticleSystem dust;
    public LayerMask whatIsGround;
    public Transform groundCheck;
    public Transform wallCheck;
    public Transform ledgeCheck;
    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;
    public float movementSpeed = 8.0f;
    public float jumpForce = 16f;
    public float groundCheckRadius;
    public float variableJumpHeightMultiplier = 0.5f;
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float airDragMultiplier = 0.95f;
    public float wallJumpForce;
    public float jumpTimerSet = 0.15f;
    public float turnTimerSet = 0.1f;
    public float wallJumpTimerSet = 0.5f;
    public float ledgeClimbXOffset1 = 0f;
    public float ledgeClimbXOffset2 = 0f;
    public float ledgeClimbYOffset1 = 0f;
    public float ledgeClimbYOffset2 = 0f;
    public float dashTime = 0.2f;
    public float dashSpeed = 50.0f;
    public float distanceBetweenImages = 0.1f;
    public float dashCoolDown = 2.5f;
    public int amountOfJumps = 1;
    public static bool m_isDead = false;
    #endregion


    // Use this for initialization
    private void Start()
    {
        /*if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);*/
        PS = GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        amountOfJumpsLeft = amountOfJumps;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
        quiz = FindObjectOfType<Quiz>();
        //m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
    }
    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckJump();
        CheckLedgeClimb();
        CheckDash();
        CheckKnockback();
    }
    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurrondings();
    }
    public int GetFacingDirection()
    {
        return facingDirection;
    }
    public bool GetDashStatus()
    {
        return isDashing;
    }
    public void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }
    private void CheckKnockback()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }
    private void CheckIfWallSliding()
    {
        if (isTouchingWall && movementInputDirection == facingDirection && rb.velocity.y < 0 && !canClimbLedge)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }
    private void CheckLedgeClimb()
    {
        if (ledgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;
            if (isFacingRight)
            {
                ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) - ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) + ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }
            else
            {
                ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) + ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) - ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }
            canMove = false;
            canFlip = false;
            anim.SetBool("canClimbLedge", canClimbLedge);
        }
        if (canClimbLedge)
        {
            transform.position = ledgePos1;
        }
    }
    public void FinishLedgeClimb()
    {
        canClimbLedge = false;
        transform.position = ledgePos2;
        canMove = true;
        canFlip = true;
        ledgeDetected = false;
        anim.SetBool("canClimbLedge", canClimbLedge);
    }
    private void CheckSurrondings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
        isTounchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, wallCheckDistance, whatIsGround);
        if (isTouchingWall && !isTounchingLedge && !ledgeDetected)
        {
            ledgeDetected = true;
            ledgePosBot = wallCheck.position;
        }
    }
    private void CheckIfCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0.01f)
        {
            amountOfJumpsLeft = amountOfJumps;
        }
        if (isTouchingWall)
        {
            canWallJump = true;
        }
        if (amountOfJumpsLeft <= 0)
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }
    }
    private void CheckMovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }
        if (!quiz.canPlayerMove)
        {
            rb.velocity = new Vector2(0.0f, 0.0f);
        }
        else
        {
            if (Mathf.Abs(rb.velocity.x) >= 0.01f)
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }
        }

    }
    private void UpdateAnimations()
    {
        if (!quiz.canPlayerMove)
        {
            isWalking = false;
        }
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        //Set AirSpeed in animator
        anim.SetFloat("AirSpeed", rb.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
    }
    private void CheckInput()
    {
        if (quiz.canPlayerMove)
        {
            movementInputDirection = Input.GetAxis("Horizontal");

            if (Input.GetButtonDown("Jump"))
            {
                if (isGrounded || (amountOfJumpsLeft > 0 && !isTouchingWall))
                {
                    NormalJump();
                }
                else
                {
                    jumpTimer = jumpTimerSet;
                    isAttemtingtoJump = true;
                }
            }
            if (Input.GetButtonDown("Horizontal") && isTouchingWall)
            {
                if (!isGrounded && movementInputDirection != facingDirection)
                {
                    canMove = false;
                    canFlip = false;

                    turnTimer = turnTimerSet;
                }
            }
            if (turnTimer >= 0)
            {
                turnTimer -= Time.deltaTime;
                if (turnTimer <= 0)
                {
                    canMove = true;
                    canFlip = true;
                }
            }
            if (checkJumpMultiplier && !Input.GetButton("Jump"))
            {
                checkJumpMultiplier = false;
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
            }
            if (Input.GetButtonDown("Dash"))
            {
                if (Time.time >= (lastDash + dashCoolDown))
                {
                    AttemToDash();
                }
            }
        }
    }
    private void AttemToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }
    private void CheckDash()
    {
        if (isDashing)
        {
            FindObjectOfType<DashSlider>().Dashing();
            if (dashTimeLeft > 0)
            {
                canMove = false;
                canFlip = false;
                rb.velocity = new Vector2(dashSpeed * facingDirection, rb.velocity.y);
                dashTimeLeft -= Time.deltaTime;
                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }
            if (dashTimeLeft <= 0 || isTouchingWall)
            {
                isDashing = false;
                canMove = true;
                canFlip = true;
            }
        }
    }
    private void ApplyMovement()
    {
        if (quiz.canPlayerMove)
        {
            if (!isGrounded && !isWallSliding && movementInputDirection == 0 && !knockback)
            {
                rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
            }
            else if (canMove && !knockback)
            {
                rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
            }
            if (isWallSliding)
            {
                if (rb.velocity.y < -wallSlideSpeed)
                {
                    rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
                }
            }
        }

    }
    private void CheckJump()
    {
        if (jumpTimer > 0)
        {
            if (!isGrounded && isTouchingWall && movementInputDirection != 0 && movementInputDirection != facingDirection)
            {
                WallJump();
            }
            else if (isGrounded)
            {
                NormalJump();
            }
        }
        if (isAttemtingtoJump)
        {
            jumpTimer -= Time.deltaTime;
        }
        if (wallJumpTimer > 0)
        {
            if (hasWallJumped && movementInputDirection == -lastWallJumpDirection)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                hasWallJumped = false;
            }
            else if (wallJumpTimer <= 0)
            {
                hasWallJumped = false;
            }
            else
            {
                wallJumpTimer = Time.deltaTime;
            }
        }
    }
    private void NormalJump()
    {
        if (canNormalJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
            jumpTimer = 0;
            isAttemtingtoJump = false;
            checkJumpMultiplier = true;
        }
    }
    private void WallJump()
    {
        if (canWallJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            isWallSliding = false;
            amountOfJumpsLeft = amountOfJumps;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            jumpTimer = 0;
            isAttemtingtoJump = false;
            checkJumpMultiplier = true;
            turnTimer = 0;
            canMove = true;
            canFlip = true;
            hasWallJumped = true;
            wallJumpTimer = wallJumpTimerSet;
            lastWallJumpDirection = -facingDirection;
        }
    }
    public void DisableFlip()
    {
        canFlip = false;
    }
    public void EnableFlip()
    {
        canFlip = true;
    }
    private void Flip()
    {
        if (!isWallSliding && canFlip && !knockback)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }

    }
    /*
    private void OnLevelWasLoaded(int level)
    {
        FindStartPos();

        players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length > 1)
        {
            Destroy(players[1]);
        }
    }*/
    void FindStartPos()
    {
        transform.position = GameObject.FindWithTag("StartPos").transform.position;
    }
    void CreatDust()
    {
        dust.Play();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}
