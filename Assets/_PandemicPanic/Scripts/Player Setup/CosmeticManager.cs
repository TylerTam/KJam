using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmeticManager : MonoBehaviour
{
    public static CosmeticManager Instance;

    
    [Tooltip("The individual's unlocks")]
    public List<UnlockedCosmetics> m_playerCosmetics;
    [System.Serializable]
    public class UnlockedCosmetics
    {

        public int m_playerId;
        public List<GameObject> m_headCosmetics, m_rightShoulder, m_leftShoulder, m_chestPlate;

    }

    [Tooltip("The reference for all the collectables")]
    public List<CosmeticUnlocks> m_unlockableCosmetics;

    [System.Serializable]
    public class CosmeticUnlocks
    {
        public string m_cosmeticName;
        public List<NewCosmetic> m_cosmeticReferences;
        
        public void CheckUnlock(int p_playerId)
        {
            switch (m_cosmeticReferences[0].m_myType)
            {
                case NewCosmetic.CosmeticType.Helmet:
                    CosmeticManager.Instance.AddHeadCosmetic(p_playerId, m_cosmeticReferences[0].m_cosmetic);
                    break;
                case NewCosmetic.CosmeticType.LShoulder:
                    CosmeticManager.Instance.AddLeftShoulderCosmetic(p_playerId, m_cosmeticReferences[0].m_cosmetic);
                    CosmeticManager.Instance.AddRightShoulderCosmetics(p_playerId, m_cosmeticReferences[1].m_cosmetic);
                    break;
                case NewCosmetic.CosmeticType.RShoulder:
                    CosmeticManager.Instance.AddLeftShoulderCosmetic(p_playerId, m_cosmeticReferences[0].m_cosmetic);
                    CosmeticManager.Instance.AddRightShoulderCosmetics(p_playerId, m_cosmeticReferences[1].m_cosmetic);
                    break;
                case NewCosmetic.CosmeticType.Chest:
                    CosmeticManager.Instance.AddChestPlate(p_playerId, m_cosmeticReferences[0].m_cosmetic);
                    break;
            }
        }

        [System.Serializable]
        public class NewCosmetic
        {
            public enum CosmeticType {  Helmet, RShoulder, LShoulder, Chest}
            public CosmeticType m_myType;
            public GameObject m_cosmetic;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CheckCosmetic (int p_playerId, int p_cosmeticType)
    {
        m_unlockableCosmetics[p_cosmeticType].CheckUnlock(p_playerId);
    }

    public void AddHeadCosmetic(int p_playerId,GameObject p_headPiece)
    {
        if (!m_playerCosmetics[p_playerId].m_headCosmetics.Contains(p_headPiece))
        {
            m_playerCosmetics[p_playerId].m_headCosmetics.Add(p_headPiece);
        }
    }
    public void AddRightShoulderCosmetics(int p_playerId, GameObject p_right)
    {
        if (!m_playerCosmetics[p_playerId].m_rightShoulder.Contains(p_right))
        {
            m_playerCosmetics[p_playerId].m_rightShoulder.Add(p_right);
        }
    }
    public void AddLeftShoulderCosmetic(int p_playerId, GameObject p_left)
    {
        if (!m_playerCosmetics[p_playerId].m_leftShoulder.Contains(p_left))
        {
            m_playerCosmetics[p_playerId].m_leftShoulder.Add(p_left);
        }
    }
    public void AddChestPlate(int p_playerId, GameObject p_chestPlate)
    {
        if (!m_playerCosmetics[p_playerId].m_chestPlate.Contains(p_chestPlate))
        {
            m_playerCosmetics[p_playerId].m_chestPlate.Add(p_chestPlate);
        } 
    }

    #region Toggling Between cosmetics on join screen

    public GameObject GetHat(int p_playerId, ref int p_index, int p_selectionDirection)
    {


        return m_playerCosmetics[p_playerId].m_headCosmetics[GetNewIndex(ref p_index,p_selectionDirection, m_playerCosmetics[p_playerId].m_headCosmetics.Count)];
    }
    public GameObject GetRightShoulder(int p_playerId, ref int p_index, int p_selectionDirection)
    {

        return m_playerCosmetics[p_playerId].m_rightShoulder[GetNewIndex(ref p_index, p_selectionDirection, m_playerCosmetics[p_playerId].m_rightShoulder.Count)];

    }
    public GameObject GetLeftShoulder(int p_playerId, ref int p_index, int p_selectionDirection)
    {
        return m_playerCosmetics[p_playerId].m_leftShoulder[GetNewIndex(ref p_index, p_selectionDirection, m_playerCosmetics[p_playerId].m_leftShoulder.Count)];
    }
    public GameObject GetChestPlate(int p_playerId, ref int p_index, int p_selectionDirection)
    {
        return m_playerCosmetics[p_playerId].m_chestPlate[GetNewIndex(ref p_index, p_selectionDirection, m_playerCosmetics[p_playerId].m_chestPlate.Count)];
    }

    private int GetNewIndex(ref int p_index, int p_selectionDirection, int p_listCount)
    {
        p_index += p_selectionDirection;
        if (p_index == p_listCount)
        {
            p_index = 0;
        }
        else if (p_index < 0)
        {
            p_index = p_listCount - 1;
        }
        return p_index;
    }

    #endregion
}
