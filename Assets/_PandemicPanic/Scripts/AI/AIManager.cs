using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{

    public static AIManager Instance;
    public List<Transform> m_patrolPoints;
    public List<Transform> m_zombieSpawnPoints;

    public GameObject m_zombiePrefab;
    public Vector2 m_spawnTime;
    private float m_timer, m_currentSpawnTime;
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (m_timer > m_currentSpawnTime)
        {
            m_timer = 0;
            m_currentSpawnTime = Random.Range(m_spawnTime.x, m_spawnTime.y);
            //SpawnAi();
        }
    }

    public void SpawnAi()
    {
        int random = Random.Range(0, m_zombieSpawnPoints.Count);
        Instantiate(m_zombiePrefab, m_zombieSpawnPoints[random].position, m_zombieSpawnPoints[random].rotation);
    }
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
        foreach(Transform newPoint in m_patrolPoints)
        {
            newPath.Enqueue(newPoint);
        }
        return newPath;
    }
}
