using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public static CameraBehaviour Instance;
    public Camera m_camera;

    [Tooltip("Changing this will change the starting Camera postiion")]
    public float m_lowestCameraZoom;
    public float m_farthestCameraZoon;
    public float m_maxCharacterDistance;

    private List<Transform> m_players = new List<Transform>();
    public float m_cameraFieldOfView = 60;
    public float m_horizontalLerpSpeed, m_zoomLerpSpeed;

    private Vector3 m_cameraOffset;
    private float m_startingDistance;

    private void Awake()
    {
        Instance = this;
        m_cameraOffset = transform.position;
        m_camera.transform.localPosition = new Vector3(0, 0, m_lowestCameraZoom);
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

        #region Horizontal Movement Adjustment
        foreach (Transform pos in m_players)
        {
            centerPoint += pos.position;
            if(Vector3.Distance(pos.position, Vector3.zero) > farthestDistance)
            {
                farthestDistance = Vector3.Distance(pos.position, Vector3.zero);
                farthestPoint = pos.position;
            }
        }
        centerPoint = centerPoint / m_players.Count;
        Vector3 targetPos = new Vector3(centerPoint.x + m_cameraOffset.x, transform.position.y, centerPoint.z + m_cameraOffset.z);
        transform.position = Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, transform.position.z), targetPos, m_horizontalLerpSpeed * Time.deltaTime);

        #endregion

        #region Zoom Adjustment
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

        
        Debug.DrawLine(transform.position, transform.forward * 1000, Color.green);
        targetPos = new Vector3(0, 0, Mathf.Lerp(m_lowestCameraZoom, m_farthestCameraZoon, farthestDistance / m_maxCharacterDistance));
        m_camera.transform.localPosition = Vector3.Lerp(m_camera.transform.localPosition, targetPos, m_zoomLerpSpeed * Time.deltaTime);
        #endregion

    }
}
