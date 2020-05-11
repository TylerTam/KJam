using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


public class JoinScreen : MonoBehaviour
{

    private enum ChangeCosmetic { Hat, LeftShoulder, RightShoulder, Chest, RightKnee, LeftKnee }
    private ChangeCosmetic m_selection;

    public int m_playerId;

    public bool m_inMatch;
    public bool m_isReady;
    private bool m_allowStart;
    private bool m_selectingCosmetic;
    private Player m_playerInputController;

    public float m_changeThreshold;
    public GameObject m_playerObject;
    private PlayerIdManager m_playerIdManager;
    private int m_currentHat, m_currentLeftShoulder, m_currentRightShoulder, m_currentChest, m_currentRightKneepads, m_currentLeftKneepads;
    private bool m_canSwap = true, m_canSwap2 = true;

    public int m_nextSceneIndex;

    public SoundEvent m_join, m_disconnect;

    private StartUI.UIElements m_playerUi;
    private void Start()
    {
        m_playerInputController = ReInput.players.GetPlayer(m_playerId);
    }
    private void Update()
    {

        if (!m_inMatch)
        {
            if (m_playerInputController.GetButtonDown("Jump") && PlayerManager.Instance.m_players.Count < 4)
            {
                m_join.Invoke();
                PlayerManager.Instance.AssignPlayer(m_playerId, out m_playerObject);
                m_playerIdManager = m_playerObject.GetComponent<PlayerIdManager>();
                m_playerUi = StartUI.Instance.GetUIElements(PlayerManager.Instance.m_players.Count - 1);
                m_playerUi.Joined();
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
                    m_playerUi.SelectedItems();
                    m_isReady = true;
                }
            }
            else
            {

                if (m_playerInputController.GetButtonDown("Start") && m_allowStart)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(m_nextSceneIndex);
                }

            }


            if (m_playerInputController.GetButtonDown("Ragdoll"))
            {
                m_disconnect.Invoke();
                PlayerManager.Instance.RemovePlayer(m_playerId);
                m_playerUi.Leave();
                m_playerUi = null;
                m_inMatch = false;
                m_playerUi = null;
                m_isReady = false;
            }
        }
    }

    public void AllowStart(bool p_activeState)
    {
        m_allowStart = p_activeState;
    }
    private void AlternateCosmetic(int p_dir)
    {
        GameObject newCosmetic = null;
        switch (m_selection)
        {
            #region Hat
            case ChangeCosmetic.Hat:
                m_playerIdManager.RemoveHelmet();

                newCosmetic = CosmeticManager.Instance.GetHat(m_playerId, ref m_currentHat, p_dir);
                if (newCosmetic != null)
                {
                    m_playerIdManager.AssignHelmet(newCosmetic);
                }
                break;
            #endregion

            #region Left Shoulder
            case ChangeCosmetic.LeftShoulder:

                m_playerIdManager.RemoveLeftShoulder();


                newCosmetic = CosmeticManager.Instance.GetLeftShoulder(m_playerId, ref m_currentLeftShoulder, p_dir);
                if (newCosmetic != null)
                {
                    m_playerIdManager.AssignLeftShoulder(newCosmetic);
                }

                break;
            #endregion

            #region Right Shoulder
            case ChangeCosmetic.RightShoulder:

                m_playerIdManager.RemoveRightShoulder();


                newCosmetic = CosmeticManager.Instance.GetRightShoulder(m_playerId, ref m_currentRightShoulder, p_dir);
                if (newCosmetic != null)
                {
                    m_playerIdManager.AssignRightShoulder(newCosmetic);
                }

                break;
            #endregion

            #region Chest 
            case ChangeCosmetic.Chest:
                m_playerIdManager.RemoveChest();

                newCosmetic = CosmeticManager.Instance.GetChestPlate(m_playerId, ref m_currentChest, p_dir);
                if (newCosmetic != null)
                {
                    m_playerIdManager.AssignChest(newCosmetic);
                }
                break;
            #endregion

            #region Left Knee
            case ChangeCosmetic.LeftKnee:
                m_playerIdManager.RemoveLeftKneepad();

                newCosmetic = CosmeticManager.Instance.GetLeftShoulder(m_playerId, ref m_currentLeftKneepads, p_dir);
                if (newCosmetic != null)
                {
                    m_playerIdManager.AssignLeftKnee(newCosmetic);
                }
                break;
            #endregion

            #region RightKnee
            case ChangeCosmetic.RightKnee:
                m_playerIdManager.RemoveRightKneepad();

                newCosmetic = CosmeticManager.Instance.GetRightShoulder(m_playerId, ref m_currentRightKneepads, p_dir);
                if (newCosmetic != null)
                {
                    m_playerIdManager.AssignRightKnee(newCosmetic);
                }
                break;
                #endregion

        }
        m_playerUi.ChangeCosmeticName((newCosmetic == null) ? "No Item" : newCosmetic.GetComponent<CosmeticItem>().m_cosmeticName);
    }


    private void SwitchSelection(int p_dir)
    {
        switch (m_selection)
        {
            case ChangeCosmetic.Hat:
                if (p_dir > 0)
                {
                    ChangeSelection(ChangeCosmetic.Chest);
                }
                else
                {
                    ChangeSelection(ChangeCosmetic.LeftShoulder);
                }
                break;
            case ChangeCosmetic.LeftShoulder:
                if (p_dir > 0)
                {
                    ChangeSelection(ChangeCosmetic.Hat);
                }
                else
                {
                    ChangeSelection(ChangeCosmetic.RightShoulder);
                }
                break;
            case ChangeCosmetic.RightShoulder:
                if (p_dir > 0)
                {
                    ChangeSelection(ChangeCosmetic.LeftShoulder);
                }
                else
                {
                    ChangeSelection(ChangeCosmetic.Chest);
                }
                break;
            case ChangeCosmetic.Chest:
                if (p_dir > 0)
                {
                    ChangeSelection(ChangeCosmetic.RightShoulder);
                }
                else
                {
                    ChangeSelection(ChangeCosmetic.LeftKnee);
                }
                break;
            case ChangeCosmetic.LeftKnee:
                if (p_dir > 0)
                {
                    ChangeSelection(ChangeCosmetic.Chest);
                }
                else
                {
                    ChangeSelection(ChangeCosmetic.RightKnee);
                }
                break;
            case ChangeCosmetic.RightKnee:
                if (p_dir > 0)
                {
                    ChangeSelection(ChangeCosmetic.LeftKnee);
                }
                else
                {
                    ChangeSelection(ChangeCosmetic.Hat);
                }
                break;

        }
    }

    private void ChangeSelection(ChangeCosmetic p_newType)
    {
        m_selection = p_newType;
        m_playerUi.ChangeCosmeticType(p_newType.ToString());
        AlternateCosmetic(0);
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
