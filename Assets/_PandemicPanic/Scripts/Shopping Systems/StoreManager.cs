using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance;


    public List<Transform> m_spawnPoints;

    public float m_startDelay = 3;
    public float m_gameTime;
    public float m_endGameTime = 3;

    [Header("Food Items")]
    public List<FoodObject> m_foodItems;

    [Header("UI Elements")]
    public List<GameObject> m_playerUIContainer;
    public List<string> m_uiColorNames;
    public List<Transform> m_uiPos;
    public UnityEngine.UI.Text m_countdownText, m_matchTimeText, m_winningPlayerText;

    private List<PlayerScores> m_playerScores = new List<PlayerScores>();

    private CosmeticAppearManager m_cosmetics;


    [System.Serializable]
    public class PlayerScores
    {
        public int m_playerId;
        public int m_playerColorType;
        public int m_score;
        public UnityEngine.UI.Text m_scoreText;
    }

    private void Awake()
    {
        Instance = this;
        m_cosmetics = GetComponent<CosmeticAppearManager>();
    }
    private void Start()
    {
        PlayerManager.Instance.SpawnPlayers(m_spawnPoints);

        SetupCamera();

        PlayerManager.Instance.ChangePlayerInputs(false);
        CreateScoreMenu();
        StartCoroutine(StartMatchTime());
    }

    private void SetupCamera()
    {
        List<GameObject> playerRoots = new List<GameObject>();
        for (int i = 0; i < PlayerManager.Instance.GetPlayerCount(); i++)
        {
            playerRoots.Add(PlayerManager.Instance.GetPlayerPropertiesByIndex(i).m_gameAvatar.GetComponent<APRController>().Root);
        }
        CameraBehaviour.Instance.AssignPlayer(playerRoots);
    }
    private void CreateScoreMenu()
    {
        for (int i = 0; i < PlayerManager.Instance.GetPlayerCount(); i++)
        {
            GameObject uiElement = Instantiate(m_playerUIContainer[i], m_uiPos[i]);
            PlayerScores newScore = new PlayerScores();


            newScore.m_playerId = PlayerManager.Instance.GetPlayerPropertiesByIndex(i).m_playerID;
            newScore.m_scoreText = uiElement.GetComponentInChildren<UnityEngine.UI.Text>();
            newScore.m_scoreText.text = "0";
            newScore.m_playerColorType = PlayerManager.Instance.GetPlayerPropertiesByIndex(i).m_gameAvatar.GetComponent<PlayerIdManager>().m_playerType;
            m_playerScores.Add(newScore);
        }
    }

    public void AddScore(int p_playerId, int p_scoreToAdd)
    {
        foreach (PlayerScores player in m_playerScores)
        {
            if (player.m_playerId == p_playerId)
            {
                player.m_score += p_scoreToAdd;
                player.m_scoreText.text = player.m_score.ToString("F0");
                return;
            }
        }
    }

    private void StartMatch()
    {
        PlayerManager.Instance.ChangePlayerInputs(true);
        m_countdownText.gameObject.SetActive(false);
    }

    private IEnumerator StartMatchTime()
    {
        float timer = m_startDelay;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            m_countdownText.text = timer.ToString("F0");
            yield return null;
        }
        StartMatch();
        StartCoroutine(MatchTime());
    }

    private IEnumerator MatchTime()
    {
        float timer = m_gameTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            m_matchTimeText.text = timer.ToString("F0");
            m_cosmetics.CheckCosmeticAppearTime(timer);
            yield return null;
        }
        GameOver();
    }

    private void GameOver()
    {
        PlayerManager.Instance.ChangePlayerInputs(false);

        m_winningPlayerText.gameObject.SetActive(true);
        m_winningPlayerText.text = GetWinningPlayer() + " won!";

        StartCoroutine(EndGameTimer());
    }

    private string GetWinningPlayer()
    {
        int m_currentScore = 0;
        int m_heldIndex = 0;
        foreach(PlayerScores score in m_playerScores)
        {
            if(score.m_score > m_currentScore)
            {
                m_heldIndex = m_playerScores.IndexOf(score);
                m_currentScore = score.m_score;
            }
        }
        return "Shopper " + (m_heldIndex + 1).ToString();
    }

    private IEnumerator EndGameTimer()
    {
        yield return new WaitForSeconds(m_endGameTime);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public List<FoodObject> GetRandomFoodObjects(int p_amount)
    {
        List<FoodObject> randomFood = new List<FoodObject>();
        List<FoodObject> activeFoods = new List<FoodObject>(m_foodItems);
        if(p_amount > activeFoods.Count)
        {
            p_amount = activeFoods.Count;
        }
        for (int i = 0; i < p_amount; i++)
        {
            randomFood.Add(activeFoods[Random.Range(0, activeFoods.Count)]);
            activeFoods.Remove(randomFood[i]);
        }
        return randomFood;

    }

    public void RemoveDeactiveFood(FoodObject p_remove)
    {
        if (m_foodItems.Contains(p_remove))
        {
            m_foodItems.Remove(p_remove);
        }
        else
        {
            Debug.Log(p_remove.gameObject.name + " is missing from the store manager food list");
        }
    }
}
