using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls character movement
/// </summary>
// "abstract" means that this component cannot used directly on a gameobject.
// we need to make child components that expand upon it (PCMovement, NPCMovement, etc.)!
public abstract class CharacterMovement : MonoBehaviour
{
    public LayerMask platformMask;
    public float speed;
    public float jumpForce;
    // a Rigidbody reference to cache the component
    protected Rigidbody2D m_Rigidbody2D;
    // a Animator reference to cache the component
    protected Animator m_Animator;
    private float m_DistFromGround;
    private Collider2D m_Collider2D;
    private Vector2 m_LowerLeftCorner;
    private Vector2 m_LowerRightCorner;
    private RaycastHit2D[] raycastHits;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        // cache the components using the GetComponent method
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_Collider2D = GetComponent<Collider2D>();
        m_LowerLeftCorner = m_Collider2D.offset +
        new Vector2(-m_Collider2D.bounds.extents.x, -m_Collider2D.bounds.extents.y);
        m_LowerRightCorner = m_Collider2D.offset +
        new Vector2(m_Collider2D.bounds.extents.x, -m_Collider2D.bounds.extents.y);
        raycastHits = new RaycastHit2D[3];
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    protected virtual void Update()
    {
        UpdateAnimator();
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    protected virtual void FixedUpdate()
    {
        CalculatePosition();
    }

    protected virtual void Move(float amount)
    {
        // Preserve the vertical speed by using the original velocity.y
        // and apply the horizontal speed on the x component/axis 
        m_Rigidbody2D.velocity = new Vector2(amount * speed, m_Rigidbody2D.velocity.y);
    }

    protected void Jump()
    {
        // Apply "jumpForce' on the y-axis
        // The second parameter tells Unity to use an impulse force.
        // If left empty, Unity will apply a slow, continuous "push" force by default,
        // which doesn't work for a jump - a impulsive upward force 
        m_Rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }

    private void CalculatePosition()
    {
        var centralRayOrigin = transform.position + Vector3.up * 0.01f;
        var leftRayOrigin = centralRayOrigin + (Vector3)m_LowerLeftCorner;
        var rightRayOrigin = centralRayOrigin + (Vector3)m_LowerRightCorner;

        // Cast a ray from the position of this object, to 5 units down, and only detect objects in platformMask 
        var centerHit2D = raycastHits[0] =
        Physics2D.Raycast(centralRayOrigin, Vector2.down, 1, platformMask.value);
        var leftHit2D = raycastHits[1] =
        Physics2D.Raycast(leftRayOrigin, Vector2.down, 1, platformMask.value);
        var rightHit2D = raycastHits[2] =
        Physics2D.Raycast(rightRayOrigin, Vector2.down, 1, platformMask.value);

        // Use Debug.DrawRay to draw a visaulization of the raycast in the Scene View
        Debug.DrawLine(centralRayOrigin, centralRayOrigin + Vector3.down, Color.red);
        Debug.DrawLine(leftRayOrigin, leftRayOrigin + Vector3.down, Color.red);
        Debug.DrawLine(rightRayOrigin, rightRayOrigin + Vector3.down, Color.red);

        // Reset the distance from ground for new calculation
        m_DistFromGround = 1;
        foreach (var hit in raycastHits)
        {
            // If nothing is hit, character should be at least 1+ units from the ground
            // hence the default value of 1
            float currentDist = 1;
            if (hit.collider != null)
            {
                // If something is detected, store the distance from the result
                currentDist = centerHit2D.distance;
            }
            m_DistFromGround = Mathf.Min(m_DistFromGround, currentDist);
        }
    }

    protected virtual void UpdateAnimator()
    {
        m_Animator.SetFloat("distFromGround", m_DistFromGround);
    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void OnDrawGizmos()
    {
        if (raycastHits != null)
        {
            foreach (var hit in raycastHits)
            {
                // if raycast hits something, draw a little green circle to indicate it
                if (hit.collider != null)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(hit.point, 0.1f);
                }
            }
        }
    }
}



