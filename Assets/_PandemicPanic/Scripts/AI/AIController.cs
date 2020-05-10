using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public enum AiState { Wander, Chase, Search, KnockedOut };
    public AiState m_currentState;

    public Transform m_calculateTransform;
    private APRController m_ragdollController;


    public bool m_useGlobalPoints;
    private AIManager m_aiManager;
    public NavMeshAgent m_agent;



    [Header("Movement Properties")]
    public AIArea m_patrolArea;
    public float m_stoppingDistance;
    public float m_patrolSpeed;
    public float m_chaseSpeed;


    private Queue<Transform> m_patrolRoute = new Queue<Transform>();
    private Transform m_currentTargetPatrolPoint;
    private List<Vector3> m_currentPath = new List<Vector3>();
    private NavMeshPath m_path;

    [Header("Rotation")]
    public Transform m_rotatingTransform;
    public float m_minRotationAngle = 5;

    [Header("State Properties")]
    public Vector2 m_faintTime;
    private float m_currentFaintTime, m_faintTimer;
    public float m_startPunchingDistance, m_punchRate;

    private float m_punchTimer = 3f;
    private bool m_leftPunch;

    [Header("Player Detection")]
    public VisionCone m_vision;
    public bool m_punchPlayer;
    private Vector3 m_lastKnownPosition;
    public float m_searchTime;
    private float m_currentSearchTimer;
    private GameObject m_currentTargetPlayer;
    private APRController m_targetPlayerController;




    private void Awake()
    {
        m_ragdollController = GetComponent<APRController>();
        m_agent.enabled = false;
        m_path = new NavMeshPath();
    }
    private void Start()
    {
        if (m_useGlobalPoints)
        {
            m_aiManager = AIManager.Instance;
        }
        GetNewPatrolRoute();

    }

    private void Update()
    {
        m_ragdollController.LeftPickupInput(true);
        m_ragdollController.RightPickupInput(true);
        CheckState();
    }
    /// <summary>
    /// Change The AI State. 
    /// 1 - Patrol | 2 - Chase | 3 - Search | 4 - KO
    /// </summary>
    /// <param name="p_newState"></param>
    public void ChangeState(int p_newState)
    {
        m_ragdollController.LeftPickupInput(false);
        m_ragdollController.RightPickupInput(false);


        if (p_newState == 1)
        {
            m_currentState = AiState.Wander;
            m_currentTargetPlayer = null;
            GetNewPath();
        }
        else if (p_newState == 2 && m_currentState != AiState.KnockedOut)
        {
            if (m_currentState == AiState.Chase) return;
            m_currentState = AiState.Chase;
            m_currentTargetPlayer = m_vision.GetComponent<VisionCone>().GetFirstDetectedPlayer().gameObject;
            m_targetPlayerController = m_currentTargetPlayer.transform.parent.GetComponent<APRController>();
        }
        else if (p_newState == 3 && m_currentState != AiState.KnockedOut)
        {
            m_currentState = AiState.Search;
            m_currentSearchTimer = 0;
            m_lastKnownPosition = m_currentTargetPlayer.transform.position;
            m_currentTargetPlayer = null;
        }
        else if (p_newState == 4)
        {
            m_currentState = AiState.KnockedOut;
            m_faintTimer = 0;
            m_currentFaintTime = Random.Range(m_faintTime.x, m_faintTime.y);
            m_currentTargetPlayer = null;
        }
    }

    private void CheckState()
    {
        switch (m_currentState)
        {
            case AiState.Wander:
                CheckPatrol();
                CheckBalance();
                break;
            case AiState.Chase:
                CheckChase();
                CheckBalance();
                break;
            case AiState.Search:
                CheckSearch();
                CheckBalance();
                break;
            case AiState.KnockedOut:
                CheckKO();
                break;
        }
    }

    private void CheckBalance()
    {
        if (!m_ragdollController.IsBalanced())
        {
            m_ragdollController.ActivateRagdoll();
        }
    }

    #region Movement Functions
    private void MoveToPosition(Vector3 p_point, float p_speed)
    {
        m_ragdollController.MoveSpeed = p_speed;

        Vector3 pos = new Vector3(m_calculateTransform.position.x, 0, m_calculateTransform.position.z);
        Vector3 target = new Vector3(p_point.x, 0, p_point.z);

        Debug.DrawLine(pos, pos + (target - pos).normalized * p_speed, Color.magenta);
        m_ragdollController.Move((target - pos).normalized);

        if (Vector3.Angle(new Vector3(m_rotatingTransform.forward.x, 0, m_rotatingTransform.forward.z).normalized, (target - pos).normalized) > m_minRotationAngle)
        {
            if (Mathf.Sign(Vector3.Dot(new Vector3(m_rotatingTransform.forward.x, 0, m_rotatingTransform.forward.z).normalized, Quaternion.AngleAxis(-90, Vector3.up) * (target - pos).normalized)) > 0)
            {
                m_ragdollController.TurnCharacter(true, false);
            }
            else
            {
                m_ragdollController.TurnCharacter(false, true);
            }
        }
    }

    private bool CloseToPoint(Vector3 p_point, float p_detectionDistance)
    {
        float Dis = Vector3.Distance(new Vector3(m_calculateTransform.position.x, 0, m_calculateTransform.position.z), new Vector3(p_point.x, 0, p_point.z));
        return Vector3.Distance(new Vector3(m_calculateTransform.position.x, 0, m_calculateTransform.position.z), new Vector3(p_point.x, 0, p_point.z)) < p_detectionDistance;
    }
    #endregion

    #region Patrolling State

    private void CheckPatrol()
    {
        if (m_currentPath.Count > 0)
        {
            if (!CloseToPoint(m_currentPath[0], m_stoppingDistance))
            {
                MoveToPosition(m_currentPath[0], m_patrolSpeed);
                return;
            }

        }
        if (m_currentPath.Count > 1)
        {
            m_currentPath.RemoveAt(0);
        }
        else
        {
            GetNewPatrolPoint();
        }
    }

    /// <summary>
    /// Gets the new queue pattern
    /// </summary>
    private void GetNewPatrolRoute()
    {

        m_patrolRoute = (m_useGlobalPoints) ? m_aiManager.GetRandomPatrolRoute() : m_patrolArea.GetRandomPatrolRoute();
        GetNewPatrolPoint();
    }

    /// <summary>
    /// Gets the new patrol point
    /// </summary>
    private void GetNewPatrolPoint()
    {
        if (m_patrolRoute.Count <= 0)
        {
            GetNewPatrolRoute();
        }
        m_currentTargetPatrolPoint = m_patrolRoute.Dequeue();

        GetNewPath();
    }

    /// <summary>
    /// Gets the new path to the new patrol point
    /// </summary>
    private void GetNewPath()
    {
        m_agent.enabled = true;
        m_agent.CalculatePath(m_currentTargetPatrolPoint.position, m_path);
        m_agent.enabled = false;
        m_currentPath.Clear();

        Vector3 debugVector = m_calculateTransform.position;
        foreach (Vector3 corner in m_path.corners)
        {
            Debug.DrawLine(debugVector, corner, Color.green, 5);
            m_currentPath.Add(corner);
            debugVector = corner;
        }

    }

    #endregion


    private void CheckKO()
    {
        if (m_faintTimer > m_currentFaintTime)
        {
            ChangeState(1);
            m_ragdollController.DeactivateRagdoll();

        }
        else
        {
            m_faintTimer += Time.deltaTime;
        }
    }


    private void CheckChase()
    {
        if (m_targetPlayerController.IsKnockedOut())
        {
            ChangeState(1);
            return;
        }


        if (CloseToPoint(m_currentTargetPlayer.transform.position, m_startPunchingDistance))
        {
            if (m_punchPlayer)
            {
                if (m_punchTimer > m_punchRate)
                {
                    m_ragdollController.PunchInput(!m_leftPunch, m_leftPunch);
                    m_leftPunch = !m_leftPunch;
                    m_punchTimer = 0;
                }
            }
        }
        m_punchTimer += Time.deltaTime;

        if (!CloseToPoint(m_currentTargetPlayer.transform.position, m_stoppingDistance))
        {
            if (m_vision.CanSee(m_currentTargetPlayer.gameObject))
            {
                MoveToPosition(m_currentTargetPlayer.transform.position, m_chaseSpeed);
                return;
            }
            else
            {
                ChangeState(3);
            }
        }
    }

    private void CheckSearch()
    {
        if (!CloseToPoint(m_lastKnownPosition, m_stoppingDistance))
        {
            MoveToPosition(m_lastKnownPosition, m_chaseSpeed);
        }
        if (m_currentSearchTimer < m_searchTime)
        {
            m_currentSearchTimer += Time.deltaTime;
        }
        else
        {
            ChangeState(1);
        }
    }
}
