using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


public class JoinScreen : MonoBehaviour
{

    private enum ChangeCosmetic { Hat, LeftShoulder, RightShoulder, Chest, RightKnee, LeftKnee }
    private ChangeCosmetic m_selection;

    public int m_playerId;

    private bool m_inMatch;
    private bool m_selectingCosmetic;
    private Player m_playerInputController;

    public float m_changeThreshold;
    public GameObject m_playerObject;
    private PlayerIdManager m_playerIdManager;
    private int m_currentHat, m_currentLeftShoulder, m_currentRightShoulder, m_currentChest, m_currentRightKneepads, m_currentLeftKneepads;
    private bool m_canSwap = true, m_canSwap2 = true;

    public int m_nextSceneIndex;

    public SoundEvent m_join, m_disconnect;

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
                m_join.Invoke();
                PlayerManager.Instance.AssignPlayer(m_playerId, out m_playerObject);
                m_playerIdManager = m_playerObject.GetComponent<PlayerIdManager>();
                m_currentHat = m_currentLeftShoulder = m_currentRightShoulder = m_currentChest = m_currentRightKneepads = m_currentLeftKneepads = 0;
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
                m_disconnect.Invoke();
                PlayerManager.Instance.RemovePlayer(m_playerId);
                m_inMatch = false;
            }
        }
    }

    private void AlternateCosmetic(int p_dir)
    {
        switch (m_selection)
        {
            #region Hat
            case ChangeCosmetic.Hat:
                m_playerIdManager.RemoveHelmet();

                GameObject newHat = CosmeticManager.Instance.GetHat(m_playerId, ref m_currentHat, p_dir);
                if (newHat != null)
                {
                    m_playerIdManager.AssignHelmet(newHat);
                }
                break;
            #endregion

            #region Left Shoulder
            case ChangeCosmetic.LeftShoulder:

                m_playerIdManager.RemoveLeftShoulder();


                GameObject leftShould = CosmeticManager.Instance.GetLeftShoulder(m_playerId, ref m_currentLeftShoulder, p_dir);
                if (leftShould != null)
                {
                    m_playerIdManager.AssignLeftShoulder(leftShould);
                }

                break;
            #endregion

            #region Right Shoulder
            case ChangeCosmetic.RightShoulder:

                m_playerIdManager.RemoveRightShoulder();


                GameObject rightShoulder = CosmeticManager.Instance.GetRightShoulder(m_playerId, ref m_currentRightShoulder, p_dir);
                if (rightShoulder != null)
                {
                    m_playerIdManager.AssignRightShoulder(rightShoulder);
                }

                break;
            #endregion

            #region Chest 
            case ChangeCosmetic.Chest:
                m_playerIdManager.RemoveChest();

                GameObject newChest = CosmeticManager.Instance.GetChestPlate(m_playerId, ref m_currentChest, p_dir);
                if (newChest != null)
                {
                    m_playerIdManager.AssignChest(newChest);
                }
                break;
            #endregion

            #region Left Knee
            case ChangeCosmetic.LeftKnee:
                m_playerIdManager.RemoveLeftKneepad();

                GameObject newLeftKnees = CosmeticManager.Instance.GetLeftShoulder(m_playerId, ref m_currentLeftKneepads, p_dir);
                if (newLeftKnees != null)
                {
                    m_playerIdManager.AssignLeftKnee(newLeftKnees);
                }
                break;
            #endregion

            #region RightKnee
            case ChangeCosmetic.RightKnee:
                m_playerIdManager.RemoveRightKneepad();

                GameObject newRightKnees = CosmeticManager.Instance.GetRightShoulder(m_playerId, ref m_currentRightKneepads, p_dir);
                if (newRightKnees != null)
                {
                    m_playerIdManager.AssignRightKnee(newRightKnees);
                }
                break;
                #endregion

        }
    }

    private void SwitchSelection(int p_dir)
    {
        switch (m_selection)
        {
            case ChangeCosmetic.Hat:
                if (p_dir > 0)
                {
                    m_selection = ChangeCosmetic.Chest;
                }
                else
                {
                    m_selection = ChangeCosmetic.LeftShoulder;
                }
                break;
            case ChangeCosmetic.LeftShoulder:
                if (p_dir > 0)
                {
                    m_selection = ChangeCosmetic.Hat;
                }
                else
                {
                    m_selection = ChangeCosmetic.RightShoulder;
                }
                break;
            case ChangeCosmetic.RightShoulder:
                if (p_dir > 0)
                {
                    m_selection = ChangeCosmetic.LeftShoulder;
                }
                else
                {
                    m_selection = ChangeCosmetic.Chest;
                }
                break;
            case ChangeCosmetic.Chest:
                if (p_dir > 0)
                {
                    m_selection = ChangeCosmetic.RightShoulder;
                }
                else
                {
                    m_selection = ChangeCosmetic.LeftKnee;
                }
                break;
            case ChangeCosmetic.LeftKnee:
                if (p_dir > 0)
                {
                    m_selection = ChangeCosmetic.Chest;
                }
                else
                {
                    m_selection = ChangeCosmetic.RightKnee;
                }
                break;
            case ChangeCosmetic.RightKnee:
                if (p_dir > 0)
                {
                    m_selection = ChangeCosmetic.LeftKnee;
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
        PlayerManager.Instance.AssignPlayerCosmetics(m_playerId, CosmeticManager.Instance.GetHat(m_playerId, ref m_currentHat, 0),
                                                                    CosmeticManager.Instance.GetRightShoulder(m_playerId, ref m_currentRightShoulder, 0),
                                                                    CosmeticManager.Instance.GetLeftShoulder(m_playerId, ref m_currentLeftShoulder, 0),
                                                                    CosmeticManager.Instance.GetChestPlate(m_playerId, ref m_currentChest, 0),
                                                                    CosmeticManager.Instance.GetLeftKnee(m_playerId, ref m_currentLeftKneepads, 0),
                                                                    CosmeticManager.Instance.GetRightKnee(m_playerId, ref m_currentRightKneepads, 0));
    }
}
