using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float m_dampTime = 0.2f;
    public float m_screenEdgeBuffer = 4f;
    public float m_minSize = 6.5f;
    public Vector3 cameraOffset;

    private List<Transform> m_targets = new List<Transform>();

    private Vector3 m_moveVelocity;
    private Vector3 m_desiredPosition;
    private float m_zoomSpeed;

    private Camera m_Camera;

    private void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>();
    }

    void Start ()
    {
        m_targets = FindAllPlayers();
    }

    private void Update()
    {
        Move();
        Zoom();
    }

    private List<Transform> FindAllPlayers()
    {
        List<Transform> players = new List<Transform>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] partners = GameObject.FindGameObjectsWithTag("Partner");

        if (player != null)
            players.Add(player.transform);

        for (int i = 0; i < partners.Length; i++)
        {
            players.Add(partners[i].transform);
        }
        //Debug.Log(players.Count);
        return players;
    }

    private void Move()
    {
        // Find the average position of the targets.
        FindAveragePosition();

        // Smoothly transition to that position.
        transform.position = Vector3.SmoothDamp(transform.position, m_desiredPosition, ref m_moveVelocity, m_dampTime);
    }

    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        // Go through all the targets and add their positions together.
        for (int i = 0; i < m_targets.Count; i++)
        {
            // If the target isn't active, go on to the next one.
            if (!m_targets[i].gameObject.activeSelf)
                continue;

            // Add to the average and increment the number of targets in the average.
            averagePos += m_targets[i].position;
            numTargets++;
        }

        // If there are targets divide the sum of the positions by the number of them to find the average.
        if (numTargets > 0)
            averagePos /= numTargets;

        // Keep the same y value.
        averagePos.y = transform.position.y;

        // The desired position is the average position;
        m_desiredPosition = averagePos;
    }

    private void Zoom()
    {
        // Find the required size based on the desired position and smoothly transition to that size.
        float requiredSize = FindRequiredSize();
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_zoomSpeed, m_dampTime);
    }

    private float FindRequiredSize()
    {
        // Find the position the camera rig is moving towards in its local space.
        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_desiredPosition);

        // Start the camera's size calculation at zero.
        float size = 0f;

        // Go through all the targets...
        for (int i = 0; i < m_targets.Count; i++)
        {
            // ... and if they aren't active continue on to the next target.
            if (!m_targets[i].gameObject.activeSelf)
                continue;

            // Otherwise, find the position of the target in the camera's local space.
            Vector3 targetLocalPos = transform.InverseTransformPoint(m_targets[i].position);

            // Find the position of the target from the desired position of the camera's local space.
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            // Choose the largest out of the current size and the distance of the tank 'up' or 'down' from the camera.
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

            // Choose the largest out of the current size and the calculated size based on the tank being to the left or right of the camera.
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_Camera.aspect);
        }

        // Add the edge buffer to the size.
        size += m_screenEdgeBuffer;

        // Make sure the camera's size isn't below the minimum.
        size = Mathf.Max(size, m_minSize);

        return size;
    }

    public void SetStartPositionAndSize()
    {
        // Find the desired position.
        FindAveragePosition();

        // Set the camera's position to the desired position without damping.
        transform.position = m_desiredPosition;

        // Find and set the required size of the camera.
        m_Camera.orthographicSize = FindRequiredSize();
    }
}
