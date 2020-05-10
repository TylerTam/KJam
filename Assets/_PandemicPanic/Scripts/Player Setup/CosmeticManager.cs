using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmeticManager : MonoBehaviour
{
    public static CosmeticManager Instance;

    public List<GameObject> m_headCosmetics, m_rightShoulder, m_leftShoulder, m_chestPlate;

    public List<CosmeticUnlocks> m_unlockableCosmetics;

    [System.Serializable]
    public class CosmeticUnlocks
    {
        
        public List<NewCosmetic> m_cosmeticUnlock;
        public int m_numberToUnlock;

        public int m_currentCollected;
        private bool m_unlocked;
        public void CheckUnlock()
        {
            if (m_unlocked) return;
            m_currentCollected++;
            if(m_currentCollected >= m_numberToUnlock)
            {
                m_unlocked = true;
                foreach(NewCosmetic cos in m_cosmeticUnlock)
                {
                    switch (cos.m_myType)
                    {
                        case NewCosmetic.CosmeticType.Helmet:
                            CosmeticManager.Instance.AddHeadCosmetic(cos.m_cosmetic);
                            break;
                        case NewCosmetic.CosmeticType.LShoulder:
                            CosmeticManager.Instance.AddLeftShoulderCosmetic(cos.m_cosmetic);
                            break;
                        case NewCosmetic.CosmeticType.RShoulder:
                            CosmeticManager.Instance.AddRightShoulderCosmetics(cos.m_cosmetic);
                            break;
                        case NewCosmetic.CosmeticType.Chest:
                            CosmeticManager.Instance.AddChestPlate(cos.m_cosmetic);
                            break;
                    }
                }
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

    public void CheckCosmetic (int p_cosmeticType)
    {
        m_unlockableCosmetics[p_cosmeticType].CheckUnlock();
    }

    public void AddHeadCosmetic(GameObject p_headPiece)
    {
        if (!m_headCosmetics.Contains(p_headPiece))
        {
            m_headCosmetics.Add(p_headPiece);
        }
    }
    public void AddRightShoulderCosmetics(GameObject p_right)
    {
        if (!m_rightShoulder.Contains(p_right))
        {
            m_rightShoulder.Add(p_right);
        }
    }
    public void AddLeftShoulderCosmetic(GameObject p_left)
    {
        if (!m_leftShoulder.Contains(p_left))
        {
            m_leftShoulder.Add(p_left);
        }
    }
    public void AddChestPlate(GameObject p_chestPlate)
    {
        if (!m_chestPlate.Contains(p_chestPlate))
        {
            m_chestPlate.Add(p_chestPlate);
        } 
    }

    public GameObject GetHat(int p_index)
    {
        return m_headCosmetics[p_index];
    }
    public GameObject GetRightShoulder(int p_index)
    {
        return m_rightShoulder[p_index];

    }
    public GameObject GetLeftShoulder(int p_index)
    {
        return m_leftShoulder[p_index];
    }
    public GameObject GetChestPlate(int p_index)
    {
        return m_chestPlate[p_index];
    }

}
