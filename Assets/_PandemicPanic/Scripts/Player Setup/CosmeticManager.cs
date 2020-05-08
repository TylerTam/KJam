using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmeticManager : MonoBehaviour
{
    public static CosmeticManager Instance;

    public List<GameObject> m_headCosmetics, m_rightShoulder, m_leftShoulder, m_chestPlate;
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

    public void AddHeadCosmetic(GameObject p_headPiece)
    {
        if (!m_headCosmetics.Contains(p_headPiece))
        {
            m_headCosmetics.Add(p_headPiece);
        }
    }
    public void AddShoulderCosmetics(GameObject p_right, GameObject p_left)
    {
        if (!m_rightShoulder.Contains(p_right))
        {
            m_rightShoulder.Add(p_right);
        }
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
