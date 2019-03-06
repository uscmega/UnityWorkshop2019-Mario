using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Playable Character movement
/// </summary>
public class PCMovement : CharacterMovement
{
    public float acceleration = 20;
    int numJumps = 1;
    private float m_HorizontalInput;
    private int m_FaceDirection;
    private bool m_jumpTrigger;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        // Whenever "W" is pressed, trigger a jump
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            // catch the jump key to use it in the Fixed/Physics update
            m_jumpTrigger = true;
        }


        m_HorizontalInput = Input.GetAxis("Horizontal");
        if (m_HorizontalInput != 0)
        {
            m_FaceDirection = (int)Mathf.Sign(m_HorizontalInput);
        }
        if (m_FaceDirection < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    protected override void FixedUpdate()
    {
        if(m_jumpTrigger)
        {
            Jump();
            m_jumpTrigger = false;
        }
        Move(m_HorizontalInput);
    }

    protected override void Move(float amount)
    {
        // Apply a continuous force to the rigidbody
        m_Rigidbody2D.AddForce(Vector2.right * acceleration * amount);
        var currentSpeed = Mathf.Abs(m_Rigidbody2D.velocity.x);
        if (currentSpeed > speed)
        {
            // If the actual speed exceeds the speed limit
            // Apply force in the opposite direction
            Debug.Log("delta: " + (speed - currentSpeed));
            m_Rigidbody2D.AddForce(Vector2.left * m_FaceDirection * (currentSpeed - speed));
        }
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.name);
    }

    protected override void UpdateAnimator()
    {
        base.UpdateAnimator();
        m_Animator.SetFloat("speed", Mathf.Abs(m_HorizontalInput) * speed);
    }
}
