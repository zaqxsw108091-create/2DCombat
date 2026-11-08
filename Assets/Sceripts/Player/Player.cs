using System;
using System.Collections;
using UnityEngine;



public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7.5f;
    [SerializeField] private float jumpPower = 7f;
    [SerializeField, Range(0f, 1f)] private float jumpCutMultiplier = 0.5f; // rise kept when the jump key is released early

    [SerializeField] private float extraHeight = 0.25f;
    [SerializeField] private LayerMask whatIsGround;

    Rigidbody2D rigid;
    Animator anim;
    Collider2D coll;


    // Todo : 코드 테스트를 위해서 인스팩터에 노출시킴, 확인 후 지울 것
    [SerializeField] private bool isFacingRight = false;
    private bool isjumping;
    private bool isfalling;
    private RaycastHit2D groundHit;

    private Coroutine resetTrigger;

    [Header("Dash")]
    private bool isDashing;
    private TrailRenderer trailRenderer;    

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();
        DashSkill.OnSkillUsed += Dashing;
    }

    private void OnDestroy()
    {
        DashSkill.OnSkillUsed -= Dashing;
    }

    private void Update()
    {
        if (isDashing) return;
        Move();
        Jump();
    }

    private void Dashing(float DashPower, float DashTime)
    {
        StartCoroutine(DashEnable(DashPower,DashTime));
    }

    private IEnumerator DashEnable(float DashPower, float DashTime)
    {
        float faceValue = isFacingRight ? 1 : -1;
        isDashing = true;
        float originGravity = rigid.gravityScale;
        rigid.gravityScale = 0;
        rigid.velocity = new Vector2(faceValue * DashPower, 0);

        trailRenderer.emitting = true;
        yield return new WaitForSeconds(DashTime);
        trailRenderer.emitting = false;
        rigid.gravityScale = originGravity;
        isDashing = false;
    }
    private void Move()
    {
        // 입력 Control
        float horizontal = InputUser.Instance.moveInput.x;

        // 입력값이 있었는가?
        if (Math.Abs(horizontal) > 0) // 입력값이 있을 떄
        {
            anim.SetBool("isWalking", true);
            TurnCheck();
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        // rigidbody2D
        rigid.velocity = new Vector2(horizontal * moveSpeed, rigid.velocity.y);
    }

    #region Flip

    private void TurnCheck()
    {
        if (InputUser.Instance.moveInput.x > 0 && !isFacingRight) // 정면
        {
            Turn();
        }
        else if (InputUser.Instance.moveInput.x < 0 && isFacingRight) // 후면
        {
            Turn();
        }
    }

    private void Turn()
    {
        if (isFacingRight) // 정면 -> 후면
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
        else // 후면 -> 정면
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
    }
    #endregion

    private void Jump()
    {
        var jump = InputUser.Instance.control.Jumping.Jump;

        // Jump starts only on the key PRESS while grounded.
        if (jump.WasPressedThisFrame() && IsGrounded())
        {
            isjumping = true;
            isfalling = false;
            rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
        }

        // Variable jump height: releasing the key while still rising cuts the rise,
        // so a short tap jumps lower and a held key reaches the same max height as before.
        if (isjumping && jump.WasReleasedThisFrame())
        {
            if (rigid.velocity.y > 0f)
                rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * jumpCutMultiplier);
            isjumping = false;
            isfalling = true;
        }

        // Reached the top of the jump.
        if (isjumping && rigid.velocity.y <= 0f)
        {
            isjumping = false;
            isfalling = true;
        }
    }
    private bool CheckForLand()
    {
        if(isfalling)
        {
            if(IsGrounded())
            {
                isfalling = false;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    private bool IsGrounded()
    {
        groundHit = Physics2D.BoxCast(coll.bounds.center,coll.bounds.size,0f, Vector2.down,extraHeight, whatIsGround);

        if(groundHit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
         

    }

    #region Debug Function
    private void DrawGroundCheck()
    {

        Color rayColor = Color.green;
        if(IsGrounded() )
        {
            rayColor = Color.green;
        }
        Debug.DrawRay(coll.bounds.center +
        new Vector3(coll.bounds.extents.x, 0), Vector2.down * (coll.bounds.extents.y + extraHeight), rayColor);

        Debug.DrawRay(coll.bounds.center -
          new Vector3(coll.bounds.extents.x, 0), Vector2.down * (coll.bounds.extents.y + extraHeight), rayColor);

        Debug.DrawRay(coll.bounds.center - new Vector3(coll.bounds.extents.x, coll.bounds.extents.y + extraHeight),
      Vector2.right * (coll.bounds.extents.x * 2), rayColor);
    }

    #endregion
}
