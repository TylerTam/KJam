using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class JoinScreen : MonoBehaviour
{

    private enum ChangeCosmetic {Hat, Shoulder, Chest }
    private ChangeCosmetic m_selection;

    public int m_playerId;

    private bool m_inMatch;
    private bool m_selectingCosmetic;
    private Player m_playerInputController;

    public float m_changeThreshold;
    public GameObject m_playerObject;
    private PlayerIdManager m_playerIdManager;
    private int m_currentHat, m_currentShoulder, m_currentChest;
    private bool m_canSwap = true, m_canSwap2 = true;

    public int m_nextSceneIndex;

    private void Start()
    {
        m_playerInputController = ReInput.players.GetPlayer(m_playerId);
    }
    private void Update()
    {

        if (!m_inMatch)
        {
            if (m_playerInputController.GetButtonDown("Jump"))
            {
                PlayerManager.Instance.AssignPlayer(m_playerId, out m_playerObject);
                m_playerIdManager = m_playerObject.GetComponent<PlayerIdManager>();
                m_currentHat = m_currentShoulder = m_currentChest = 0;
                m_inMatch = true;
                m_selectingCosmetic = true;
            }
        }
        else
        {
            if (m_selectingCosmetic)
            {
                if (Mathf.Abs(m_playerInputController.GetAxis("MoveHorizon")) > m_changeThreshold && m_canSwap)
                {
                    AlternateCosmetic((int)Mathf.Sign(m_playerInputController.GetAxis("MoveHorizon")));
                    m_canSwap = false;
                }
                else if (Mathf.Abs(m_playerInputController.GetAxis("MoveHorizon")) < m_changeThreshold)
                {
                    m_canSwap = true;
                }
                if (Mathf.Abs(m_playerInputController.GetAxis("MoveVertical")) > m_changeThreshold && m_canSwap2)
                {
                    SwitchSelection((int)Mathf.Sign(m_playerInputController.GetAxis("MoveVertical")));
                    m_canSwap2 = false;
                }
                else if (Mathf.Abs(m_playerInputController.GetAxis("MoveVertical")) < m_changeThreshold)
                {
                    m_canSwap2 = true;
                }
                if (m_playerInputController.GetButtonDown("Jump"))
                {
                    m_selectingCosmetic = false;
                    m_playerObject.GetComponent<PlayerInput>().enabled = true;
                    AssignPlayerCosmetics();
                }
            }
            else
            {
                if (m_playerInputController.GetButtonDown("Start"))
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(m_nextSceneIndex);
                }
            }


            if (m_playerInputController.GetButtonDown("Ragdoll"))
            {
                PlayerManager.Instance.RemovePlayer(m_playerId);
                m_inMatch = false;
            }
        }
    }

    private void AlternateCosmetic(int p_dir)
    {
        switch (m_selection)
        {
            case ChangeCosmetic.Hat:
                m_playerIdManager.RemoveHelmet();
                
                GameObject newHat = CosmeticManager.Instance.GetHat(m_playerId,ref m_currentHat, p_dir);
                if(newHat != null)
                {
                    m_playerIdManager.AssignHelmet(newHat);
                }
                break;
            case ChangeCosmetic.Shoulder:
                #region Shoulders
                m_playerIdManager.RemoveShoulders();


                GameObject rightShould = CosmeticManager.Instance.GetRightShoulder(m_playerId, ref m_currentShoulder, p_dir);
                GameObject leftShould = CosmeticManager.Instance.GetLeftShoulder(m_playerId, ref m_currentShoulder, 0);
                if (rightShould != null)
                {
                    m_playerIdManager.AssignShoulders(rightShould, leftShould);
                }
                #endregion
                break;
            case ChangeCosmetic.Chest:
                m_playerIdManager.RemoveChest();

                GameObject newChest = CosmeticManager.Instance.GetChestPlate(m_playerId,ref m_currentChest, p_dir);
                if (newChest != null)
                {
                    m_playerIdManager.AssignChest(newChest);
                }
                break;
        }
    }

    private void SwitchSelection(int p_dir)
    {
        switch (m_selection)
        {
            case ChangeCosmetic.Hat:
                if(p_dir > 0)
                {
                    m_selection = ChangeCosmetic.Chest;
                }
                else
                {
                    m_selection = ChangeCosmetic.Shoulder;
                }
                break;
            case ChangeCosmetic.Shoulder:
                if (p_dir > 0)
                {
                    m_selection = ChangeCosmetic.Hat;
                }
                else
                {
                    m_selection = ChangeCosmetic.Chest;
                }
                break;
            case ChangeCosmetic.Chest:
                if (p_dir > 0)
                {
                    m_selection = ChangeCosmetic.Shoulder;
                }
                else
                {
                    m_selection = ChangeCosmetic.Hat;
                }
                break;
        }
    }

    private void AssignPlayerCosmetics()
    {
        PlayerManager.Instance.AssignPlayerCosmetics(m_playerId, CosmeticManager.Instance.GetHat(m_playerId,ref m_currentHat, 0), CosmeticManager.Instance.GetRightShoulder(m_playerId, ref m_currentShoulder, 0), CosmeticManager.Instance.GetLeftShoulder(m_playerId, ref m_currentShoulder,0), CosmeticManager.Instance.GetChestPlate(m_playerId, ref m_currentChest, 0));
    }
}
