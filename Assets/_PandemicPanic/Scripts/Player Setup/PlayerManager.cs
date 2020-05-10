using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    private int m_playerCount;

    public List<Transform> m_spawnPoints;

    public List<GameObject> m_playerPrefabs;

    public List<PlayerProperties> m_players = new List<PlayerProperties>();

    public class PlayerProperties
    {
        public GameObject m_helmet, m_rShoulder, m_lShoulder, m_chestPiece, m_leftKnee, m_rightKnee;
        public GameObject m_playerObject;
        public int m_playerID;
        public int m_score;

        public GameObject m_startingAvatar;
        public GameObject m_gameAvatar;

        public void RemoveFromList()
        {
            PlayerManager.Instance.m_playerPrefabs.Add(m_playerObject);
            Destroy(m_startingAvatar);
            PlayerManager.Instance.m_players.Remove(this);
        }
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void AssignPlayer(int p_playerID, out GameObject p_playerObject)
    {
        int randomPlayer = Random.Range(0, m_playerPrefabs.Count);
        PlayerProperties currentPlayer = new PlayerProperties();
        currentPlayer.m_playerID = p_playerID;
        currentPlayer.m_playerObject = m_playerPrefabs[randomPlayer];
        m_playerPrefabs.RemoveAt(randomPlayer);
        m_players.Add(currentPlayer);

        currentPlayer.m_startingAvatar = Instantiate(currentPlayer.m_playerObject, m_spawnPoints[m_players.IndexOf(currentPlayer)].position, m_spawnPoints[m_players.IndexOf(currentPlayer)].rotation);

        currentPlayer.m_startingAvatar.GetComponent<PlayerInput>().enabled = false;
        currentPlayer.m_startingAvatar.GetComponent<PlayerInput>().m_playerId = p_playerID;
        p_playerObject = currentPlayer.m_startingAvatar;
    }

    public void RemovePlayer(int p_playerID)
    {
        PlayerProperties removeMe = null;
        foreach (PlayerProperties player in m_players)
        {
            if (player.m_playerID == p_playerID)
            {
                removeMe = player;
                break;
            }
        }

        if (removeMe != null)
        {
            removeMe.RemoveFromList();
        }
    }

    public void AssignPlayerCosmetics(int p_playerId, GameObject p_headPiece, GameObject p_rShould, GameObject p_lShould, GameObject p_chest, GameObject p_leftKnee, GameObject p_rightKnee)
    {
        foreach (PlayerProperties player in m_players)
        {
            if (player.m_playerID == p_playerId)
            {
                player.m_helmet = p_headPiece;
                player.m_rShoulder = p_rShould;
                player.m_lShoulder = p_lShould;
                player.m_chestPiece = p_chest;
                player.m_leftKnee = p_leftKnee;
                player.m_rightKnee = p_rightKnee;
            }
        }
    }

    public void SpawnPlayers(List<Transform> p_spawns)
    {
        foreach (PlayerProperties player in m_players)
        {
            GameObject newPlayer = Instantiate(player.m_playerObject, p_spawns[m_players.IndexOf(player)].position, p_spawns[m_players.IndexOf(player)].rotation);
            newPlayer.GetComponent<PlayerIdManager>().AssignCosmetics(player.m_helmet, player.m_rShoulder, player.m_lShoulder, player.m_chestPiece, player.m_leftKnee,player.m_rightKnee);
            newPlayer.GetComponent<PlayerInput>().m_playerId = player.m_playerID;
            player.m_gameAvatar = newPlayer;
            m_playerCount += 1;
        }
    }

    public void ChangePlayerInputs(bool p_activeState)
    {
        foreach (PlayerProperties player in m_players)
        {
            player.m_gameAvatar.GetComponent<PlayerInput>().enabled = p_activeState;
        }
    }

    public int GetPlayerCount()
    {
        return m_playerCount;
    }

    public PlayerProperties GetPlayerProperties(int p_playerId)
    {
        foreach (PlayerProperties player in m_players)
        {
            if(player.m_playerID == p_playerId)
            {
                return player;
            }
        }
        return null;
    }

    public PlayerProperties GetPlayerPropertiesByIndex(int p_index)
    {
        return m_players[p_index];
    }
}
