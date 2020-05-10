using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public static CameraBehaviour Instance;
    public Camera m_camera;
    private float m_startingCameraZ;
    public float m_lowestCameraZoom;
    private List<Transform> m_players = new List<Transform>();
    public float m_cameraFieldOfView = 60;
    public float m_lerpSpeed;

    private Vector3 m_cameraOffset;
    private float m_startingDistance;

    private void Awake()
    {
        Instance = this;
        m_cameraOffset = transform.position;
        m_startingCameraZ = m_camera.transform.localPosition.z;
    }
    public void AssignPlayer(List<GameObject> p_player)
    {
        foreach(GameObject player in p_player)
        {
            m_players.Add(player.transform);
        }

        Vector3 farthestPoint = Vector3.zero;
        float farthestDistance = 0;

        foreach (Transform pos in m_players)
        {
            if (Vector3.Distance(pos.position, Vector3.zero) > farthestDistance)
            {
                farthestDistance = Vector3.Distance(pos.position, Vector3.zero);
                farthestPoint = pos.position;
            }
        }

        foreach (Transform pos in m_players)
        {
            if (Vector3.Distance(pos.position, farthestPoint) > farthestDistance)
            {
                farthestDistance = Vector3.Distance(pos.position, farthestPoint);
            }
        }
        m_startingDistance = farthestDistance;
    }

    // Update is called once per frame
    void Update()
    {
        AdjustCamera();
    }

    private void AdjustCamera()
    {
        Vector3 centerPoint = Vector3.zero;
        Vector3 farthestPoint = Vector3.zero, secondFarthest = Vector3.zero;
        float farthestDistance = 0;

        foreach(Transform pos in m_players)
        {
            centerPoint += pos.position;
            if(Vector3.Distance(pos.position, Vector3.zero) > farthestDistance)
            {
                farthestDistance = Vector3.Distance(pos.position, Vector3.zero);
                farthestPoint = pos.position;
            }
        }
        centerPoint = centerPoint / m_players.Count;

        farthestDistance = 0;

        foreach (Transform pos in m_players)
        {
            if(Vector3.Distance(pos.position, farthestPoint) > farthestDistance)
            {
                secondFarthest = pos.position;
                farthestDistance = Vector3.Distance(pos.position, farthestPoint);
            }

            Debug.DrawLine(pos.position, centerPoint, Color.yellow);
        }

        Vector3 targetPos = new Vector3(centerPoint.x + m_cameraOffset.x, transform.position.y, centerPoint.z + m_cameraOffset.z);

        transform.position = Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, transform.position.z), targetPos, m_lerpSpeed*Time.deltaTime);
        Debug.DrawLine(transform.position, transform.forward * 1000, Color.green);
        float percent = farthestDistance / m_startingDistance;
        m_camera.transform.localPosition = new Vector3(0, 0, Mathf.Lerp(m_startingCameraZ, m_lowestCameraZoom, 1 - percent));


    }
}
