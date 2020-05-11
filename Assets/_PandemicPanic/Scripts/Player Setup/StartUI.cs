using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUI : MonoBehaviour
{
    public static StartUI Instance;

    public GameObject m_pressStartToPlay;
    public List<UIElements> m_uiElements;

    public List<JoinScreen> m_players;

    

    [System.Serializable]
    public class UIElements
    {
        public StartUI_Container m_uiContainer;

        public void ChangeCosmeticType(string p_newType)
        {
            m_uiContainer.m_cosmeticTypeText.text = p_newType;
        }
        public void ChangeCosmeticName(string p_newName)
        {
            m_uiContainer.m_cosmeticNameText.text = p_newName;
        }

        public void Joined()
        {
            m_uiContainer.m_pressToJoin.SetActive(false);
            m_uiContainer.m_cosmeticChooser.SetActive(true);
        }
        public void SelectedItems()
        {
            m_uiContainer.m_cosmeticChooser.SetActive(false);
        }

        public void Leave()
        {
            m_uiContainer.m_cosmeticChooser.SetActive(false);
            m_uiContainer.m_pressToJoin.SetActive(true);
        }
    }

    private void Awake()
    {
        Instance = this;
    }
    bool m_canStart, m_startToggled = false;
    private void Update()
    {
        m_canStart = m_players.Count > 0;
        foreach(JoinScreen join in m_players)
        {
            if (!join.m_inMatch) continue;
            if (!join.m_isReady)
            {
                m_canStart = false;
            }
        }
        if (m_canStart != m_startToggled)
        {
            m_startToggled = m_canStart;
            foreach(JoinScreen join in m_players)
            {
                join.AllowStart(m_startToggled);
            }
            ToggleStart(m_startToggled);
        }
    }
    public UIElements GetUIElements(int p_indexId)
    {
        return m_uiElements[p_indexId];
    }

    public void ToggleStart(bool p_activeState)
    {
        m_pressStartToPlay.SetActive(p_activeState);
    }
}
