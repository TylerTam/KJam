using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance;

    private List<int> m_playerScores;

    public List<Transform> m_spawnPoints;

    private void Awake()
    {
        Instance = this;

    }
    private void Start()
    {
        PlayerManager.Instance.SpawnPlayers(m_spawnPoints);
    }

    public void AddScore(int p_playerId, int p_scoreToAdd)
    {
        m_playerScores[p_playerId] += p_scoreToAdd;
    }


}
