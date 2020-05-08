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


}
