using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private float horizontalMoveInput;
    private float moveSpeed = 15f;
    private float jumpingPower = 20f;
    private bool isFacingRight = true;

    private float acceleration = 2f;
    private float deceleration = 2f;
    private float velocityPower = 2f;

    // Update is called once per frame
    void Update()
    {
        if (!isFacingRight && horizontalMoveInput > 0f)
        {
            Flip();
        }
        else if (isFacingRight && horizontalMoveInput < 0f)
        {
            Flip();
        }
    }

    public void FixedUpdate()
    {
        #region Run

        // Calculate the direction we want to move in and our desired velocity
        float targetSpeed = horizontalMoveInput * moveSpeed;

        // Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - rb.velocity.x;

        // Change acceleration rate depending on situation
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

        // Applies acceleration to speed difference, then raises to a set power so acceleration increases with higher speeds
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velocityPower) * Mathf.Sign(speedDif);

        // Applies force to RigidBody, multiplying by Vector2.Right so that it only affects X axis
        rb.AddForce(movement * Vector2.right);

        #endregion
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMoveInput = context.ReadValue<Vector2>().x;
    }
}
