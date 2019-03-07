using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Non-player Character Movement
/// </summary>
public class NPCMovement : CharacterMovement
{
    [Range(-1, 1)]
    [Tooltip("The current direction the NPC is going to")]
    public int direction;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Move(direction);
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        // Note: DO NOT mix this up with OnCollisionEnter(Collision other).
        // This works with 2D colliders only
        if (m_IsGrounded && !other.gameObject.CompareTag("Player"))
        {
            // if collided with another non-player object when grounded,
            // flip movement direction
            direction = -direction;
        }
    }

    protected override void UpdateAnimator()
    {
        base.UpdateAnimator();
        m_Animator.SetFloat("movementX", direction);
    }
}
