using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioCamera : MonoBehaviour
{
    public Transform target;
    public BoxCollider2D leftBoundCollider;
    private Vector3 m_DesiredPosition;
    private Camera m_Camera;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        m_Camera = GetComponent<Camera>();
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        // set the initial position to the camera's position
        m_DesiredPosition = transform.position;
        leftBoundCollider.transform.localPosition = Vector3.zero;
        leftBoundCollider.size = new Vector2(1, 2 * m_Camera.orthographicSize);
        leftBoundCollider.offset = new Vector2(
            -m_Camera.aspect * m_Camera.orthographicSize - 0.5f, 0);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // Calculate the desired position
        if (target.position.x > transform.position.x)
        {
            m_DesiredPosition.x = target.position.x;
            m_DesiredPosition.z = transform.position.z;
            m_DesiredPosition.y = transform.position.y;
        }

        // Apply the position to the camera
        transform.position = m_DesiredPosition;
    }
}
