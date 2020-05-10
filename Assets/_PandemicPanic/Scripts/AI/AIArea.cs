using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIArea : MonoBehaviour
{
    public List<Transform> m_patrolPoints;

    public Queue<Transform> GetRandomPatrolRoute()
    {
        for (int i = 0; i < m_patrolPoints.Count; i++)
        {
            Transform temp = m_patrolPoints[i];
            int random = Random.Range(0, m_patrolPoints.Count);
            m_patrolPoints[i] = m_patrolPoints[random];
            m_patrolPoints[random] = temp;
        }
        Queue<Transform> newPath = new Queue<Transform>();
        foreach (Transform newPoint in m_patrolPoints)
        {
            newPath.Enqueue(newPoint);
        }
        return newPath;
    }
}
